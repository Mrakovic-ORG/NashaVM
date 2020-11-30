#include <vector>
#include "./../Headers/VMBody.hpp"
#include "./../Headers/SectionReader.hpp"
#include "./../Headers/Config.hpp"
#include "./NativeProxies.cpp"
#pragma endregion
public ref class Main
{
public:
	Main() {}
	Object^ Execute(cli::array<System::Object^>^ Parameters, int ID, Config^ cfg)
	{
		msclr::interop::marshal_context ctx;
		msclr::gcroot<Object^>* OperandPtr = new msclr::gcroot<Object^>();
		size_t InstructionsCount = 0;
		msclr::gcroot<VMBody^>* body = new msclr::gcroot<VMBody^>();
		vector<msclr::gcroot<Tuple<Int32, Object^>^>> Instructions;

		// Get calling assembly from .NET
		auto CallingAssembly = System::Reflection::Assembly::GetCallingAssembly();

		// Instance a VMBody from .Nasha0 for the actual body
		*body = gcnew VMBody(cfg->glob->VMBytes->Get(ID), Parameters, cfg->glob);

		// Get the amount of instructions coming from the current body
		InstructionsCount = (*body)->GetReader()->ReadInt32();

		//
		// Body Struct Example:
		// 0b -> The amount of instructions in the body
		// 1b -> Instruction ID (example: 1 -> Integer)
		// 2b..6b -> Operand (example: 0x04D2)
		//

		for (size_t i = 0; i < InstructionsCount; ++i)
		{
			// Get the ID of an instruction that goes to the handler
			Int32 handID = (*body)->GetReader()->ReadByte();

			// Add the handled instruction to Instructions list
			Instructions.push_back(*new msclr::gcroot<Tuple<Int32, Object^>^>(
				gcnew Tuple<Int32, Object^>(handID, NativeProxy(body, handID, cfg->glob))));
		}
		for (int i = 0; i < Instructions.size();)
		{
			// Check if the body returns any value
			if ((*body)->IsMethodReturning())
			{
				// Returns the value gathered by the body
				return (*body)->GetReturnedObject();
			}
			*OperandPtr = Instructions[i]->Item2;

			// NativeProxy2 is responsible for executing an instruction with its operand
			NativeProxy2(body, OperandPtr, Instructions[i]->Item1, cfg->glob);

			// Yes, sorry for the spaghetti code...
			msclr::gcroot<Int32^> NewFlow = (Int32^)((Int32)(Int32^)(*body)->GetActualFlow() + 1);
			(*body)->SetActualFlow(NewFlow);
			i = (Int32)(Int32^)(*body)->GetActualFlow();
		}
		return (*body)->GetReturnedObject();
	}
};

int main()
{
	
}