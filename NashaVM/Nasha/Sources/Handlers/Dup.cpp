#pragma once
#include "./../../Headers/VMBody.hpp"
public class Dup
{
public:
	Dup() {}
	static void Run(msclr::gcroot<VMBody^>& body, msclr::gcroot<Object^> operand)
	{
		auto pop = body->GetStack()->Peek();
		body->GetStack()->Push(pop);
	}
	static gcroot<Object^> Constructor(msclr::gcroot<VMBody^>& body)
	{
		return nullptr;
	}
};