#pragma once
#include "./../../Headers/VMBody.hpp"
#include "./../../Headers/ParameterParser.hpp"
using namespace System::Reflection;
public class Brfalse
{
public:
	Brfalse() {}
	static void Run(msclr::gcroot<VMBody^>& body, msclr::gcroot<Object^> operand)
	{
		Int32 Postion = (Int32)(Object^)operand;
		auto Value = (Object^)body->GetStack()->Pop();

		if (Value != nullptr)
		{
			if (dynamic_cast<String^>(Value) != nullptr)
			{
				if ((String^)Value == nullptr)
				{
					body->SetActualFlow((Int32^)(Postion));
				}
			}
			else
			{
				if (!(bool)Value)
					body->SetActualFlow((Int32^)(Postion));
			}
		}
	}
	static gcroot<Object^> Constructor(msclr::gcroot<VMBody^>& body)
	{
		return body->GetReader()->ReadInt32();
	}
};
