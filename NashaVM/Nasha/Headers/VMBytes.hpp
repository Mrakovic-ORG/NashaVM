#pragma once
#include <msclr\marshal_cppstd.h>
public class VMBytes
{
public:
	VMBytes(gcroot<cli::array<System::Byte>^> bytes);
	gcroot<System::IO::BinaryReader^> Get(int index);
private:
	gcroot<cli::array<System::Byte>^> _bytes;
};