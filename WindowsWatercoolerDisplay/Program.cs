using HidLibrary;
using LibreHardwareMonitor.Hardware;

namespace WindowsWatercoolerDisplay
{
    /// <summary>
    /// Gerencia o ciclo de vida do aplicativo na bandeja do sistema e a thread de monitoramento em segundo plano.
    /// </summary>
    public class TrayApplicationContext : ApplicationContext
    {
        private readonly NotifyIcon _trayIcon;
        private readonly Computer _computer;
        private readonly CancellationTokenSource _cancellationTokenSource;

        // Identificadores de hardware do display USB do watercooler
        private const int VendorId = 0xAA88;
        private const int ProductId = 0x8666;

        public TrayApplicationContext()
        {
            _trayIcon = new NotifyIcon
            {
                // Extrai o ícone associado ao próprio executável em tempo de execução
                Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath),
                ContextMenuStrip = new ContextMenuStrip(),
                Visible = true,
                Text = "Watercooler: Iniciando leitura..."
            };

            _trayIcon.ContextMenuStrip.Items.Add("Sair", null, ExitApp);

            _computer = new Computer
            {
                IsCpuEnabled = true,
                IsMotherboardEnabled = true
            };
            
            try 
            {
                _computer.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao abrir monitoramento. Execute como administrador.\n\nDetalhe: {ex.Message}", "Erro de Inicialização", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExitApp(this, EventArgs.Empty);
                return;
            }

            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => UpdateLoop(_cancellationTokenSource.Token));
        }

        private async Task UpdateLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    int currentTemp = GetCpuTemperature();
            
                    if (currentTemp > 0)
                    {
                        _trayIcon.Text = $"Watercooler: {currentTemp}°C";
                        SendTemperatureToDevice(currentTemp);
                    }
                    else
                    {
                        _trayIcon.Text = "Watercooler: Aguardando sensor...";
                    }
                }
                catch (Exception ex)
                {
                    // Agora o erro vai aparecer no texto do ícone do relógio!
                    _trayIcon.Text = $"Erro: {ex.Message}";
                }

                await Task.Delay(1000, token);
            }
        }

        private void SendTemperatureToDevice(int temperature)
        {
            var device = HidDevices.Enumerate(VendorId, ProductId).FirstOrDefault();

            if (device != null)
            {
                // 1. Precisamos ABRIR a porta antes de falar com ela
                device.OpenDevice();

                if (device.IsOpen)
                {
                    byte[] payload = new byte[9] { 0x00, (byte)temperature, 0x1C, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00 };

                    for (int i = 0; i < 5; i++)
                    {
                        device.Write(payload);
                        Thread.Sleep(50); 
                    }
            
                    // 2. Fechamos para não dar conflito no próximo loop
                    device.CloseDevice();
                }
                else
                {
                    _trayIcon.Text = "Erro: Dispositivo bloqueado pelo Windows";
                }
            }
            else
            {
                _trayIcon.Text = "Erro: USB não encontrado";
            }
        }

        /// <summary>
        /// Varre os sensores de hardware e retorna a maior temperatura real encontrada na CPU.
        /// </summary>
        private int GetCpuTemperature()
        {
            int maxTemp = 0;

            void BuscarSensores(IHardware hw)
            {
                hw.Update();

                foreach (var sensor in hw.Sensors)
                {
                    if (sensor.SensorType == SensorType.Temperature && sensor.Value.HasValue)
                    {
                        // Escreve no console do Rider todos os sensores de temperatura encontrados
                        System.Diagnostics.Debug.WriteLine($"[SENSOR ENCONTRADO] {hw.Name} -> {sensor.Name} = {sensor.Value.Value}°C");

                        string sName = sensor.Name.ToLower();

                        // Ignora sensores de limite térmico ("Distance to TjMax")
                        if (sName.Contains("distance"))
                            continue;

                        // Filtra pelo nome OU garante que é um sensor do próprio processador
                        if (sName.Contains("cpu") || sName.Contains("core") || 
                            sName.Contains("package") || sName.Contains("tdie") || 
                            sName.Contains("tctl") || sName.Contains("ccd") ||
                            hw.HardwareType == HardwareType.Cpu) // <-- O pulo do gato para CPUs novas!
                        {
                            int currentTemp = (int)Math.Round(sensor.Value.Value);
                            if (currentTemp > maxTemp)
                            {
                                maxTemp = currentTemp;
                            }
                        }
                    }
                }

                foreach (var subHw in hw.SubHardware)
                {
                    BuscarSensores(subHw);
                }
            }

            foreach (var hardware in _computer.Hardware)
            {
                BuscarSensores(hardware);
            }
    
            return maxTemp; 
        }



        private void ExitApp(object? sender, EventArgs e)
        {
            _trayIcon.Visible = false;
            _cancellationTokenSource?.Cancel();
            _computer?.Close();
            Application.Exit();
        }
    }

    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TrayApplicationContext());
        }
    }
}