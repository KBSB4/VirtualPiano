# Piano Hero
**Virtual Piano Educational Game**</br></br>
![Platform](https://img.shields.io/badge/platform-windows-lightgrey)
![GitHub repo size](https://img.shields.io/github/repo-size/KBSB4/VirtualPiano)
![GitHub last commit](https://img.shields.io/github/last-commit/KBSB4/VirtualPiano)
![Maintenance](https://img.shields.io/maintenance/yes/2023)

This (student) project has been commisioned by Windesheim

## Building
Build a release executable with the following command: `dotnet publish -r win-x64 /p:PublishSingleFile=true /p:UseAppHost=true /p:IncludeNativeLibrariesForSelfExtract=true --output "C:\tmp\pianohero"`

After that these files are required to run:
* Wpfview.exe
* SoundPlayer folder 
* DEBUGmidi folder
