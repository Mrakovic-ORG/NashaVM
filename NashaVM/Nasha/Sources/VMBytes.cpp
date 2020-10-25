#include "./../Headers/VMBytes.hpp"
#include "SectionReader/SectionReaderAuxiliary.cpp"

VMBytes::VMBytes(gcroot<cli::array<System::Byte>^> bytes)
{
	_bytes = bytes;
}
gcroot<System::IO::BinaryReader^> VMBytes::Get(int index)
{
	return sectionHelper(_bytes, index);
}