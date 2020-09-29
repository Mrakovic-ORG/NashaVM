#pragma once
#include "HandlerLinker.hpp"
#include "DotnetReferences.hpp"
public class GSC
{
public:
	GSC();
	HandlerLinker* Handlers;
	DotnetReferences* References;
};
