#include "./../Headers/ParameterParser.hpp"
gcroot<cli::array<Object^>^> ParameterParser::Parser(gcroot<cli::array<Object^>^> parameters, System::Reflection::MethodBase^ method) 
{
	cli::array<Object^>^ Fixedparameters = gcnew cli::array<Object^>(parameters->Length);
	if (method->GetParameters()->Length == 0)
		return parameters;
	for each (auto param in method->GetParameters())
	{
		int index = System::Linq::Enumerable::ToList(((System::Collections::Generic::IEnumerable<System::Reflection::ParameterInfo^>^)method->GetParameters()))->IndexOf(param);
		if (param->ParameterType->IsGenericParameter)
			continue;

		auto isInt = dynamic_cast<int^>(parameters[index]) != nullptr;

		if (isInt && param->ParameterType != parameters[index]->GetType() && param->ParameterType != Object::typeid)
		{
			if (param->ParameterType->BaseType == Enum::typeid)
			{
				Fixedparameters[index] = Enum::Parse(param->ParameterType, parameters[index]->ToString());
				continue;
			}
			try
			{
				Fixedparameters[index] = Convert::ChangeType((int)parameters[index], param->ParameterType);
			}
			catch (...)
			{
				Fixedparameters[index] = ChangeType((int)parameters[index], param->ParameterType);
			}
			continue;
		}
		Fixedparameters[index] = parameters[index];
	}
	return Fixedparameters;
}

gcroot<Object^> ParameterParser::ChangeType(int value, gcroot<Type^> type) 
{
	System::Reflection::Emit::DynamicMethod^ dynamic_ = gcnew System::Reflection::Emit::DynamicMethod("", Object::typeid, nullptr);
	System::Reflection::Emit::ILGenerator^ ilgen = dynamic_->GetILGenerator();

	ilgen->Emit(System::Reflection::Emit::OpCodes::Ldc_I4, value);
	ilgen->Emit(System::Reflection::Emit::OpCodes::Box, type);
	ilgen->Emit(System::Reflection::Emit::OpCodes::Ret);
	Object^ test = dynamic_->Invoke(nullptr, gcnew cli::array<Object^>(0));
	return test;
}