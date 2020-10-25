#pragma once
#include <msclr\marshal_cppstd.h>
using namespace System;
public class ParameterParser
{
public:
	static gcroot<cli::array<Object^>^> Parser(gcroot<cli::array<Object^>^> parameters, System::Reflection::MethodBase^ method);
	static gcroot<Object^> ChangeType(int value, gcroot<Type^> type);
};