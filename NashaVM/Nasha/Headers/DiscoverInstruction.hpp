#pragma once
public class DiscoverInstruction
{
public:
	DiscoverInstruction(int opcode, int operand);
	int GetOpcode();
	int GetOperand();
private:
	int _opcode;
	int _operand;
};