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
    unsafe class Model_Base_Blaze_Manage_Archive_ArchiveManager_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(Model.Base.Blaze.Manage.Archive.ArchiveManager);
            args = new Type[]{};
            method = type.GetMethod("GetArchive", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetArchive_0);
            args = new Type[]{typeof(Model.Base.Blaze.Manage.Archive.ArchiveData), typeof(System.Action<System.Boolean>)};
            method = type.GetMethod("PushSlot", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, PushSlot_1);

            field = type.GetField("Archive", flag);
            app.RegisterCLRFieldGetter(field, get_Archive_0);
            app.RegisterCLRFieldSetter(field, set_Archive_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_Archive_0, AssignFromStack_Archive_0);


        }


        static StackObject* GetArchive_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Model.Base.Blaze.Manage.Archive.ArchiveManager instance_of_this_method = (Model.Base.Blaze.Manage.Archive.ArchiveManager)typeof(Model.Base.Blaze.Manage.Archive.ArchiveManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetArchive();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* PushSlot_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Boolean> @pushCb = (System.Action<System.Boolean>)typeof(System.Action<System.Boolean>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            Model.Base.Blaze.Manage.Archive.ArchiveData @archiveData = (Model.Base.Blaze.Manage.Archive.ArchiveData)typeof(Model.Base.Blaze.Manage.Archive.ArchiveData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            Model.Base.Blaze.Manage.Archive.ArchiveManager instance_of_this_method = (Model.Base.Blaze.Manage.Archive.ArchiveManager)typeof(Model.Base.Blaze.Manage.Archive.ArchiveManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.PushSlot(@archiveData, @pushCb);

            return __ret;
        }


        static object get_Archive_0(ref object o)
        {
            return ((Model.Base.Blaze.Manage.Archive.ArchiveManager)o).Archive;
        }

        static StackObject* CopyToStack_Archive_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((Model.Base.Blaze.Manage.Archive.ArchiveManager)o).Archive;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Archive_0(ref object o, object v)
        {
            ((Model.Base.Blaze.Manage.Archive.ArchiveManager)o).Archive = (Model.Base.Blaze.Manage.Archive.ArchiveData)v;
        }

        static StackObject* AssignFromStack_Archive_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            Model.Base.Blaze.Manage.Archive.ArchiveData @Archive = (Model.Base.Blaze.Manage.Archive.ArchiveData)typeof(Model.Base.Blaze.Manage.Archive.ArchiveData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((Model.Base.Blaze.Manage.Archive.ArchiveManager)o).Archive = @Archive;
            return ptr_of_this_method;
        }



    }
}
