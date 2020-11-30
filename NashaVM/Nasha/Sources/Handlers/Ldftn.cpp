#pragma once
#include "./../../Headers/VMBody.hpp"
using namespace System::Reflection;
public class Ldftn
{
public:
	Ldftn() {}
	static void Run(msclr::gcroot<VMBody^>& body, msclr::gcroot<Object^> operand)
	{
		auto proxi = (Tuple<Int16, Int32>^)(Object^)operand;
		Int16 referenceID = proxi->Item1;
		Int32 ldftnToken = proxi->Item2;

		auto reference = AppDomain::CurrentDomain->Load(gcnew AssemblyName(msclr::interop::marshal_as<System::String^>(body->Global()->References->Lookup(referenceID))));
		auto method = reference->ManifestModule->ResolveMethod(ldftnToken);
		MethodInfo^ methodInfo = (MethodInfo^)method;
		body->GetStack()->Push((Object^)methodInfo->MethodHandle.GetFunctionPointer());
	}
	static gcroot<Object^> Constructor(msclr::gcroot<VMBody^>& body)
	{
		return (Object^)gcnew Tuple<Int16, Int32>(body->GetReader()->ReadInt16(), body->GetReader()->ReadInt32());
	}
};