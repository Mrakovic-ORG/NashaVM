<!-- 
Ovo je naša vm, dobrodošli. - Tesla.
-->

# [NashaVM - Native Sharp Virtual Machine](https://github.com/Mrakovic-ORG/NashaVM)

Nasha is a Virtual Machine for .NET files and its runtime was made in [C++/CLI](https://en.wikipedia.org/wiki/C++/CLI)

### Installation
```bash
git clone https://github.com/Mrakovic-ORG/NashaVM --recurse
cd NashaVM\NashaVM
nuget restore
msbuild
```

### Dependencies
- [dnlib](https://github.com/0xd4d/dnlib)
- [.NET Framework](https://dotnet.microsoft.com/download/dotnet-framework)
- [Visual C++ Redistrutable](https://support.microsoft.com/help/2977003/the-latest-supported-visual-c-downloads)


## FAQ

- **What is this project for?**

This project is made to protect and hide managed opcodes inside a mixed engine to make it harder for reverse engineers to view or tamper your application. 

- **Will this project be maintened?**

Yes as long as the project is appreciated or until there is a major unfixable issue.

- **What opcodes are supported?**

All the supported opcodes are listed on [Supported Opcodes Wiki](https://github.com/Mrakovic-ORG/NashaVM/wiki/Supported-Opcodes).

- **Is Nasha a code obfuscator?**

No, Nasha is a Instruction Virtual Machine which means it could be interpreted as a obfuscator while it is not. Your **code** will remain hidden has a IL stack within the VMIL.

- **How can i contribute?**

	- If you facing any problem have a look at [Issues](https://github.com/Mrakovic-ORG/NashaVM/issues) before opening a [New Issue](https://github.com/Mrakovic-ORG/NashaVM/issues/new/choose).
	- If you have no programming knowledge but you are willing to support you can donate at **bc1qfedg6qty0l8hk8qu9d4akj86mh7yqfwzcjnvn7** (Segwit BTC)
	- If you are willing to make your own project out of this repo, [Follow the setup and installation guide](#Installation) and make sure to credit our work or you will be facing **Mrak The Murderer** all jokes beside, since we are licensed by the [GNU](https://github.com/Mrakovic-ORG/NashaVM/blob/master/LICENSE) license it is strictly prohibed to personally this project without disclosing the source.

## Contributors & Greetings

* [Мрак](https://github.com/MrakDev)
* [0x11DFE](https://github.com/0x11DFE) aka Tesla
* Assumer
*  All members from the [Mrakovic Organization](https://github.com/Mrakovic-ORG), [Wannabe1337](https://wannabe1337.xyz), [NetShields](https://discord.gg/Pqf2A9d)


## Credits

* [0xd4d](https://github.com/0xd4d) for the awesome work he brings to the .NET community.
* xsilent007 for [MemeVM](https://github.com/TobitoFatitoRE/MemeVM)

<!--
Our story just begun, tchau. - Mrak the Murderer
-->