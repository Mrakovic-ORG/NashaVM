#include "HandlerLinker.hpp"
#include "LdcI4.cpp"
#include "Ret.cpp"
#pragma managed(push,off)
/// <summary>
/// A class to link handlers to pointeirs.
/// </summary>
HandlerLinker::HandlerLinker()
{
	OpcodesPointers[0] = &Ret::Run;	
	OpcodesPointers[1] = &LdcI4::Run;

	DeserializationPointers[0] = &Ret::Constructor;
	DeserializationPointers[1] = &LdcI4::Constructor;
}
#pragma managed(pop)