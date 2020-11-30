#pragma once
#include "./../../Headers/VMBody.hpp"
public class Br
{
public:
	Br() {}
	static void Run(msclr::gcroot<VMBody^>& body, msclr::gcroot<Object^> operand)
	{
		body->SetActualFlow((Int32^)(Object^)operand);
	}
	static gcroot<Object^> Constructor(msclr::gcroot<VMBody^>& body)
	{
		return (Object^)body->GetReader()->ReadInt32();
	}
};