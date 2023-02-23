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
    unsafe class ETModel_Game_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.Game);
            args = new Type[]{};
            method = type.GetMethod("get_EventSystem", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_EventSystem_0);
            args = new Type[]{};
            method = type.GetMethod("get_Scene", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_Scene_1);

            field = type.GetField("Hotfix", flag);
            app.RegisterCLRFieldGetter(field, get_Hotfix_0);
            app.RegisterCLRFieldSetter(field, set_Hotfix_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_Hotfix_0, AssignFromStack_Hotfix_0);


        }


        static StackObject* get_EventSystem_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = ETModel.Game.EventSystem;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_Scene_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = ETModel.Game.Scene;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_Hotfix_0(ref object o)
        {
            return ETModel.Game.Hotfix;
        }

        static StackObject* CopyToStack_Hotfix_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ETModel.Game.Hotfix;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Hotfix_0(ref object o, object v)
        {
            ETModel.Game.Hotfix = (ETModel.Hotfix)v;
        }

        static StackObject* AssignFromStack_Hotfix_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            ETModel.Hotfix @Hotfix = (ETModel.Hotfix)typeof(ETModel.Hotfix).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ETModel.Game.Hotfix = @Hotfix;
            return ptr_of_this_method;
        }



    }
}
