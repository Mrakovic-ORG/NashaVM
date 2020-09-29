#include "./../Headers/DotnetReferences.hpp"
#pragma managed(push,off)

/// <summary>
/// A class designed to store all references coming from assembly.net.
/// </summary>
DotnetReferences::DotnetReferences()
{
	_references = new std::vector<std::string>();
}

/// <summary>
/// Push new reference to references list.
/// </summary>
void DotnetReferences::New(std::string ReferenceName)
{
	_references->push_back(ReferenceName);
}

/// <summary>
/// Find the reference by its id.
/// </summary>
/// <param name="id">Reference ID</param>
/// <returns>Reference Name</returns>
std::string DotnetReferences::Lookup(int id)
{
	return _references->at(id);
}
#pragma managed(pop)