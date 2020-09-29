#pragma once
#include "./../../Headers/VMBody.hpp"
public class Ret
{
public:
	Ret() {}
	static void Run(msclr::gcroot<VMBody^>& body, msclr::gcroot<Object^> operand)
	{
		if (body->GetStack()->Count() > 0) {
			body->SetReturnedObject((Object^)(body->GetStack()->Pop()));
			return;
		}
		body->SetReturnedObject(nullptr);
	}
	static gcroot<Object^> Constructor(msclr::gcroot<VMBody^>& body)
	{
		return nullptr;
	}
};