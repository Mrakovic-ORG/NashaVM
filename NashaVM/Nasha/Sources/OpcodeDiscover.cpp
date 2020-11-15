#include "./../Headers/OpcodeDiscover.hpp"
#pragma managed(push,off)

OpcodeDiscover::OpcodeDiscover(std::string DiscoverData)
{
	// Extracted from Parameters
	_stack = new vector<int>();
	_data = DiscoverData;

	// Start Interpreter
	DiscoverInterpreter();
}

/// <summary>
/// Get opcode allocate by ID
/// </summary>
/// <param name="StackIndex"></param>
/// <returns></returns>
int OpcodeDiscover::GetOpcodeAllocatedInto(int StackID)
{
	return _stack->at(StackID);
}

/// <summary>
/// Allocate new opcode to the stack list
/// </summary>
/// <param name="Opcode"></param>
void OpcodeDiscover::AllocateOpcode(int Opcode)
{
	_stack->push_back(Opcode);
}
#pragma managed(pop)

void OpcodeDiscover::DiscoverInterpreter()
{
	// Instance a pseudo BinaryReader using _data as parameter
	NStringReader* reader = new NStringReader(_data);

	// Vectors for DiscoverInstructions and Blocks
	vector<DiscoverInstruction> DiscoverInstructions;
	vector<tuple<int, int>> Blocks;

	for (size_t i = 0; i < _data.length(); ++i)
	{
		// Push the Opcode and Operand to the DiscoverInstruction
		DiscoverInstructions.push_back(DiscoverInstruction(reader->ReadInt32(), reader->ReadInt32()));
		i = reader->GetBuffer();
	}

	/*
	0 = Goto block
	1 = Push into the Stack
	2 = Exit Emulator
	4 = Set new block
	*/

	for (size_t x = 0; x < DiscoverInstructions.size(); ++x)
	{
		switch (DiscoverInstructions[x].GetOpcode())
		{
		case 4:
			// Set new block
			Blocks.push_back(tuple<int, int>{DiscoverInstructions[x].GetOperand(), x});
			break;
		default:
			continue;
		}
	}

	for (size_t i = 0; i < DiscoverInstructions.size(); ++i)
	{
		switch (DiscoverInstructions[i].GetOpcode())
		{
		case 0:
			// Goto block
			i = get<1>(Lookup(Blocks, DiscoverInstructions[i].GetOperand()));
			break;
		case 1:
			// Push operand into the stack
			AllocateOpcode(DiscoverInstructions[i].GetOperand());
			break;
		case 2:
			// Exit the emulator by setting a large index
			i = 9999;
			break;
		default:
			continue;
		}
	}
}
