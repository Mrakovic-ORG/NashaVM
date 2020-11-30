#include "./../Headers/VMBody.hpp"
#include "./../Headers/Config.hpp"

#pragma managed(push,off)
/// <summary>
///  Performs an indirect call(calli) to one of the handlers(Deserialization) by its id.
/// </summary>
/// <param name="ptr">VMBody's address pointer</param>
/// <param name="handlerID">Handler identification</param>
/// <param name="glob">Global Static Class instance</param>
/// <returns>Deserialized Operand</returns>
static msclr::gcroot<Object^> NativeProxy(void* ptr, int handlerID, GSC* glob)
{
	typedef msclr::gcroot<Object^>(*fastCast)(msclr::gcroot<VMBody^>&);

	msclr::gcroot<VMBody^>& obj = *((msclr::gcroot<VMBody^>*)ptr);
	fastCast castOpcode = reinterpret_cast<fastCast>(reinterpret_cast<LONGLONG>(glob->Handlers->DeserializationPointers[handlerID]));
	return (*(castOpcode))(obj);
}

/// <summary>
///  Performs an indirect call(calli) to one of the handlers(Deserialization) by its id (without returning value).
/// </summary>
/// <param name="ptr">VMBody's address pointer</param>
/// <param name="operand">Operand's address pointer</param>
/// <param name="handlerID">Handler identification</param>
/// <param name="glob">Global Static Class instance</param>
static void NativeProxy2(void* ptr, void* operand, int handlerId, GSC* glob)
{
	typedef void (*fastCast)(msclr::gcroot<VMBody^>&, msclr::gcroot<Object^>);

	msclr::gcroot<VMBody^>& obj = *((msclr::gcroot<VMBody^>*)ptr);
	msclr::gcroot<Object^>& Operand = *((msclr::gcroot<Object^>*)operand);

	fastCast castOpcode = reinterpret_cast<fastCast>(reinterpret_cast<LONGLONG>(glob->Handlers->OpcodesPointers[handlerId]));
	(*(castOpcode))(obj, Operand);
}
#pragma managed(pop)