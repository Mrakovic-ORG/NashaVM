#pragma once
#include "./../../Headers/VMBody.hpp"
#include "./../../Headers/ParameterParser.hpp"
using namespace System::Reflection;
public class Newobj
{
public:
	Newobj() {}
	static void Run(msclr::gcroot<VMBody^>& body, msclr::gcroot<Object^> operand)
	{
		auto instruction = (Tuple<Int16, Int32>^)(Object^)operand;
		Int16 referenceID = instruction->Item1;
		Int32 operandToken = instruction->Item2;

		auto reference = AppDomain::CurrentDomain->Load(gcnew AssemblyName(msclr::interop::marshal_as<System::String^>(body->Global()->References->Lookup(referenceID))));

		auto constructorBase = (ConstructorInfo^)reference->ManifestModule->ResolveMethod(operandToken);
		int num = constructorBase->GetParameters()->Length;

		cli::array<Object^>^ array = gcnew cli::array<Object^>(num);
		for (int i = num - 1; i >= 0; i--)
		{
			array[i] = body->GetStack()->Pop();
		}
		array = ParameterParser::Parser(array, constructorBase);
		if (constructorBase->DeclaringType->IsGenericType)
		{
			body->GetStack()->Push(constructorBase->Invoke(array));
			return;
		}
		body->GetStack()->Push(Activator::CreateInstance(constructorBase->DeclaringType, array));
	}
	static gcroot<Object^> Constructor(msclr::gcroot<VMBody^>& body)
	{
		return gcnew Tuple<Int16, Int32>(body->GetReader()->ReadInt16(), body->GetReader()->ReadInt32());
	}
};
