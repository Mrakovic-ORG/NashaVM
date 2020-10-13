#include "./../Headers/DiscoverInstruction.hpp"
#pragma managed(push,off)

DiscoverInstruction::DiscoverInstruction(int opcode, int operand)
{
	_opcode = opcode;
	_operand = operand;
}

int DiscoverInstruction::GetOpcode()
{
	return _opcode;
}

int DiscoverInstruction::GetOperand()
{
	return _operand;
}
#pragma managed(pop)