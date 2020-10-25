#pragma once
#include "Globals.hpp"
public ref class Config
{
public:
	GSC* glob;
	Config();
	void SetupReferences();
	void SetupDiscover();
	void SetupBody();
};