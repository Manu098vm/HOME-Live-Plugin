# About

This Plugin is meant to allow people to dump and view infos from those Pokémons that are stuck in HOME beyond the first Box, or that for some reason the owner does not want to transfer away (eg. Unique GO/HOME Symbol sticker or SWSH unavailable mons). This Plugin is not intended to allow users to edit Pokémon Infos directly from HOME and it will never allow to.

This Plugin is not developed by the PKHeX Development Projects server, so do NOT report problems or request support there. Use the [Project Pokémon thread](https://projectpokemon.org/home/forums/topic/58311-pkhex-home-live-plugin-ram-viewer-thread/) instead.

The creators of this tool are not responsible for any adverse outcomes or side effects of using this tool.

We do not condone use of cheating and modified data to take advantage of others. Trading converted files without disclosure could be considered scam.

# Disclosure about the Dumper feature:

The Plugin creates a 1:1 dump for the encrypted Pokémon HOME data, in the EH1 format. If requested by the user, PKHeX can decrypt that dump in the PH1 format. Please be aware that these format structure might change in the future.

# Disclosure about the Viewer feature:

PKHeX simulates a conversion from the Pokémon HOME data format (PH1) to standard PKM file formats based on the current loaded save file.

This process is unofficial and there is always the chance that it does not accurately replicate an official transfer.

If you proceed with this tool, you accept the following:

* The PKM files from the conversion are NOT legitimate in any way, even if the original encounter was.

* The resulting files from the conversion may not even be legal in some circumstances.

* When using 'Convert any PKM data if compatible with save file', it is likely that the resulting Pokémon will be illegal.

* Do NOT use converted PKM in online battles/trades.

* Do NOT use converted files to report legality issues, whether in the Project Pokémon forums/Discord or in the PKHeX Development Projects Discord.

* The Viewer is intended for research, learning, and entertainment purposes.

# Prerequisites

- Hacked Switch with [Atmosphère](https://github.com/Atmosphere-NX/Atmosphere) installed.

- [sys-botbase](https://github.com/olliz0r/sys-botbase/releases) sysmodule running on the system.

- Online Internet connection.

- HOME Subscription is not needed.

# How to use 

Download the DLL from [Project Pokémon](https://projectpokemon.org/home/files/file/4388-pkhex-plugin-home-live-viewer/).

For Windows users, right click on the DLL, go to properties and select "Unlock".

Move the DLL to your PKHeX/Plugins folder, if you don't have it, just create it.

Open PKHeX with a Sword/Shield Save File, or a Blank SWSH save, and click on Tools->HOME to open the Plugin.

Open the Pokémon HOME application from your Nintendo Switch and tap the screen and least once. 

Enter your IP Adrress, select the Boxes you want to show and click "Connect".

For any problems or suggestion, feel free to write an issue on the GitHub or on the [Project Pokémon thread](https://projectpokemon.org/home/forums/topic/58311-pkhex-home-live-plugin-ram-viewer-thread/).

# Credits

[olliz0r](https://github.com/olliz0r) and [berichan](https://github.com/berichan) for [sys-botbase](https://github.com/olliz0r/sys-botbase)

[fishguy6564](https://github.com/fishguy6564) and [Koi-3088](https://github.com/Koi-3088) for [usb-botbase](https://github.com/fishguy6564/USB-Botbase)

[SciresM](https://github.com/SciresM) for his researches into the new Pokémon Home encryption data method, implemented in his [PKHeX fork](https://github.com/SciresM/PKHeX/blob/no_way_home/PKHeX.Core/PKM/Util/HomeCrypto.cs).

[kwsch](https://github.com/kwsch) for [PKHeX](https://github.com/kwsch/PKHeX)

[architdate](https://github.com/architdate), [Lusamine](https://github.com/Lusamine), all the ALM team and all the people involved in the developing/testing of the [PKHeX-Plugins](https://github.com/architdate/PKHeX-Plugins), thanks to which this project would never exist

[PP-theSLAYER](https://github.com/PP-theSLAYER) for the mutual support on the HOME research
