#pragma once
#include "./../../Headers/VMBody.hpp"
public class Ldarg
{
public:
	Ldarg() {}
	static void Run(msclr::gcroot<VMBody^>& body, msclr::gcroot<Object^> operand)
	{
		Int16 ArgumentID = (Int16)(Object^)operand;
		body->GetStack()->Push(body->GetParameters()[ArgumentID]);
	}
	static gcroot<Object^> Constructor(msclr::gcroot<VMBody^>& body)
	{
		return (Object^)body->GetReader()->ReadInt16();
	}
};