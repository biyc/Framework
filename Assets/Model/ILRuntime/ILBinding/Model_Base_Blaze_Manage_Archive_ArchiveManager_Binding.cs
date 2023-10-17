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
