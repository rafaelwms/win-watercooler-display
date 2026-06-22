# Windows Watercooler Display Controller

*[Português](#português) | [English](#english)*

A native Windows System Tray application built in .NET 8 (C#) to monitor CPU temperatures and control USB Watercooler displays (VID: 0xAA88, PID: 0x8666).

---

## English

### 🌟 Features
- **Native Execution:** Built with C# and .NET 8.
- **Silent Background Process:** Runs discreetly in the Windows System Tray.
- **Modern Hardware Support:** Uses `LibreHardwareMonitor` (v0.9.6+) to detect temperatures even on the latest CPU architectures.
- **Plug & Play (No Zadig Required):** Communicates directly via native Windows HID.

### 🚀 Installation (Release)
1. Go to the [Releases](../../releases) page and download the latest `.exe` or `.zip` file.
2. Extract the file to a folder of your choice.
3. Right-click the executable and select **"Run as Administrator"** (Required for hardware temperature reading).
4. Check your System Tray! The icon will display the current temperature and send it to your watercooler.

### ⚠️ Troubleshooting (The Zadig Trap)
If you previously used a Python script or installed a custom driver via **Zadig** (like `WinUSB` or `libusb-win32`), this application **will not find your device**. 
To fix this:
1. Open Windows **Device Manager**.
2. Find your display (usually under `libusb-win32 devices` or Universal Serial Bus devices).
3. Right-click -> **Uninstall device**.
4. **CRITICAL:** Check the box that says **"Attempt to remove the driver for this device"**.
5. Click "Scan for hardware changes". The device should return as a standard Human Interface Device (HID).

---

## Português

### 🌟 Funcionalidades
- **Execução Nativa:** Construído em C# com .NET 8.
- **Processo Silencioso:** Roda discretamente na bandeja do sistema (System Tray) do Windows.
- **Suporte a Hardware Moderno:** Utiliza o `LibreHardwareMonitor` (v0.9.6+) para detectar temperaturas até nas arquiteturas de CPU mais recentes.
- **Plug & Play (Sem Zadig):** Comunicação direta via HID nativo do Windows.

### 🚀 Instalação (Release)
1. Vá até a página de [Releases](../../releases) e baixe o arquivo `.exe` ou `.zip` mais recente.
2. Extraia para uma pasta da sua preferência.
3. Clique com o botão direito no executável e selecione **"Executar como Administrador"** (Obrigatório para leitura dos sensores físicos).
4. Olhe a bandeja do seu sistema (perto do relógio)! O ícone mostrará a temperatura e a enviará para o watercooler.

### ⚠️ Solução de Problemas (A Armadilha do Zadig)
Se você usou scripts em Python anteriormente ou instalou um driver customizado via **Zadig** (como `WinUSB` ou `libusb-win32`), este aplicativo **não conseguirá encontrar o seu visor**.
Para resolver:
1. Abra o **Gerenciador de Dispositivos** do Windows.
2. Encontre o seu visor (geralmente em `libusb-win32 devices` ou Dispositivos USB).
3. Clique com o botão direito -> **Desinstalar dispositivo**.
4. **CRÍTICO:** Marque a caixinha **"Tentar remover o driver deste dispositivo"**.
5. Clique em "Verificar se há alterações de hardware". O dispositivo voltará a ser um Dispositivo de Interface Humana (HID) padrão.