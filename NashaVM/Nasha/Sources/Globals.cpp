#include "./../Headers/Globals.hpp"
#pragma managed(push,off)
/// <summary>
/// Global Static Class (GSC), It can be accessed globaly.
/// </summary>
GSC::GSC()
{
	// Instantiates the DotnetReferences class.
	References = new DotnetReferences();
}
#pragma managed(pop)