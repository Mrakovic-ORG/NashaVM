#include "HandlerLinker.hpp"
#include "Ret.cpp"
#include "LdcI4.cpp"
#include "Ldstr.cpp"
#include "Call.cpp"
#include "Nop.cpp"
#include "Pop.cpp"

#pragma managed(push,off)
/// <summary>
/// A class to link handlers to pointeirs.
/// </summary>
HandlerLinker::HandlerLinker()
{
	OpcodesPointers[0] = &Ret::Run;	
	OpcodesPointers[1] = &LdcI4::Run;
	OpcodesPointers[2] = &Ldstr::Run;
	OpcodesPointers[3] = &Call::Run;
	OpcodesPointers[4] = &Nop::Run;
	OpcodesPointers[5] = &Pop::Run;

	DeserializationPointers[0] = &Ret::Constructor;
	DeserializationPointers[1] = &LdcI4::Constructor;
	DeserializationPointers[2] = &Ldstr::Constructor;
	DeserializationPointers[3] = &Call::Constructor;
	DeserializationPointers[4] = &Nop::Constructor;
	DeserializationPointers[5] = &Pop::Constructor;
}
#pragma managed(pop)