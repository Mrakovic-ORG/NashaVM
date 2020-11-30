#pragma once
#include "./../../Headers/VMBody.hpp"
public class Ldloc
{
public:
	Ldloc() {}
	static void Run(msclr::gcroot<VMBody^>& body, msclr::gcroot<Object^> operand)
	{
		body->GetStack()->Push(body->GetLocals()->Lookup((Int32)(Object^)operand));
	}
	static gcroot<Object^> Constructor(msclr::gcroot<VMBody^>& body)
	{
		return (Object^)body->GetReader()->ReadInt32();
	}
};
