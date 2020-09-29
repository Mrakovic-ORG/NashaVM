#pragma once
#include "./../../Headers/VMBody.hpp"
public class LdcI4 
{
public:
	LdcI4() {}
	static void Run(msclr::gcroot<VMBody^>& body, msclr::gcroot<Object^> operand)
	{
		body->GetStack()->Push((Int32^)(Object^)operand);
	}
	static gcroot<Object^> Constructor(msclr::gcroot<VMBody^>& body)
	{
		return (Object^)body->GetReader()->ReadInt32();
	}
};
