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
    unsafe class Blaze_Resource_AssetBundles_AssetProviderBundle_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Blaze.Resource.AssetBundles.AssetProviderBundle);
            args = new Type[]{};
            method = type.GetMethod("DevCleanAsset", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DevCleanAsset_0);
            args = new Type[]{};
            method = type.GetMethod("DevAssetShow", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DevAssetShow_1);


        }


        static StackObject* DevCleanAsset_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Blaze.Resource.AssetBundles.AssetProviderBundle instance_of_this_method = (Blaze.Resource.AssetBundles.AssetProviderBundle)typeof(Blaze.Resource.AssetBundles.AssetProviderBundle).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.DevCleanAsset();

            return __ret;
        }

        static StackObject* DevAssetShow_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Blaze.Resource.AssetBundles.AssetProviderBundle instance_of_this_method = (Blaze.Resource.AssetBundles.AssetProviderBundle)typeof(Blaze.Resource.AssetBundles.AssetProviderBundle).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.DevAssetShow();

            return __ret;
        }



    }
}
