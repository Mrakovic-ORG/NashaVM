#include "./../Headers/Locals.hpp"
Locals::Locals()
{
	_locals = gcnew cli::array<System::Object^>(50);
}

/// <summary>
/// Insert new value to the local stack by the specificed index.
/// </summary>
void Locals::Insert(int index, msclr::gcroot<System::Object^> Value)
{
	_locals[index] = Value;
}

/// <summary>
/// Lookup a local stack by its specified index.
/// </summary>
msclr::gcroot<System::Object^> Locals::Lookup(int index)
{
	return _locals[index];
}