#pragma once
#include "./../../Headers/VMBody.hpp"
public class Brtrue
{
public:
	Brtrue() {}
	static void Run(msclr::gcroot<VMBody^>& body, msclr::gcroot<Object^> operand)
	{
		Int32 Position = (Int32)(Object^)operand;
		auto Value = (Object^)body->GetStack()->Pop();
		if (Value != nullptr)
		{
			auto isBool = dynamic_cast<bool^>(Value) != nullptr;
			if (isBool)
			{
				if ((bool)Value)
				{
					body->SetActualFlow((Int32^)(Position));
				}
			}
			else 
			{
				body->SetActualFlow((Int32^)(Position));
			}
		}
	}
	static gcroot<Object^> Constructor(msclr::gcroot<VMBody^>& body)
	{
		return (Object^)body->GetReader()->ReadInt32();
	}
};