#pragma once
#include "./../../Headers/VMBody.hpp"
using namespace System::Reflection;
public class Stsfld
{
public:
	Stsfld() {}
	static void Run(msclr::gcroot<VMBody^>& body, msclr::gcroot<Object^> operand)
	{
		auto instruction = (Tuple<bool, Int16, Int32>^)(Object^)operand;
		bool isGeneric = instruction->Item1;
		Int16 referenceID = instruction->Item2;
		Int32 fieldToken = instruction->Item3;

		auto reference = AppDomain::CurrentDomain->Load(gcnew System::Reflection::AssemblyName(msclr::interop::marshal_as<System::String^>(body->Global()->References->Lookup(referenceID))));
		FieldInfo^ field;

		if (isGeneric)
		{
			auto fieldTypes = body->GetParameters()[0]->GetType()->GetGenericArguments();
			field = reference->ManifestModule->ResolveField(fieldToken, fieldTypes, nullptr);
		}
		else
		{
			field = reference->ManifestModule->ResolveField(fieldToken);
		}
		auto fieldValue = body->GetStack()->Pop();
		auto instanceField = field->IsStatic ? nullptr : body->GetStack()->Pop();
		try
		{
			fieldValue = Convert::ChangeType(fieldValue, field->FieldType);
		}
		catch (...)
		{
		}
		field->SetValue(instanceField, fieldValue);

	}
	static gcroot<Object^> Constructor(msclr::gcroot<VMBody^>& body)
	{
		return gcnew Tuple<bool, Int16, Int32>(body->GetReader()->ReadBoolean(), body->GetReader()->ReadInt16(), body->GetReader()->ReadInt32());
	}
};