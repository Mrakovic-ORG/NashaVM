#pragma once
#include "HandlerLinker.hpp"
#include "DotnetReferences.hpp"
#include "VMBytes.hpp"
public class GSC
{
public:
	GSC();
	HandlerLinker* Handlers;
	DotnetReferences* References;
	VMBytes* VMBytes;
};
