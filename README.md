# FDFEditor
WIP gui editor and decryptor for the data files of the Touhou fangame **東方幕華祭 春雪篇 ～ FANTASTIC DANMAKU FESTIVAL PART II**. Partial support for the first game.
Written in .NET Core using the WPF gui framework. GPL-3 license.

## Features
1. Open and edit all .xna files found in the Content/Data directory. This includes stage scripts, pattern scripts, dialogues and a bunch of other stuff.
2. Decrypt or Encrypt files in bulk.
3. Handy notepad for all your copy pasting needs.
4. General documentation of the scripting "languages" used for the game's content. Alternatively visit [the file in your browser](https://github.com/Kraballa/FDFEditor/blob/master/FDFEditor/Resources/Modding%20Guide.txt).

## Installation Windows
1. buy the game on Steam [here](https://store.steampowered.com/app/1031480/___Fantastic_Danmaku_Festival_Part_II/)
2. download and install the .NET Core Desktop Runtime [here](https://dotnet.microsoft.com/download/dotnet-core/3.1/runtime/?utm_source=getdotnetcore&utm_medium=referral)
3. download the lastest version of FDFEditor [here](https://github.com/Kraballa/FDFEditor/releases)
4. extract and run the exe

## Installation Linux
.Net Core runs natively on linux but the WPF framework doesn't. It might still be possible to run the program through Wine. Simply follow the steps in the Windows installation but run the .NET Core Desktop Runtime Installer through Wine and launch the program the same way.

## Building from source
1. Install Visual Studio Community 2019, or any other version of the 2019 line.
2. Install the WPF and .NET Core SDK's from the Visual Studio Installer
3. Clone the project and build.
It should be plug and play since I haven't used any additional libraries or frameworks.

## Info
Please make sure to read the About page found under the Help menu after first opening the app. For some general guidelines and tips consult the Modding Guide, also found under the Help menu.

THIS TOOL WAS MADE WITH GOOD INTENTIONS. DO NOT USE IT FOR MALICIOUS PURPOSES. I WILL TAKE NO RESPONSIBILITY FOR ANY LEGAL ACTIONS BROUGHT AGAINST YOU.
