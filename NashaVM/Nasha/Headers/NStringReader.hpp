#pragma once
#include <string>
#include <sstream>
#include <vector>

public class NStringReader
{
public:
	NStringReader(std::string subject);
	int ReadByte();
	std::vector<int>* ReadBytes(int count);
	int ReadInt32();
	int GetBuffer();
private:
	int _buffer;
	std::string _subject;
	int combine(char a, char b) 
	{
		unsigned int result;
		std::stringstream ss;
		ss << std::hex << a << b;
		ss >> result;
		return static_cast<int>(result);
	}
};