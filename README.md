
# NashaVM - Native Sharp Virtual Machine

Nasha is a Virtual Machine for .NET files and its runtime was made in [C++/CLI](https://en.wikipedia.org/wiki/C%2B%2B/CLI)

### Installation
```bash
git clone https://github.com/Mrakovic-ORG/NashaVM --recurse
cd NashaVM\NashaVM
nuget restore
msbuild
```

### Dependencies
- [dnlib](https://github.com/0xd4d/dnlib)
- [.NET Framework 4.0](https://www.microsoft.com/pt-br/download/details.aspx?id=17851)
- [Visual C++ Redistrutable](https://www.microsoft.com/en-us/download/details.aspx?id=48145)
- **I recommend you to use Visual Studio to build!**

### Opcodes Status
<details>
  <summary>Click to open !</summary>
  
  ## Status Description
|Status|Description|
|--|----|
|AD|Added|
|NA|Not Added|
|NC|Not complete (upon work)|
  
 ## Opcodes
|Opcode| Description | Status |
|--|----|--|
|Ret| Return from method, possibly with a value. |AD|
|Ldstr| Push a string object for the literal string. |AD|
|Ldc_X|  |NA|

TODO: Add more opcodes
</details>


### Known issues
- Incompatible with Linux based OS


## FAQ

- **What is this project for?**

This project is made to protect and hide managed opcodes inside a mixed engine to make it harder for reverse engineers to view or tamper your application. 
- **Will this project be maintened?**

We do not plan any update yet, It has been published so with the help from the community we can make the project grow and improve.
- **Is Nasha a code obfuscator?**

No, Nasha is a Instruction Virtual Machine which means it could be interpreted as a obfuscator while it is not. Your code will be protected but not in a similar way.
- **How can i contribute?**

	- If you have an issue you can open a ticket and we will investigate the issue in question.
	- If you have no programming knowledge but you are willing to support you can donate at **bc1qfedg6qty0l8hk8qu9d4akj86mh7yqfwzcjnvn7** (Segwit BTC)
	- If you are willing to make your own project out of this repo, [Follow the setup and installation guide](#Installation) and make sure to credit our work or you will be facing **Mrak The Murderer** all jokes apart since we are licensed by the [GNU](https://github.com/Mrakovic-ORG/NashaVM/blob/master/LICENSE) license it is strictly prohibed to personally this project without disclosing the source.
	- You can also join in our [discord server](https://discord.gg/JhCWDF4)

## Contributors & Greetings

* [Мрак](https://github.com/MrakDev)
* [0x11DFE](https://github.com/0x11DFE) aka Tesla
*  All members from the [Mrakovic Organization](https://github.com/Mrakovic-ORG), [Wannabe1337](https://wannabe1337.xyz), [NetShields](https://discord.gg/Pqf2A9d)


## Credits

* [0xd4d](https://github.com/0xd4d) for the awesome work he brings to the .NET community.
* MemeVM Creator (Sorry can't find original repo).
