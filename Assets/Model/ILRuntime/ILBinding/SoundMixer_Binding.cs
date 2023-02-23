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
    unsafe class SoundMixer_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::SoundMixer);

            field = type.GetField("audioMixer", flag);
            app.RegisterCLRFieldGetter(field, get_audioMixer_0);
            app.RegisterCLRFieldSetter(field, set_audioMixer_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_audioMixer_0, AssignFromStack_audioMixer_0);


        }



        static object get_audioMixer_0(ref object o)
        {
            return ((global::SoundMixer)o).audioMixer;
        }

        static StackObject* CopyToStack_audioMixer_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::SoundMixer)o).audioMixer;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_audioMixer_0(ref object o, object v)
        {
            ((global::SoundMixer)o).audioMixer = (UnityEngine.Audio.AudioMixer)v;
        }

        static StackObject* AssignFromStack_audioMixer_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            UnityEngine.Audio.AudioMixer @audioMixer = (UnityEngine.Audio.AudioMixer)typeof(UnityEngine.Audio.AudioMixer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::SoundMixer)o).audioMixer = @audioMixer;
            return ptr_of_this_method;
        }



    }
}
