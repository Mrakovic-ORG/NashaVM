#pragma once
#using <Microsoft.CSharp.dll>
using namespace System::Runtime::CompilerServices;
using namespace Microsoft::CSharp::RuntimeBinder;
public ref class Arithmetics
{
public:
	CallSite<System::Func<CallSite^, Object^, Object^, Object^>^>^ AddCS;
	CallSite<System::Func<CallSite^, Object^, Object^, Object^>^>^ SubCS;
	CallSite<System::Func<CallSite^, Object^, Object^, Object^>^>^ XorCS;
	CallSite<System::Func<CallSite^, Object^, Object^, Object^>^>^ DivCS;
	CallSite<System::Func<CallSite^, Object^, Object^, Object^>^>^ MulCS;

	Arithmetics()
	{
		AddCS = CallSite<System::Func<CallSite^, Object^, Object^, Object^>^>::Create(Binder::BinaryOperation(CSharpBinderFlags::None, System::Linq::Expressions::ExpressionType::Add, Arithmetics::typeid, gcnew cli::array<CSharpArgumentInfo^>
		{
			CSharpArgumentInfo::Create(CSharpArgumentInfoFlags::None, nullptr),
			CSharpArgumentInfo::Create(CSharpArgumentInfoFlags::None, nullptr)
		}));

		SubCS = CallSite<System::Func<CallSite^, Object^, Object^, Object^>^>::Create(Binder::BinaryOperation(CSharpBinderFlags::None, System::Linq::Expressions::ExpressionType::Subtract, Arithmetics::typeid, gcnew cli::array<CSharpArgumentInfo^>
		{
			CSharpArgumentInfo::Create(CSharpArgumentInfoFlags::None, nullptr),
			CSharpArgumentInfo::Create(CSharpArgumentInfoFlags::None, nullptr)
		}));

		XorCS = CallSite<System::Func<CallSite^, Object^, Object^, Object^>^>::Create(Binder::BinaryOperation(CSharpBinderFlags::None, System::Linq::Expressions::ExpressionType::ExclusiveOr, Arithmetics::typeid, gcnew cli::array<CSharpArgumentInfo^>
		{
			CSharpArgumentInfo::Create(CSharpArgumentInfoFlags::None, nullptr),
			CSharpArgumentInfo::Create(CSharpArgumentInfoFlags::None, nullptr)
		}));

		DivCS = CallSite<System::Func<CallSite^, Object^, Object^, Object^>^>::Create(Binder::BinaryOperation(CSharpBinderFlags::None, System::Linq::Expressions::ExpressionType::Divide, Arithmetics::typeid, gcnew cli::array<CSharpArgumentInfo^>
		{
			CSharpArgumentInfo::Create(CSharpArgumentInfoFlags::None, nullptr),
			CSharpArgumentInfo::Create(CSharpArgumentInfoFlags::None, nullptr)
		}));

		MulCS = CallSite<System::Func<CallSite^, Object^, Object^, Object^>^>::Create(Binder::BinaryOperation(CSharpBinderFlags::None, System::Linq::Expressions::ExpressionType::Multiply, Arithmetics::typeid, gcnew cli::array<CSharpArgumentInfo^>
		{
			CSharpArgumentInfo::Create(CSharpArgumentInfoFlags::None, nullptr),
			CSharpArgumentInfo::Create(CSharpArgumentInfoFlags::None, nullptr)
		}));
	}
};