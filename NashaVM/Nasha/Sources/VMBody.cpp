#include "./../Headers/VMBody.hpp"

/// <summary>
/// Create a <see cref="VMBody"/> to store important values for the running method.
/// </summary>
VMBody::VMBody(msclr::gcroot<BinaryReader^> reader, msclr::gcroot<cli::array<Object^>^> parameters, GSC* gsc)
{
	// Extracted from Parameters
	_reader = new msclr::gcroot<BinaryReader^>(reader);
	_parameters = new msclr::gcroot<cli::array<Object^>^>(parameters);
	_glob = gsc;

	// Default Static Initial Value
	_flow = new msclr::gcroot<Int32^>(0);
	_stack = new NStack();
	_locals = gcnew Locals();
	_arithmetics = gcnew Arithmetics();
}

/// <summary>
/// Get the parameters assigned to this body.
/// </summary>
/// <returns>List of parameters as object</returns>
msclr::gcroot<cli::array<Object^>^> VMBody::GetParameters()
{
	return (*_parameters);
}

/// <summary>
/// Get the BinaryReader assigned to this body.
/// </summary>
msclr::gcroot<BinaryReader^> VMBody::GetReader()
{
	return (msclr::gcroot<BinaryReader^>)(*_reader);
}

/// <summary>
/// Check if the method is returning any value.
/// </summary>
bool VMBody::IsMethodReturning()
{
	return _isReturning;
}

/// <summary>
/// Get the value that was returned by the method.
/// </summary>
/// <returns>Object returned from method</returns>
msclr::gcroot<Object^> VMBody::GetReturnedObject()
{
	return *_return;
}

/// <summary>
/// Set the value that the method is returning.
/// </summary>
void VMBody::SetReturnedObject(msclr::gcroot<Object^> object)
{
	_isReturning = true;
	_return = new msclr::gcroot<Object^>(object);
}

/// <summary>
/// Get the current flow position of the method.
/// </summary>
msclr::gcroot<Int32^> VMBody::GetActualFlow()
{
	return *_flow;
}

/// <summary>
/// Set the current flow postion of the method.
/// </summary>
void VMBody::SetActualFlow(msclr::gcroot<Int32^> flowValue)
{
	_flow = new msclr::gcroot<Int32^>(flowValue);
}

/// <summary>
/// Get the stack structure of the method.
/// </summary>
NStack* VMBody::GetStack()
{
	return _stack;
}

/// <summary>
/// Get the Global Static Class(GSC).
/// </summary>
GSC* VMBody::Global()
{
	return _glob;
}

/// <summary>
/// Get the locals from body.
/// </summary>
Locals^ VMBody::GetLocals()
{
	return _locals;
}

Arithmetics^ VMBody::GetArithmetics()
{
	return _arithmetics;
}