#include "Globals.hpp"
#pragma managed(push,off)
/// <summary>
/// A Global Static Class (GSC), that can be accesed globaly.
/// </summary>
GSC::GSC()
{
	// Instantiates the handler linking class.
	Handlers = new HandlerLinker();
	// Instantiates the DotnetReferencies class.
	Referencies = new DotnetReferencies();
}
#pragma managed(pop)