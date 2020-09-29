#pragma once
#include "./../../Headers/VMBody.hpp"
public class Pop
{
public:
	Pop() {}
	static void Run(msclr::gcroot<VMBody^>& body, msclr::gcroot<Object^> operand)
	{
		body->GetStack()->Pop();
	}
	static gcroot<Object^> Constructor(msclr::gcroot<VMBody^>& body)
	{
		return nullptr;
	}
};