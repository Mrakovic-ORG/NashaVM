#pragma once
#include <string>
#include <vector>

public class DotnetReferencies
{
public:
	DotnetReferencies();
private:
	std::vector<std::string>* _referencies;
public:
	void New(std::string ReferenceName);
	std::string Lookup(int id);
};
