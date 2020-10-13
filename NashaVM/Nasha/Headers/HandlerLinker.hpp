#pragma once
#include "OpcodeDiscover.hpp"

public class HandlerLinker {
public:
	HandlerLinker(OpcodeDiscover* discover);
	void* OpcodesPointers[255];
	void* DeserializationPointers[255];
};
