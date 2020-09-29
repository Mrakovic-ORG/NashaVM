#include "./../Headers/HandlerLinker.hpp"
#include "./Handlers/Ret.cpp"
#include "./Handlers/LdcI4.cpp"
#include "./Handlers/Ldstr.cpp"
#include "./Handlers/Call.cpp"
#include "./Handlers/Nop.cpp"
#include "./Handlers/Pop.cpp"
#include "./Handlers/Ldsfld.cpp"
#include "./Handlers/Stsfld.cpp"
#include "./Handlers/Brfalse.cpp"
#include "./Handlers/Brtrue.cpp"
#include "./Handlers/Br.cpp"
#include "./Handlers/Ldloc.cpp"
#include "./Handlers/Stloc.cpp"

#pragma managed(push,off)
/// <summary>
/// A class to link handlers to their pointers.
/// </summary>
HandlerLinker::HandlerLinker()
{
	OpcodesPointers[0] = &Ret::Run;	
	OpcodesPointers[1] = &LdcI4::Run;
	OpcodesPointers[2] = &Ldstr::Run;
	OpcodesPointers[3] = &Call::Run;
	OpcodesPointers[4] = &Nop::Run;
	OpcodesPointers[5] = &Pop::Run;
	OpcodesPointers[6] = &Ldsfld::Run;
	OpcodesPointers[7] = &Stsfld::Run;
	OpcodesPointers[8] = &Brfalse::Run;
	OpcodesPointers[9] = &Brtrue::Run;
	OpcodesPointers[10] = &Br::Run;
	OpcodesPointers[11] = &Ldloc::Run;
	OpcodesPointers[12] = &Stloc::Run;

	DeserializationPointers[0] = &Ret::Constructor;
	DeserializationPointers[1] = &LdcI4::Constructor;
	DeserializationPointers[2] = &Ldstr::Constructor;
	DeserializationPointers[3] = &Call::Constructor;
	DeserializationPointers[4] = &Nop::Constructor;
	DeserializationPointers[5] = &Pop::Constructor;
	DeserializationPointers[6] = &Ldsfld::Constructor;
	DeserializationPointers[7] = &Stsfld::Constructor;
	DeserializationPointers[8] = &Brfalse::Constructor;
	DeserializationPointers[9] = &Brtrue::Constructor;
	DeserializationPointers[10] = &Br::Constructor;
	DeserializationPointers[11] = &Ldloc::Constructor;
	DeserializationPointers[12] = &Stloc::Constructor;
}
#pragma managed(pop)