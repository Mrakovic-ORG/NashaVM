#pragma once
#include "./../../Headers/VMBody.hpp"
public class Ldstr
{
public:
	Ldstr() {}
	static void Run(msclr::gcroot<VMBody^>& body, msclr::gcroot<Object^> operand)
	{
		cli::array<Byte>^ strB = (cli::array<Byte>^)(Object^)operand;
		String^ strX = Text::Encoding::UTF8->GetString(strB);

		body->GetStack()->Push(strX);
	}
	static gcroot<Object^> Constructor(msclr::gcroot<VMBody^>& body)
	{
		Int32 len = body->GetReader()->ReadInt32();
		return body->GetReader()->ReadBytes(len);
	}
};
