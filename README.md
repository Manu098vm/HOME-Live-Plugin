# About  
This program is meant to dump and view information from Pokémon deposited in the Pokémon HOME application.  
**It is not intended to edit or inject Pokémon data directly into HOME and will never support such features. Do not request any injection capabilities.**  

Powered by [PKHeX.Core](https://github.com/kwsch/PKHeX) and [SysBot.Base](https://github.com/kwsch/SysBot.NET).  

## Features:  
* Handle (dump, load, view) PKH/EKH/PH1/EH1/PH2/EH2/PH3/EH3 (Pokémon HOME format) files.  
* Generate PB7/PK8/PB8/PA8/PK9 (Pokémon game-specific format) files from Pokémon HOME data/files.  

## Use Case Examples:  
* Dump and preserve all your Pokémon HOME PKM files, or load them into PKHeX.  
* Access Pokémon stored beyond the first Pokémon HOME box if your HOME subscription has expired.  
* Generate a cloned Pokémon for use in any core series game while keeping the original data intact in your Pokémon HOME account (preserving unique GO, HOME, or Bank stamps).  

For support, please post in the [Project Pokémon thread](https://projectpokemon.org/home/forums/topic/58311-pkhex-home-live-plugin-ram-viewer-thread/).  
Alternatively, feel free to join my [Discord server](https://discord.gg/yWveAjKbKt).


[<img src="https://canary.discordapp.com/api/guilds/693083823197519873/widget.png?style=banner2">](https://discord.gg/yWveAjKbKt)

## Notes
The creators of this tool are not responsible for any adverse outcomes or side effects of using this tool.

We do not condone use of cheating and modified data to take advantage of others. Trading converted files without disclosure could be considered scam.

# Disclosure about the Dumper feature:

The Plugin creates a 1:1 dump for the encrypted Pokémon HOME data. If requested by the user, PKHeX can decrypt the dumped files. Please be aware that the Pokémon HOME data structure might change in the future.

# Disclosure about the Viewer feature:

PKHeX simulates a conversion from the Pokémon HOME data format to a standard PKM file based on the current game mode loaded.

This process is unofficial and there is always the chance that it does not accurately replicate an official transfer.

If you proceed with this tool, you accept the following:

* The PKM files from the conversion can not be considered legitimate, even if the original encounter was. The files will be legal at best.

* Do NOT use the obtained files to report legality issues with PKHeX. Join the [Manu's Discord](https://discord.gg/yWveAjKbKt) server for support.

# Prerequisites

- Hacked Switch with [Atmosphère](https://github.com/Atmosphere-NX/Atmosphere) installed.

- [sys-botbase](https://github.com/olliz0r/sys-botbase/releases) or [usb-botbase](https://github.com/Koi-3088/usb-botbase) running on the system.

- Online Internet connection.

- HOME Subscription is not needed.

The Home Live Plugins can not be used in EmuNand or emulators.

# How to Use  

Download the DLL from [Project Pokémon](https://projectpokemon.org/home/files/file/4388-pkhex-plugin-home-live-viewer/) or from the [GitHub Release page](https://github.com/Manu098vm/HOME-Live-Plugin/releases).  

For Windows users, right-click on the DLL, go to Properties, and select `Unblock`.  

Move the DLL to your PKHeX `plugins` folder. If the folder does not exist, create it in the same directory as the PKHeX executable.  

Open PKHeX in Let's Go/Sword/Shield/Brilliant Diamond/Shining Pearl/Legends Arceus/Scarlet/Violet mode with a blank/default save file, then click on `Tools -> Home Live Dumper` or `Tools -> Home Live Viewer` to open the plugin.  

Open the Pokémon HOME application on your Nintendo Switch and tap the screen at least once. The app must be successfully connected to the internet and linked to your Pokémon HOME account before proceeding.  

Enter your IP address into the plugin, select the boxes/slots you want to display or dump, and click "Connect".  

# Credits  

[architdate](https://github.com/architdate) for contributions in the initial stage of this plugin.  

[olliz0r](https://github.com/olliz0r) and [berichan](https://github.com/berichan) for [sys-botbase](https://github.com/olliz0r/sys-botbase).  

[fishguy6564](https://github.com/fishguy6564) and [Koi-3088](https://github.com/Koi-3088) for [usb-botbase](https://github.com/fishguy6564/USB-Botbase).  

[SciresM](https://github.com/SciresM) for his research into the Pokémon HOME encryption data method, implemented in his [PKHeX fork](https://github.com/SciresM/PKHeX/blob/5bf28522c34bca09e24d4ed83cf24358ed86a8d7/PKHeX.Core/PKM/Util/HomeCrypto.cs#L1).  

[kwsch](https://github.com/kwsch) for [PKHeX](https://github.com/kwsch/PKHeX) and [SysBot.NET](https://github.com/kwsch).  

[architdate](https://github.com/architdate), [Lusamine](https://github.com/Lusamine), the entire ALM team, and everyone involved in the development and testing of [PKHeX-Plugins](https://github.com/architdate/PKHeX-Plugins), without whom this project would not exist.  

[PP-theSLAYER](https://github.com/PP-theSLAYER) for mutual support in Pokémon HOME research.  

# License

![gplv3-with-text-136x68](https://user-images.githubusercontent.com/52102823/199572700-4e02ed70-74ef-4d67-991e-3168d93aac0d.png)

Copyright © 2025 Manu098vm

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
