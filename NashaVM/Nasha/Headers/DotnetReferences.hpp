#pragma once
#include <string>
#include <vector>

public class DotnetReferences
{
public:
	DotnetReferences();
private:
	std::vector<std::string>* _references;
public:
	void New(std::string ReferenceName);
	std::string Lookup(int id);
};
