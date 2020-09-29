#pragma once
#include "./../../Headers/VMBody.hpp"
public class Ldsfld
{
public:
	Ldsfld() {}
	static void Run(msclr::gcroot<VMBody^>& body, msclr::gcroot<Object^> operand)
	{
		auto instruction = (Tuple<Int16, Int32>^)(Object^)operand;
		Int16 referenceID = instruction->Item1;
		Int32 fieldToken = instruction->Item2;

		auto reference = AppDomain::CurrentDomain->Load(gcnew System::Reflection::AssemblyName(msclr::interop::marshal_as<System::String^>(body->Global()->References->Lookup(referenceID))));
		auto field = reference->ManifestModule->ResolveField(fieldToken);
		auto instanceField = field->IsStatic ? nullptr : body->GetStack()->Pop();
		body->GetStack()->Push(field->GetValue(instanceField));

	}
	static gcroot<Object^> Constructor(msclr::gcroot<VMBody^>& body)
	{
		return gcnew Tuple<Int16, Int32>(body->GetReader()->ReadInt16(), body->GetReader()->ReadInt32());
	}
};