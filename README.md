# Windows CPU Cooler Display Controller

*[Português](#português) | [English](#english)*

A native Windows script to control USB CPU temperature displays, using WMI to read hardware sensors and PyUSB for device communication.

---

## English

### 📋 Prerequisites

- Windows 10 or 11
- Python 3.10+ (Make sure to check "Add Python to PATH" during installation)
- USB temperature display device (VID: 0xaa88, PID: 0x8666)
- **Zadig** (Required to replace the default Windows USB driver)

### ⚠️ Important: USB Driver Setup (Zadig)

Windows installs a default driver that prevents Python (`pyusb`) from communicating directly with the display. You **must** change this driver:

1. Download [Zadig](https://zadig.akeo.ie/).
2. Open Zadig, click on **Options** -> **List All Devices**.
3. In the dropdown, find your USB display (look for ID `aa88:8666` or similar generic USB names).
4. Select **WinUSB** as the target driver and click **Replace Driver**.

### 🚀 Automatic Installation

1. Download or clone this repository to a folder on your computer.
2. Double-click the `windows_install.bat` file.
3. The script will automatically:
   - Create a Python virtual environment.
   - Install all required libraries (`wmi`, `pyusb`, etc.).
   - Create an invisible startup shortcut so the display works automatically every time you turn on your PC.

### 🗑️ Uninstallation

To completely remove the background service:
1. Press `Win + R`, type `shell:startup`, and press Enter.
2. Delete the file named `CPUCoolerDisplay.vbs`.
3. You can now safely delete the project folder.

---

## Português

### 📋 Pré-requisitos

- Windows 10 ou 11
- Python 3.10+ (Certifique-se de marcar "Add Python to PATH" durante a instalação)
- Dispositivo USB de display de temperatura (VID: 0xaa88, PID: 0x8666)
- **Zadig** (Necessário para substituir o driver USB padrão do Windows)

### ⚠️ Importante: Configuração do Driver USB (Zadig)

O Windows instala um driver padrão que impede o Python (`pyusb`) de se comunicar diretamente com o visor. Você **precisa** alterar este driver:

1. Baixe o [Zadig](https://zadig.akeo.ie/).
2. Abra o Zadig, clique em **Options** -> **List All Devices**.
3. Na lista, encontre o seu visor USB (procure pelo ID `aa88:8666` ou nomes genéricos de USB).
4. Selecione **WinUSB** como o driver de destino e clique em **Replace Driver**.

### 🚀 Instalação Automática

1. Baixe ou clone este repositório para uma pasta no seu computador.
2. Dê um duplo clique no arquivo `windows_install.bat`.
3. O script irá automaticamente:
   - Criar um ambiente virtual Python.
   - Instalar todas as bibliotecas necessárias (`wmi`, `pyusb`, etc.).
   - Criar um atalho de inicialização invisível para que o visor funcione automaticamente toda vez que você ligar o PC.

### 🗑️ Desinstalação

Para remover completamente o serviço que roda em segundo plano:
1. Pressione `Win + R`, digite `shell:startup` e pressione Enter.
2. Apague o arquivo chamado `CPUCoolerDisplay.vbs`.
3. Agora você pode excluir a pasta do projeto com segurança.