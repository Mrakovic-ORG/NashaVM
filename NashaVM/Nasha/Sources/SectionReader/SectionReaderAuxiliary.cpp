#include <string>
#include <msclr/gcroot.h>
#include "windows.h"
#include <memory>
#include "StringToByteArray.cpp"

using namespace std;
using namespace System::IO;
using namespace System::Collections::Generic;
using namespace System::Collections;

static auto Decompress(cli::array<System::Byte>^ arr)
{
	MemoryStream^ ms = gcnew MemoryStream(arr);
	MemoryStream^ nMS = gcnew MemoryStream();

	auto def = gcnew System::IO::Compression::DeflateStream(ms, System::IO::Compression::CompressionMode::Decompress);
	def->CopyTo(nMS);

	auto a = nMS->ToArray();
	return a;
}
static msclr::gcroot<BinaryReader^> sectionHelper(msclr::gcroot<cli::array<System::Byte>^> arr, int ID)
{
	auto cuu = Decompress(arr);
	auto aax = System::Linq::Enumerable::ToList(System::Linq::Enumerable::OfType<System::Byte>(cuu));
	if (ID != -1)
		aax->RemoveRange(0, ID);
	auto err = gcnew BinaryReader(gcnew MemoryStream(System::Linq::Enumerable::ToArray(aax)));
	return err;
}

#pragma managed(push,off)

private class SectionReaderAuxiliary
{
public:

	static string GetHexData(BYTE* ptr, DWORD len)
	{
		string listHEX = "";

		size_t index = 0;
		size_t i = 0;
		const size_t width = 16;
		while (index + width < len)
		{
			int i;
			for (i = 0; i < width; ++i)
			{
				string axe = string_format("%02X", ptr[index + i]);
				listHEX += axe;
			}
			index += width;
		}

		for (i = 0; index + i < len; ++i)
		{
			string axe = string_format("%02X", ptr[index + i]);
			listHEX += (axe);
		}
		return listHEX;
	}
	template<typename ... Args>
	static std::string string_format(const std::string& format, Args ... args)
	{
		size_t size = snprintf(nullptr, 0, format.c_str(), args ...) + 1; // Extra space for '\0'
		if (size <= 0) throw;

		std::unique_ptr<char[]> buf(new char[size]);
		snprintf(buf.get(), size, format.c_str(), args ...);

		return std::string(buf.get(), buf.get() + size - 1); // We don't want the '\0' inside
	}

};
#pragma managed(pop)