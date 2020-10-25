#pragma once
#include <string>
#include <msclr\marshal_cppstd.h>

using namespace std;
using namespace msclr::interop;
using namespace System::IO;
public class SectionReader
{
public:
	SectionReader();
public:
	msclr::gcroot<BinaryReader^> BeginRead(const char* path, int ID, string Target);
	msclr::gcroot<cli::array<System::Byte>^> BeginReadBytes(const char* path, string Target);
	msclr::gcroot<System::String^> BeginReadString(const char* path, int ID, string Target);
};