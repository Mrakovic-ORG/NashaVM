#pragma once
#include "./../../Headers/VMBody.hpp"
public class Mul
{
public:
	Mul() {}
	static void Run(msclr::gcroot<VMBody^>& body, msclr::gcroot<Object^> operand)
	{
		Object^ number2 = body->GetStack()->Pop();
		Object^ number1 = body->GetStack()->Pop();

		auto Result = body->GetArithmetics()->MulCS->Target(body->GetArithmetics()->MulCS, number1, number2);
		body->GetStack()->Push(Result);
	}

	static gcroot<Object^> Constructor(msclr::gcroot<VMBody^>& body)
	{
		return nullptr;
	}
};