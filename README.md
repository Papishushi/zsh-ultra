# zsh-ultra [![CodeQL](https://github.com/Papishushi/zsh-ultra/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/Papishushi/zsh-ultra/actions/workflows/codeql-analysis.yml) [![.NET](https://github.com/Papishushi/zsh-ultra/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Papishushi/zsh-ultra/actions/workflows/dotnet.yml)


On development multiplatform GNU CLI powered by .NET, based on zsh.

# Install:

## Linux | Unix | MacOS:
## Follow the steps below to install this program with [install.sh](https://raw.githubusercontent.com/Papishushi/zsh-ultra/master/installer/install.sh):

### Check the script and make sure everything is normal and safe.

     echo "$(wget -O- https://raw.githubusercontent.com/Papishushi/zsh-ultra/master/installer/install.sh)"

### Now that we are sure everything is safe, run the script.

     zsh -c "$(wget -O- https://raw.githubusercontent.com/Papishushi/zsh-ultra/master/installer/install.sh)" 
     
### Remarks:
wget is a requierement for the installer, so if you are going to use it, you need to sudo install it like this:

     sudo apt-get install wget

To use this program you must install the .NET runtime using sudo. 
I dont want to include a sudo command on the installer so you must do it by yourself.

    sudo apt-get update && \
      sudo apt-get install -y aspnetcore-runtime-6.0

## Follow the steps below to install this program manually:

### Clone or download the master repo.

    git clone git@github.com:Papishushi/zsh-ultra.git
    
or manually download the repo then unzip-it using unzip command or another command with the same purpose.

    unzip master.zip
    
### Compile the code using dotnet build.

    dotnet build --configuration Release zsh-ultra-master
    
### Move compiled program to $HOME/.zshultra and do some cleanup

    mkdir -pm 755 $HOME/.zshultra
    mv zsh-ultra-master/bin/Release/net6.0/* $HOME/.zshultra
    rm -rf master.zip
    rm -rf zsh-ultra-master
    
### Set PATH enviroment variable in your zsh profile (.profile | .zshrc)

     export PATH=$PATH:$HOME/.zshultra
     
### Remarks:
To use this program you must install the .NET runtime using sudo. 

    sudo apt-get update && \
      sudo apt-get install -y aspnetcore-runtime-6.0     
