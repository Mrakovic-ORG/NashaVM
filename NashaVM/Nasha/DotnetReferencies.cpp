#include "DotnetReferencies.hpp"
#pragma managed(push,off)

/// <summary>
/// A class designated to store all references coming from assembly.net.
/// </summary>
DotnetReferencies::DotnetReferencies()
{
	_referencies = new std::vector<std::string>();
}

/// <summary>
/// Push new reference to referencies list.
/// </summary>
void DotnetReferencies::New(std::string ReferenceName)
{
	_referencies->push_back(ReferenceName);
}

/// <summary>
/// Find the referencie by their id.
/// </summary>
/// <param name="id">Referencie ID</param>
/// <returns>Reference Name</returns>
std::string DotnetReferencies::Lookup(int id)
{
	return _referencies->at(id);
}
#pragma managed(pop)