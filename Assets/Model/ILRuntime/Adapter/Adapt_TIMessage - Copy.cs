// using System;
// using Google.Protobuf;
// using Google.Protobuf.Reflection;
// using ILRuntime.CLR.Method;
// using ILRuntime.Runtime.Enviorment;
// using ILRuntime.Runtime.Intepreter;
//
// public class TIMessageAdapter : CrossBindingAdaptor
// {
//     //定义访问方法的方法信息
//     // static CrossBindingMethodInfo mVMethod1_0 = new CrossBindingMethodInfo("VMethod1");
//     // static CrossBindingFunctionInfo<System.Boolean> mVMethod2_1 = new CrossBindingFunctionInfo<System.Boolean>("VMethod2");
//     // static CrossBindingMethodInfo mAbMethod1_3 = new CrossBindingMethodInfo("AbMethod1");
//     // static CrossBindingFunctionInfo<System.Int32, System.Single> mAbMethod2_4 = new CrossBindingFunctionInfo<System.Int32, System.Single>("AbMethod2");
//     public override Type BaseCLRType
//     {
//         get
//         {
//             return typeof(IMessage<>); //这里是你想继承的类型
//         }
//     }
//
//     public override Type AdaptorType
//     {
//         get { return typeof(Adapter); }
//     }
//
//     public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
//     {
//         return new Adapter(appdomain, instance);
//     }
//
//     public class Adapter : IMessage<>, CrossBindingAdaptorType
//     {
//         ILTypeInstance instance;
//         ILRuntime.Runtime.Enviorment.AppDomain appdomain;
//
//         //必须要提供一个无参数的构造函数
//         public Adapter()
//         {
//         }
//
//         public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
//         {
//             this.appdomain = appdomain;
//             this.instance = instance;
//         }
//
//         public ILTypeInstance ILInstance
//         {
//             get { return instance; }
//         }
//
//         //下面将所有虚函数都重载一遍，并中转到热更内
//         // public override void VMethod1()
//         // {
//         //     if (mVMethod1_0.CheckShouldInvokeBase(this.instance))
//         //         base.VMethod1();
//         //     else
//         //         mVMethod1_0.Invoke(this.instance);
//         // }
//         //
//         // public override System.Boolean VMethod2()
//         // {
//         //     if (mVMethod2_1.CheckShouldInvokeBase(this.instance))
//         //         return base.VMethod2();
//         //     else
//         //         return mVMethod2_1.Invoke(this.instance);
//         // }
//         //
//         // protected override void AbMethod1()
//         // {
//         //     mAbMethod1_3.Invoke(this.instance);
//         // }
//         //
//         // public override System.Single AbMethod2(System.Int32 arg1)
//         // {
//         //     return mAbMethod2_4.Invoke(this.instance, arg1);
//         // }
//
//         public override string ToString()
//         {
//             IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
//             m = instance.Type.GetVirtualMethod(m);
//             if (m == null || m is ILMethod)
//             {
//                 return instance.ToString();
//             }
//             else
//                 return instance.Type.FullName;
//         }
//
//         public void MergeFrom(CodedInputStream input)
//         {
//             // appdomain.Instantiate()
//             throw new NotImplementedException();
//         }
//
//         public void WriteTo(CodedOutputStream output)
//         {
//             throw new NotImplementedException();
//         }
//
//         public int CalculateSize()
//         {
//             throw new NotImplementedException();
//         }
//
//         public MessageDescriptor Descriptor { get; }
//     }
// }