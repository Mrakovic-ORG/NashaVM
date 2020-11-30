#pragma once
#include "./../../Headers/VMBody.hpp"
#include "./../../Headers/ParameterParser.hpp"
using namespace System::Reflection;
public class Call
{
public:
	Call() {}
	static void Run(msclr::gcroot<VMBody^>& body, msclr::gcroot<Object^> operand)
	{
		auto instruction = (Tuple<Int16, Int32>^)(Object^)operand;
		Int16 referenceID = instruction->Item1;
		Int32 operandToken = instruction->Item2;

		auto reference = AppDomain::CurrentDomain->Load(gcnew AssemblyName(msclr::interop::marshal_as<System::String^>(body->Global()->References->Lookup(referenceID))));

		auto methodBase = reference->ManifestModule->ResolveMethod(operandToken);
		int num = methodBase->GetParameters()->Length;

		cli::array<Object^>^ array = gcnew cli::array<Object^>(num);
		for (int i = num - 1; i >= 0; i--)
		{
			array[i] = body->GetStack()->Pop();
		}
		array = ParameterParser::Parser(array, methodBase);

		if (methodBase->IsConstructor)
		{
			body->GetStack()->Push(methodBase->Invoke(methodBase->IsStatic ? nullptr : body->GetStack()->Pop(), array));
			return;
		}

		MethodInfo^ methodInfo = (MethodInfo^)methodBase;
		Object^ InvokeReturn = methodInfo->Invoke(methodInfo->IsStatic ? nullptr : body->GetStack()->Pop(), array);
		if (methodInfo->ReturnType != void::typeid)
		{
			body->GetStack()->Push(InvokeReturn);
		}
	}
	static gcroot<Object^> Constructor(msclr::gcroot<VMBody^>& body)
	{
		return (Object^)gcnew Tuple<Int16, Int32>(body->GetReader()->ReadInt16(), body->GetReader()->ReadInt32());
	}
};
