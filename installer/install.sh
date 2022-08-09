#!/bin/bash

#Local variables
DOTNET_ROOT=$HOME/.dotnet
DOTNET=$DOTNET_ROOT/dotnet
TEMP=$HOME/tempfilesinstaller

#Make directory to contain all temp files.
mkdir -pm a+wrx $TEMP

#Get ZSH-Ultra source code compressed on '.zip' format from github
wget https://github.com/Papishushi/zsh-ultra/archive/refs/heads/master.zip -P $TEMP

#If unzip command is not installed then setup our own unzip enviromnent
if ! hash unzip 2>/dev/null; then
	#Check for python3
	if ! hash python3 2>/dev/null; then
		#If there is no install of pyenv install it
		echo "Installing python dependencies..."
		if ! hash $HOME/.pyenv/bin/pyenv 2>/dev/null; then
			sh -c "$(wget -O- https://pyenv.run)"
			echo -e "#Autogenerated dependencies for pyenv by ZSH-Ultra Installer
			export PYENV_ROOT="$HOME/.pyenv"
			command -v pyenv >/dev/null || export PATH="\$PYENV_ROOT/bin:\$PATH"
			eval \"\$(pyenv init -)\"
			eval \"\$(pyenv virtualenv-init -)\"" >> ~/.zshrc
		fi
		#Check for python3 updates and install the latest
		$HOME/.pyenv/bin/pyenv update && $HOME/.pyenv/bin/pyenv install 3:latest
		python3 -V
	fi
	#Set unzip environment
	echo "\nSetting unzip tool..."
	unzip(){ python3 -c "from zipfile import ZipFile; ZipFile( '''$1''' ).extractall('$TEMP')"; }
fi

#Unzip source code
echo "\nUnzipping source...\n"
unzip $TEMP/master.zip
#Remove .zip file
rm -rf $TEMP/master.zip

#Check if DOTNET is installed
if ! hash $DOTNET 2>/dev/null; then
	#Get .NET file installer, then install last version of .NET (Currently .NET 6)
	wget https://dot.net/v1/dotnet-install.sh -P $TEMP
	./$TEMP/dotnet-install.sh -c Current

	#Remove .NET file installer
	rm -rf $TEMP/dotnet-install.sh
fi

#Build program from source code and move to destination path
$DOTNET build --configuration Release $TEMP/zsh-ultra-master/
ZSH-ULTRA=$HOME/.zshultra
mkdir -pm a+wrx $ZSH-ULTRA
mv $TEMP/zsh-ultra-master/bin/Release/net6.0/* $ZSH-ULTRA
rm -rf $TEMP

#Display installation path
echo "\nZSH-Ultra Path: ${ZSH-ULTRA}"
#Set PATH
echo $PATH | grep -q "\(^\|:\)$DOTNET_ROOT\(:\|/\{0,1\}$\)" || echo "PATH=$PATH:$DOTNET_ROOT" >> ~/.zshrc; . ~/.zshrc