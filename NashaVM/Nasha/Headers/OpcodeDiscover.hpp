#pragma once
#include "./../Headers/Stack.hpp"
#include "DiscoverInstruction.hpp"
#include "NStringReader.hpp"
#include <tuple>

public class OpcodeDiscover
{
public:
	OpcodeDiscover(std::string DiscoverData);
	int GetOpcodeAllocatedInto(int StackID);
	void AllocateOpcode(int Opcode);
	void DiscoverInterpreter();
private:
	tuple<int, int> Lookup(vector<tuple<int, int>> source, int find)
	{
		for (auto x : source)
		{
			if (get<0>(x) == find)
				return tuple<int, int>(get<0>(x), get<1>(x));
		}
	}
	vector<int>* _stack;
	std::string _data;
};