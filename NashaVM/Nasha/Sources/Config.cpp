#include <msclr\marshal_cppstd.h>
#include <string>
#include "./../Headers/SectionReader.hpp"
#include "./../Headers/Globals.hpp"
#include "./../Headers/Config.hpp"

Config::Config()
{
	// Instance new GSC
	glob = new GSC();
}

/// <summary>
/// Stores the references coming from .Nasha1 section
/// </summary>
void Config::SetupReferences()
{
	msclr::interop::marshal_context ctx;
	SectionReader* ReferenciesReader = new SectionReader();

	auto Bin = ReferenciesReader->BeginRead(ctx.marshal_as<std::string>(
		System::Reflection::Assembly::GetCallingAssembly()->ManifestModule->Assembly->Location).c_str(), -1, ".Nasha1");
	int ReferenciesCount = Bin->ReadInt32();
	for (int i = 0; i < ReferenciesCount; i++)
	{
		System::Int32 len = Bin->ReadInt32();
		cli::array<System::Byte>^ strB = Bin->ReadBytes(len);
		glob->References->New(msclr::interop::marshal_as<std::string>(System::Text::Encoding::UTF8->GetString(strB)));
	}
}

void Config::SetupDiscover()
{
	msclr::interop::marshal_context ctx;
	SectionReader* Reader = new SectionReader();

	auto DiscoverReader = Reader->BeginReadString(ctx.marshal_as<std::string>(
		System::Reflection::Assembly::GetCallingAssembly()->ManifestModule->Assembly->Location).c_str(), -1, ".Nasha2");

	auto discoverstr = (String^)DiscoverReader;
	auto stdStr = ctx.marshal_as<std::string>(discoverstr);

	OpcodeDiscover* discover = new OpcodeDiscover(stdStr);

	// Instantiates the handler linking class.
	glob->Handlers = new HandlerLinker(discover);
}

void Config::SetupBody()
{
	msclr::interop::marshal_context ctx;
	SectionReader* Reader = new SectionReader();

	auto Bytes = Reader->BeginReadBytes(ctx.marshal_as<std::string>(
		System::Reflection::Assembly::GetCallingAssembly()->ManifestModule->Assembly->Location).c_str(), ".Nasha0");
	glob->VMBytes = new VMBytes(Bytes);
}