#pragma once
#include <msclr\marshal_cppstd.h>
public ref class Locals 
{
public:
	Locals();
	void Insert(int index, gcroot<System::Object^> Value);
	msclr::gcroot<System::Object^> Lookup(int index);
private:
	cli::array<System::Object^>^ _locals;
};