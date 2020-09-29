#pragma once
#include "./../../Headers/VMBody.hpp"
public class Nop
{
public:
	Nop() {}
	static void Run(msclr::gcroot<VMBody^>& body, msclr::gcroot<Object^> operand)
	{
	}
	static gcroot<Object^> Constructor(msclr::gcroot<VMBody^>& body)
	{
		return nullptr;
	}
};