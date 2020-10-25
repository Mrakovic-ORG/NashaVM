#include "windows.h"
#include "./../../Headers/SectionReader.hpp"
#include "SectionReaderAuxiliary.cpp"


#pragma managed(push,off)

SectionReader::SectionReader() { }
// Sorry me for this poor Section Reader code! :P
msclr::gcroot<BinaryReader^> SectionReader::BeginRead(const char* path, int ID, string Target)
{
	SectionReaderAuxiliary* aux = new SectionReaderAuxiliary();
	IMAGE_FILE_HEADER FileHeader = { 0 };
	LPCSTR fileName = path;
	auto hFile = CreateFileA(fileName, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0);
	auto hFileMapping = CreateFileMapping(hFile, NULL, PAGE_READONLY, 0, 0, NULL);
	auto lpFileBase = MapViewOfFile(hFileMapping, FILE_MAP_READ, 0, 0, 0);
	auto dosHeader = (PIMAGE_DOS_HEADER)lpFileBase;
	auto peHeader = (PIMAGE_NT_HEADERS)((u_char*)dosHeader + dosHeader->e_lfanew);
	auto sectionHeader = IMAGE_FIRST_SECTION(peHeader);
	UINT nSectionCount = peHeader->FileHeader.NumberOfSections;
	for (UINT i = 0; i < nSectionCount; ++i, ++sectionHeader)
	{
		auto nameSec = string(reinterpret_cast<const char*>(sectionHeader->Name));
		auto ByteCount = sectionHeader->Misc.VirtualSize;
		IMAGE_DOS_HEADER DosHeader = { 0 };
		DWORD Signature = 0;
		BYTE* pData = (BYTE*)malloc(ByteCount);
		FILE* fp = fopen(fileName, "rb");
		fseek(fp, 0, SEEK_END);
		fseek(fp, 0, SEEK_SET);
		fread(&DosHeader, 1, sizeof DosHeader, fp);
		fseek(fp, DosHeader.e_lfanew, SEEK_SET);
		fread(&Signature, 1, sizeof(DWORD), fp);
		fread(&FileHeader, 1, sizeof FileHeader, fp);
		fseek(fp,
			dosHeader->e_lfanew + sizeof(IMAGE_NT_HEADERS) +
			(FileHeader.NumberOfSections - 1) * sizeof(IMAGE_SECTION_HEADER),
			SEEK_SET);
		fseek(fp, sectionHeader->PointerToRawData, SEEK_SET);
		fread(pData, 1, ByteCount, fp);

		fclose(fp);

		if (nameSec == Target)
		{
			string strresult = aux->GetHexData(pData, ByteCount);
			auto result = StrToArray::StringToByteArray(strresult);
			CloseHandle(hFile);
			CloseHandle(hFileMapping);
			return sectionHelper(result, ID);
		}
	}
	throw;
}
msclr::gcroot<cli::array<System::Byte>^> SectionReader::BeginReadBytes(const char* path, string Target)
{
	SectionReaderAuxiliary* aux = new SectionReaderAuxiliary();
	IMAGE_FILE_HEADER FileHeader = { 0 };
	LPCSTR fileName = path;
	auto hFile = CreateFileA(fileName, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0);
	auto hFileMapping = CreateFileMapping(hFile, NULL, PAGE_READONLY, 0, 0, NULL);
	auto lpFileBase = MapViewOfFile(hFileMapping, FILE_MAP_READ, 0, 0, 0);
	auto dosHeader = (PIMAGE_DOS_HEADER)lpFileBase;
	auto peHeader = (PIMAGE_NT_HEADERS)((u_char*)dosHeader + dosHeader->e_lfanew);
	auto sectionHeader = IMAGE_FIRST_SECTION(peHeader);
	UINT nSectionCount = peHeader->FileHeader.NumberOfSections;
	for (UINT i = 0; i < nSectionCount; ++i, ++sectionHeader)
	{
		auto nameSec = string(reinterpret_cast<const char*>(sectionHeader->Name));
		auto ByteCount = sectionHeader->Misc.VirtualSize;
		IMAGE_DOS_HEADER DosHeader = { 0 };
		DWORD Signature = 0;
		BYTE* pData = (BYTE*)malloc(ByteCount);
		FILE* fp = fopen(fileName, "rb");
		fseek(fp, 0, SEEK_END);
		fseek(fp, 0, SEEK_SET);
		fread(&DosHeader, 1, sizeof DosHeader, fp);
		fseek(fp, DosHeader.e_lfanew, SEEK_SET);
		fread(&Signature, 1, sizeof(DWORD), fp);
		fread(&FileHeader, 1, sizeof FileHeader, fp);
		fseek(fp,
			dosHeader->e_lfanew + sizeof(IMAGE_NT_HEADERS) +
			(FileHeader.NumberOfSections - 1) * sizeof(IMAGE_SECTION_HEADER),
			SEEK_SET);
		fseek(fp, sectionHeader->PointerToRawData, SEEK_SET);
		fread(pData, 1, ByteCount, fp);

		fclose(fp);

		if (nameSec == Target)
		{
			string strresult = aux->GetHexData(pData, ByteCount);
			auto result = StrToArray::StringToByteArray(strresult);
			CloseHandle(hFile);
			CloseHandle(hFileMapping);
			return result;
		}
	}
	throw;
}
msclr::gcroot<System::String^> SectionReader::BeginReadString(const char* path, int ID, string Target)
{
	SectionReaderAuxiliary* aux = new SectionReaderAuxiliary();
	IMAGE_FILE_HEADER FileHeader = { 0 };
	LPCSTR fileName = path;
	auto hFile = CreateFileA(fileName, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0);
	auto hFileMapping = CreateFileMapping(hFile, NULL, PAGE_READONLY, 0, 0, NULL);
	auto lpFileBase = MapViewOfFile(hFileMapping, FILE_MAP_READ, 0, 0, 0);
	auto dosHeader = (PIMAGE_DOS_HEADER)lpFileBase;
	auto peHeader = (PIMAGE_NT_HEADERS)((u_char*)dosHeader + dosHeader->e_lfanew);
	auto sectionHeader = IMAGE_FIRST_SECTION(peHeader);
	UINT nSectionCount = peHeader->FileHeader.NumberOfSections;
	for (UINT i = 0; i < nSectionCount; ++i, ++sectionHeader)
	{
		auto nameSec = string(reinterpret_cast<const char*>(sectionHeader->Name));
		auto ByteCount = sectionHeader->Misc.VirtualSize;
		IMAGE_DOS_HEADER DosHeader = { 0 };
		DWORD Signature = 0;
		BYTE* pData = (BYTE*)malloc(ByteCount);
		FILE* fp = fopen(fileName, "rb");
		fseek(fp, 0, SEEK_END);
		fseek(fp, 0, SEEK_SET);
		fread(&DosHeader, 1, sizeof DosHeader, fp);
		fseek(fp, DosHeader.e_lfanew, SEEK_SET);
		fread(&Signature, 1, sizeof(DWORD), fp);
		fread(&FileHeader, 1, sizeof FileHeader, fp);
		fseek(fp,
			dosHeader->e_lfanew + sizeof(IMAGE_NT_HEADERS) +
			(FileHeader.NumberOfSections - 1) * sizeof(IMAGE_SECTION_HEADER),
			SEEK_SET);
		fseek(fp, sectionHeader->PointerToRawData, SEEK_SET);
		fread(pData, 1, ByteCount, fp);

		fclose(fp);

		if (nameSec == Target)
		{
			string strresult = aux->GetHexData(pData, ByteCount);
			auto result = StrToArray::StringToString(strresult);
			CloseHandle(hFile);
			CloseHandle(hFileMapping);
			return result;
		}
	}
	throw;
}
#pragma managed(pop)
