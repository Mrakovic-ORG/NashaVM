#pragma once
#include <string>
#include <msclr\marshal_cppstd.h>

using namespace std;
using namespace msclr::interop;
using namespace System::IO;
public class SectionReader
{
public:
	SectionReader();/*
private:
	std::string _sectionName;
	const char* _path;
	int _id;*/
public:
	msclr::gcroot<BinaryReader^> BeginRead(const char* path, int ID, string Target);
};