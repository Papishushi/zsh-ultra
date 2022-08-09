# zsh-ultra [![CodeQL](https://github.com/Papishushi/zsh-ultra/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/Papishushi/zsh-ultra/actions/workflows/codeql-analysis.yml) [![.NET](https://github.com/Papishushi/zsh-ultra/actions/workflows/dotnet.yml/badge.svg?branch=master)](https://github.com/Papishushi/zsh-ultra/actions/workflows/dotnet.yml)


On development GNU CLI powered by .NET, based on zsh.

## Install:
Follow the steps above to install this program with install.sh :

### Check the script and make sure everything is normal and safe.

     echo "$(wget -O- https://raw.githubusercontent.com/Papishushi/zsh-ultra/master/installer/install.sh)"

### Now that we are sure everything is safe, run the script.

     sh -c "$(wget -O- https://raw.githubusercontent.com/Papishushi/zsh-ultra/master/installer/install.sh)" 
     
## Remarks:
To use this program you must install the .NET runtime using sudo. 
I dont want to include a sudo command on the installer so you must do it by yourself.

    sudo apt-get update && \
      sudo apt-get install -y aspnetcore-runtime-6.0
