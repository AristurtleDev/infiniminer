# Infiniminer Monogame 3.8.1.303 Port
This repository is a port of the original Infiniminer source code forked from https://github.com/gibbed/Infiniminer.  This port brings the Infiniminer source from XNA 3.1 to MonoGame 3.8.1.303.  

The goal of this port was to port the original source code which targeted .NET Framework and bring it up to target .NET 6 and make use of the MonoGame Cross Platform Desktop Project (mgdesktopgl) templates so that the game could be run on Windows, Mac, and Linux.

Another goal of this port was for educational purposes.  I wanted to take a complex game originally written in XNA 3.1 and update it to the current MonoGame version.  For all port related changes, see any commits that are titled `Port from XNA to MonoGame`.  These commits specifically have the changes made to bring it from XNA 3.1 to MonoGame.

## Branches
This repository contains two branches as defined below

| Branch | Description |
|---|---|
| `main` | This branch contains all updated work done to port Infiniminer to MonoGame 3.8.1.303 |
| `original` | This branch contains the original source code forked. |

## Current Port Status
At this time, the original source code has been ported to MonoGame 3.8.1.303 and will run without crashing, but the following bugs need to be fixed before the game is fully playable.
- ❌ Bug: Bloom Effect doesn't work properly. If enabled, the BlockEngine render is just blank.  For now this is disabled globally by commenting out the part of the code that checks and applies the bloom shader in BlockEngine.cs
- ❌ Bug: Windows OS specific DLLs are used throughout the codebase, and at this time those specific function calls have been disabled.  A workaround will need to be implemented to make it cross platform.
- ❌ Bug: `System.Windows.Forms` is used in several sections in the code base.  This will need to be adjusted to be cross platform once a good general solution is found

## Building
- Clone the source: `https://github.com/AristurtleDev/Infiniminer.git`
- Open the Solution `/source/Infiniminer.MonoGame.sln`
- Build `Infiniminer.Client` and `Infiniminer.Server`

To start a local server, run the `Infiniminer.Server` project.  
To start a new client, run the `Infiniminer.Client` project.

## LICENSE
This repository consists of several parts, each of which have their own license or distribution information.  Each of these are listed below.  If you are the original author or license holder for any of these and you wish them to be removed from this repository, please contact me.

- The main repository is licensed under the MIT License.  You can find the license text in the [LICENSE](./LICENSE) file.
- Infiniminer is licensed under the MIT License.  You can find the license text for Infiniminer in the [Infiniminer License](./source/Infiniminer/LICENSE) file.
- The font `04B_03B_.TTF` is distributed as freeware according to the original freeware file included.  You can find this information in the [04B_03B_.TTF About](./source/Infiniminer/Infiniminer.Client/Content/04b_03b/about.gif) file.
- The font `04B_08__.TTF` is distributed as freeware according to the original freeware file included.  You can find this information in the [04B_08__.TTF About](./source/Infiniminer/Infiniminer.Client/Content/04b_08/about.gif) file.
- Artwork by Chris Gengler (tayrtahn@gmail.com)
- Music "Can't Hide Your Love" by Kola Kid that was part of the original source code was acquired from [http://www.8bitcollective.com/music/Kola+Kid/can%27t+hide+your+love/](https://web.archive.org/web/20090210071601/http://www.8bitcollective.com/music/Kola+Kid/can't+hide+your+love/), which at the time was distributed under the [Creative Commons BY-NC-SA License](https://creativecommons.org/licenses/by-nc-sa/2.5/)


## Acknowledgements
First, I would like to thank Zach Barth and everyone at Zachtronics Industries for making this source code open under the MIT license.  From research, the circumstances which made them publish the source under the MIT was unfortunate, but thank you regardless for it.

Next, I would like to thank members of the MonoGame Community Discord.  As I worked through this port, there were several times where I was scratching my head, and they were increadibly helpful either though answering questions or by just showing interest and support in general.  Specifically I'd like to thank BlueRaven, NKast, MrGrak, Lupin, and Crippy-D.

Finally, I would thank the MonoGame team for creating and maintaining MonoGame in the first place.  This wouldn't have been possible without the work that has been put in by the maintainers and community contributions over the years.  