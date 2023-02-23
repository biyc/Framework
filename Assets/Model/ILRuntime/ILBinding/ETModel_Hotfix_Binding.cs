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
    unsafe class ETModel_Hotfix_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.Hotfix);
            args = new Type[]{};
            method = type.GetMethod("GetHotfixTypes", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetHotfixTypes_0);

            field = type.GetField("Update", flag);
            app.RegisterCLRFieldGetter(field, get_Update_0);
            app.RegisterCLRFieldSetter(field, set_Update_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_Update_0, AssignFromStack_Update_0);
            field = type.GetField("OnGUI", flag);
            app.RegisterCLRFieldGetter(field, get_OnGUI_1);
            app.RegisterCLRFieldSetter(field, set_OnGUI_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnGUI_1, AssignFromStack_OnGUI_1);
            field = type.GetField("LateUpdate", flag);
            app.RegisterCLRFieldGetter(field, get_LateUpdate_2);
            app.RegisterCLRFieldSetter(field, set_LateUpdate_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_LateUpdate_2, AssignFromStack_LateUpdate_2);
            field = type.GetField("OnApplicationQuit", flag);
            app.RegisterCLRFieldGetter(field, get_OnApplicationQuit_3);
            app.RegisterCLRFieldSetter(field, set_OnApplicationQuit_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnApplicationQuit_3, AssignFromStack_OnApplicationQuit_3);
            field = type.GetField("OnApplicationFocus", flag);
            app.RegisterCLRFieldGetter(field, get_OnApplicationFocus_4);
            app.RegisterCLRFieldSetter(field, set_OnApplicationFocus_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnApplicationFocus_4, AssignFromStack_OnApplicationFocus_4);
            field = type.GetField("OnApplicationPause", flag);
            app.RegisterCLRFieldGetter(field, get_OnApplicationPause_5);
            app.RegisterCLRFieldSetter(field, set_OnApplicationPause_5);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnApplicationPause_5, AssignFromStack_OnApplicationPause_5);
            field = type.GetField("OnFocusChanged", flag);
            app.RegisterCLRFieldGetter(field, get_OnFocusChanged_6);
            app.RegisterCLRFieldSetter(field, set_OnFocusChanged_6);
            app.RegisterCLRFieldBinding(field, CopyToStack_OnFocusChanged_6, AssignFromStack_OnFocusChanged_6);


        }


        static StackObject* GetHotfixTypes_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ETModel.Hotfix instance_of_this_method = (ETModel.Hotfix)typeof(ETModel.Hotfix).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetHotfixTypes();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_Update_0(ref object o)
        {
            return ((ETModel.Hotfix)o).Update;
        }

        static StackObject* CopyToStack_Update_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((ETModel.Hotfix)o).Update;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Update_0(ref object o, object v)
        {
            ((ETModel.Hotfix)o).Update = (System.Action)v;
        }

        static StackObject* AssignFromStack_Update_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @Update = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((ETModel.Hotfix)o).Update = @Update;
            return ptr_of_this_method;
        }

        static object get_OnGUI_1(ref object o)
        {
            return ((ETModel.Hotfix)o).OnGUI;
        }

        static StackObject* CopyToStack_OnGUI_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((ETModel.Hotfix)o).OnGUI;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnGUI_1(ref object o, object v)
        {
            ((ETModel.Hotfix)o).OnGUI = (System.Action)v;
        }

        static StackObject* AssignFromStack_OnGUI_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @OnGUI = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((ETModel.Hotfix)o).OnGUI = @OnGUI;
            return ptr_of_this_method;
        }

        static object get_LateUpdate_2(ref object o)
        {
            return ((ETModel.Hotfix)o).LateUpdate;
        }

        static StackObject* CopyToStack_LateUpdate_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((ETModel.Hotfix)o).LateUpdate;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_LateUpdate_2(ref object o, object v)
        {
            ((ETModel.Hotfix)o).LateUpdate = (System.Action)v;
        }

        static StackObject* AssignFromStack_LateUpdate_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @LateUpdate = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((ETModel.Hotfix)o).LateUpdate = @LateUpdate;
            return ptr_of_this_method;
        }

        static object get_OnApplicationQuit_3(ref object o)
        {
            return ((ETModel.Hotfix)o).OnApplicationQuit;
        }

        static StackObject* CopyToStack_OnApplicationQuit_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((ETModel.Hotfix)o).OnApplicationQuit;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnApplicationQuit_3(ref object o, object v)
        {
            ((ETModel.Hotfix)o).OnApplicationQuit = (System.Action)v;
        }

        static StackObject* AssignFromStack_OnApplicationQuit_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @OnApplicationQuit = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((ETModel.Hotfix)o).OnApplicationQuit = @OnApplicationQuit;
            return ptr_of_this_method;
        }

        static object get_OnApplicationFocus_4(ref object o)
        {
            return ((ETModel.Hotfix)o).OnApplicationFocus;
        }

        static StackObject* CopyToStack_OnApplicationFocus_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((ETModel.Hotfix)o).OnApplicationFocus;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnApplicationFocus_4(ref object o, object v)
        {
            ((ETModel.Hotfix)o).OnApplicationFocus = (System.Action<System.Boolean>)v;
        }

        static StackObject* AssignFromStack_OnApplicationFocus_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<System.Boolean> @OnApplicationFocus = (System.Action<System.Boolean>)typeof(System.Action<System.Boolean>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((ETModel.Hotfix)o).OnApplicationFocus = @OnApplicationFocus;
            return ptr_of_this_method;
        }

        static object get_OnApplicationPause_5(ref object o)
        {
            return ((ETModel.Hotfix)o).OnApplicationPause;
        }

        static StackObject* CopyToStack_OnApplicationPause_5(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((ETModel.Hotfix)o).OnApplicationPause;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnApplicationPause_5(ref object o, object v)
        {
            ((ETModel.Hotfix)o).OnApplicationPause = (System.Action<System.Boolean>)v;
        }

        static StackObject* AssignFromStack_OnApplicationPause_5(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<System.Boolean> @OnApplicationPause = (System.Action<System.Boolean>)typeof(System.Action<System.Boolean>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((ETModel.Hotfix)o).OnApplicationPause = @OnApplicationPause;
            return ptr_of_this_method;
        }

        static object get_OnFocusChanged_6(ref object o)
        {
            return ((ETModel.Hotfix)o).OnFocusChanged;
        }

        static StackObject* CopyToStack_OnFocusChanged_6(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((ETModel.Hotfix)o).OnFocusChanged;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_OnFocusChanged_6(ref object o, object v)
        {
            ((ETModel.Hotfix)o).OnFocusChanged = (System.Action<System.Boolean>)v;
        }

        static StackObject* AssignFromStack_OnFocusChanged_6(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<System.Boolean> @OnFocusChanged = (System.Action<System.Boolean>)typeof(System.Action<System.Boolean>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((ETModel.Hotfix)o).OnFocusChanged = @OnFocusChanged;
            return ptr_of_this_method;
        }



    }
}
