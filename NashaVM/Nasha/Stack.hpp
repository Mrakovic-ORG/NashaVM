#pragma once
#include <msclr\marshal_cppstd.h>
#include <vector>
using namespace std;
using namespace System;

public  class NStack
{
private:
	vector<msclr::gcroot<Object^>> _stack;
public:
	NStack();
	void Push(msclr::gcroot<Object^> object);
	msclr::gcroot<Object^> Pop();
	msclr::gcroot<Object^> Peek();
	int Count();
};
