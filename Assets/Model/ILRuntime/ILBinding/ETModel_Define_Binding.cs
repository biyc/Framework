using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class ETModel_Define_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.Define);

            field = type.GetField("IsDev", flag);
            app.RegisterCLRFieldGetter(field, get_IsDev_0);
            app.RegisterCLRFieldSetter(field, set_IsDev_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_IsDev_0, AssignFromStack_IsDev_0);
            field = type.GetField("GameSettings", flag);
            app.RegisterCLRFieldGetter(field, get_GameSettings_1);
            app.RegisterCLRFieldSetter(field, set_GameSettings_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_GameSettings_1, AssignFromStack_GameSettings_1);
            field = type.GetField("AssetBundleVersion", flag);
            app.RegisterCLRFieldGetter(field, get_AssetBundleVersion_2);
            app.RegisterCLRFieldSetter(field, set_AssetBundleVersion_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_AssetBundleVersion_2, AssignFromStack_AssetBundleVersion_2);


        }



        static object get_IsDev_0(ref object o)
        {
            return ETModel.Define.IsDev;
        }

        static StackObject* CopyToStack_IsDev_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ETModel.Define.IsDev;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_IsDev_0(ref object o, object v)
        {
            ETModel.Define.IsDev = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_IsDev_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @IsDev = ptr_of_this_method->Value == 1;
            ETModel.Define.IsDev = @IsDev;
            return ptr_of_this_method;
        }

        static object get_GameSettings_1(ref object o)
        {
            return ETModel.Define.GameSettings;
        }

        static StackObject* CopyToStack_GameSettings_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ETModel.Define.GameSettings;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_GameSettings_1(ref object o, object v)
        {
            ETModel.Define.GameSettings = (Blaze.Common.GameSettings)v;
        }

        static StackObject* AssignFromStack_GameSettings_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Blaze.Common.GameSettings @GameSettings = (Blaze.Common.GameSettings)typeof(Blaze.Common.GameSettings).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ETModel.Define.GameSettings = @GameSettings;
            return ptr_of_this_method;
        }

        static object get_AssetBundleVersion_2(ref object o)
        {
            return ETModel.Define.AssetBundleVersion;
        }

        static StackObject* CopyToStack_AssetBundleVersion_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ETModel.Define.AssetBundleVersion;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_AssetBundleVersion_2(ref object o, object v)
        {
            ETModel.Define.AssetBundleVersion = (System.String)v;
        }

        static StackObject* AssignFromStack_AssetBundleVersion_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.String @AssetBundleVersion = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ETModel.Define.AssetBundleVersion = @AssetBundleVersion;
            return ptr_of_this_method;
        }



    }
}
