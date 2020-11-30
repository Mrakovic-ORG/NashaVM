#pragma once
#include "./../../Headers/VMBody.hpp"
public class Stloc
{
public:
	Stloc() {}
	static void Run(msclr::gcroot<VMBody^>& body, msclr::gcroot<Object^> operand)
	{
		body->GetLocals()->Insert((int)(Object^)operand, body->GetStack()->Pop());

	}
	static gcroot<Object^> Constructor(msclr::gcroot<VMBody^>& body)
	{
		return (Object^)body->GetReader()->ReadInt32();
	}
};
