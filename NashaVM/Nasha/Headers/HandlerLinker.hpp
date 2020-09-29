#pragma once

public class HandlerLinker {
public:
	HandlerLinker();
	void* OpcodesPointers[255];
	void* DeserializationPointers[255];
};
