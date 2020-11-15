#pragma once
#include "./../../Headers/VMBody.hpp"
public class LdcR8
{
public:
	LdcR8() {}
	static void Run(msclr::gcroot<VMBody^>& body, msclr::gcroot<Object^> operand)
	{
		body->GetStack()->Push((Double^)(Object^)operand);
	}
	static gcroot<Object^> Constructor(msclr::gcroot<VMBody^>& body)
	{
		return (Object^)body->GetReader()->ReadDouble();
	}
};