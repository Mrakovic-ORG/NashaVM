#include "./../Headers/Stack.hpp"
#pragma managed(push,off)

/// <summary>
/// Creates a class that replicates the C# System.Collections.Stack natively
/// </summary>
NStack::NStack() {}

/// <summary>
/// Push a object to stack.
/// </summary>
void NStack::Push(msclr::gcroot<Object^> object)
{
	_stack.push_back(object);
}

/// <summary>
/// Populate a object from stack.
/// </summary>
msclr::gcroot<Object^> NStack::Pop()
{
	auto popedValue = _stack.back();
	_stack.pop_back();
	return popedValue;
}

/// <summary>
/// Populate a object from stack without removing it.
/// </summary>
msclr::gcroot<Object^> NStack::Peek()
{
	return _stack.back();
}

/// <summary>
/// Picks up the amount of objects that are in the stack.
/// </summary>
int NStack::Count()
{
	return _stack.size();
}
#pragma managed(pop)