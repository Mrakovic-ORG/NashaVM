#include "./../Headers/NStringReader.hpp"
#include <msclr\marshal_cppstd.h>
#pragma managed(push,off)

/// <summary>
/// a pseudo BinaryReader made for the OpcodeDiscover class
/// </summary>
NStringReader::NStringReader(std::string subject)
{
	// Default Static Initial Value
	_buffer = 0;
	_subject = subject;
}

/// <summary>
/// Reads one byte from its subject
/// </summary>
int NStringReader::ReadByte()
{
	// reads byte parts example: 4 + D = 0x4D = 77
	auto value1 = *(_subject.begin() + _buffer++);
	auto value2 = *(_subject.begin() + _buffer++);

	// combines both bytes and returns them
	return combine(value1, value2);

}

/// <summary>
/// Reads multiple bytes from its subject
/// </summary>
std::vector<int>* NStringReader::ReadBytes(int count)
{
	std::vector<int>* bytes = new std::vector<int>();
	do
	{
		// Read one byte
		auto b = ReadByte();
		// Put the returning value for the vector
		bytes->push_back(b);
		--count;
	} while (count > 0);
	return bytes;
}

/// <summary>
/// Reads a intenger from its subject
/// </summary>
int NStringReader::ReadInt32()
{
	// Read 4 bytes
	auto buffer = ReadBytes(4);
	// Bitwise operators to get integer value
	return buffer->at(0) | buffer->at(1) << 8 | buffer->at(2) << 16 | buffer->at(3) << 24;

}

int NStringReader::GetBuffer()
{
	return _buffer;
}
#pragma managed(pop)