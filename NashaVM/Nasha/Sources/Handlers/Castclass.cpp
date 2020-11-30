#pragma once
#include "./../../Headers/VMBody.hpp"
#include "./../../Headers/ParameterParser.hpp"
using namespace System::Reflection;
public class Castclass
{
public:
	Castclass() {}
	static void Run(msclr::gcroot<VMBody^>& body, msclr::gcroot<Object^> operand)
	{
		auto instruction = (Tuple<Int16, Int32>^)(Object^)operand;
		Int16 referenceID = instruction->Item1;
		Int32 operandToken = instruction->Item2;

		auto reference = AppDomain::CurrentDomain->Load(gcnew AssemblyName(msclr::interop::marshal_as<System::String^>(body->Global()->References->Lookup(referenceID))));
		
		auto typeBase = reference->ManifestModule->ResolveType(operandToken);
		body->GetStack()->Push(Parse(body->GetStack()->Pop(), typeBase));

	}
	static gcroot<Object^> Constructor(msclr::gcroot<VMBody^>& body)
	{
		return (Object^)gcnew Tuple<Int16, Int32>(body->GetReader()->ReadInt16(), body->GetReader()->ReadInt32());
	}
	static Object^ Parse(Object^ value, Type^ type)
	{
		if (type->BaseType == Enum::typeid)
			return Enum::Parse(type, value->ToString());
		try
		{
			return Convert::ChangeType(value, type);
		}
		catch (...)
		{
			return (Object^)ParameterParser::ChangeType((int)value, type);
		}
	}
};
