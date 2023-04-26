using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using System;
using System.Collections.Generic;
using System.Reflection;
using ILRuntime.Runtime.Generated;
using UnityEngine;

namespace ETModel
{
    public static class ILHelper
    {
        public static unsafe void InitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
        {
            // 注册重定向函数
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.RectTransform>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.UI.Button>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.String, System.Int32>();
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Converter<System.String, System.Int32>>((act) =>
            {
                return new System.Converter<System.String, System.Int32>((input) =>
                {
                    return ((Func<System.String, System.Int32>) act)(input);
                });
            });
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Threading.CancellationToken, System.Collections.IEnumerator>();

            appdomain.DelegateManager.RegisterFunctionDelegate<System.Int32, System.String>();
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Converter<System.Int32, System.String>>((act) =>
            {
                return new System.Converter<System.Int32, System.String>((input) =>
                {
                    return ((Func<System.Int32, System.String>) act)(input);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.UI.Image>();


            appdomain.DelegateManager
                .RegisterDelegateConvertor<
                    System.Converter<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.String>>((act) =>
                {
                    return new System.Converter<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.String>(
                        (input) =>
                        {
                            return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.String>) act)(input);
                        });
                });
            
            appdomain.DelegateManager
                .RegisterDelegateConvertor<
                    System.Converter<System.String, ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
                {
                    return new System.Converter<System.String, ILRuntime.Runtime.Intepreter.ILTypeInstance>(
                        (input) =>
                        {
                            return ((Func<System.String, ILRuntime.Runtime.Intepreter.ILTypeInstance>) act)(input);
                        });
                });
            
            appdomain.DelegateManager.RegisterMethodDelegate<Blaze.Resource.Common.PrefabObject>();
            appdomain.DelegateManager
                .RegisterMethodDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>();
            appdomain.DelegateManager
                .RegisterMethodDelegate<System.String, System.String, System.Action, UnityEngine.Transform>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Boolean, System.String>();
            appdomain.DelegateManager.RegisterFunctionDelegate<UniRx.Unit, System.Boolean>();
            // appdomain.DelegateManager.RegisterDelegateConvertor<global::NativeGallery.MediaSaveCallback>((act) =>
            // {
            //     return new global::NativeGallery.MediaSaveCallback((success, path) =>
            //     {
            //         ((Action<System.Boolean, System.String>)act)(success, path);
            //     });
            // });
            appdomain.DelegateManager
                .RegisterFunctionDelegate<
                    System.Linq.IGrouping<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>,
                    System.Linq.IGrouping<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>, System.Int32>();
            appdomain.DelegateManager
                .RegisterDelegateConvertor<
                    System.Comparison<
                        System.Linq.IGrouping<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>>>((act) =>
                {
                    return new
                        System.Comparison<System.Linq.IGrouping<System.Int32,
                            ILRuntime.Runtime.Intepreter.ILTypeInstance>>((x, y) =>
                        {
                            return ((Func<
                                System.Linq.IGrouping<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>
                                , System.Linq.IGrouping<System.Int32,
                                    ILRuntime.Runtime.Intepreter.ILTypeInstance>, System.Int32>) act)(x, y);
                        });
                });
            appdomain.DelegateManager.RegisterMethodDelegate<System.Linq.IGrouping<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>>();


            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.UI.Graphic>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.EventSystems.BaseEventData>();
            appdomain.DelegateManager
                .RegisterDelegateConvertor<UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData>>(
                    (act) =>
                    {
                        return new UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData>((arg0) =>
                        {
                            ((Action<UnityEngine.EventSystems.BaseEventData>) act)(arg0);
                        });
                    });
            appdomain.DelegateManager
                .RegisterFunctionDelegate<UnityEngine.Vector2, UnityEngine.Camera, System.Boolean>();

            // 适配Res 资源加载系统
            appdomain.DelegateManager.RegisterMethodDelegate<Blaze.Resource.Asset.IUAsset>();
            appdomain.DelegateManager.RegisterMethodDelegate<Sprite>();
            appdomain.DelegateManager.RegisterMethodDelegate<Texture>();
            appdomain.DelegateManager.RegisterMethodDelegate<Shader>();
            appdomain.DelegateManager.RegisterMethodDelegate<AudioClip>();
            appdomain.DelegateManager.RegisterMethodDelegate<Blaze.Resource.AssetBundles.Asset.UAsset>();
            appdomain.DelegateManager.RegisterMethodDelegate<Blaze.Resource.AssetBundles.Bundle.UniBundle>();
            appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Transform, System.Boolean>();
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<UnityEngine.Transform>>((act) =>
            {
                return new System.Predicate<UnityEngine.Transform>((obj) =>
                {
                    return ((Func<UnityEngine.Transform, System.Boolean>) act)(obj);
                });
            });
            appdomain.DelegateManager
                .RegisterFunctionDelegate<UnityEngine.EventSystems.PointerEventData,
                    ILRuntime.Runtime.Intepreter.ILTypeInstance>();
            appdomain.DelegateManager
                .RegisterFunctionDelegate<UnityEngine.EventSystems.PointerEventData, System.Boolean>();
            appdomain.DelegateManager.RegisterMethodDelegate<UniRx.Unit>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.EventSystems.PointerEventData[]>();
            appdomain.DelegateManager
                .RegisterFunctionDelegate<System.Int32, ILRuntime.Runtime.Intepreter.ILTypeInstance>();


            appdomain.DelegateManager
                .RegisterFunctionDelegate<System.String, ILRuntime.Runtime.Intepreter.ILTypeInstance>();

            appdomain.DelegateManager
                .RegisterDelegateConvertor<
                    System.Converter<ILRuntime.Runtime.Intepreter.ILTypeInstance,
                        ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
                {
                    return new
                        System.Converter<ILRuntime.Runtime.Intepreter.ILTypeInstance,
                            ILRuntime.Runtime.Intepreter.ILTypeInstance>((input) =>
                        {
                            return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance,
                                ILRuntime.Runtime.Intepreter.ILTypeInstance>) act)(input);
                        });
                });
            appdomain.DelegateManager.RegisterMethodDelegate<System.Action>();

            // 注册委托
            appdomain.DelegateManager.RegisterMethodDelegate<List<object>>();
            appdomain.DelegateManager.RegisterMethodDelegate<byte[], int, int>();
            appdomain.DelegateManager.RegisterMethodDelegate<ILTypeInstance>();


            appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Vector2, System.Boolean>();

            // appDomain.DelegateManager.RegisterFunctionDelegate<Google.Protobuf.Adapt_IMessage.Adaptor>();
            // appDomain.DelegateManager.RegisterMethodDelegate<Google.Protobuf.Adapt_IMessage.Adaptor>();

            appdomain.DelegateManager.RegisterFunctionDelegate<System.Boolean>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Int32, System.Boolean>();
            appdomain.DelegateManager.RegisterFunctionDelegate<ILTypeInstance, System.Int64>();

     
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<System.Int32>>((act) =>
            {
                return new System.Predicate<System.Int32>((obj) =>
                {
                    return ((Func<System.Int32, System.Boolean>) act)(obj);
                });
            });
            
            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((act) =>
            {
                return new UnityEngine.Events.UnityAction(() =>
                {
                    //((Action<>)act)();
                    ((System.Action) act)();
                });
            });
            appdomain.DelegateManager.RegisterFunctionDelegate<System.IO.FileInfo, System.IO.FileInfo, System.Int32>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.String>();
            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.String>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<System.String>((arg0) =>
                {
                    ((Action<System.String>) act)(arg0);
                });
            });


            appdomain.DelegateManager.RegisterMethodDelegate<System.Boolean>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Int32>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Int32>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Int32, System.Int32, System.Int32>();
            appdomain.DelegateManager
                .RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Boolean>();
            appdomain.DelegateManager
                .RegisterDelegateConvertor<System.Predicate<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
                {
                    return new System.Predicate<ILRuntime.Runtime.Intepreter.ILTypeInstance>((obj) =>
                    {
                        return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Boolean>) act)(obj);
                    });
                });
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<System.Int64>>((act) =>
            {
                return new System.Predicate<System.Int64>((obj) =>
                {
                    return ((Func<System.Int64, System.Boolean>) act)(obj);
                });
            });
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Int64, System.Boolean>();

            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Video.VideoPlayer>();
            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Video.VideoPlayer.EventHandler>((act) =>
            {
                return new UnityEngine.Video.VideoPlayer.EventHandler((source) =>
                {
                    ((Action<UnityEngine.Video.VideoPlayer>) act)(source);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Vector2>();
            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<UnityEngine.Vector2>>(
                (act) =>
                {
                    return new UnityEngine.Events.UnityAction<UnityEngine.Vector2>((arg0) =>
                    {
                        ((Action<UnityEngine.Vector2>) act)(arg0);
                    });
                });
            appdomain.DelegateManager.RegisterMethodDelegate<System.Single>();


            appdomain.DelegateManager
                .RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance,
                    ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>();
            appdomain.DelegateManager
                .RegisterDelegateConvertor<System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
                {
                    return new System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>((x, y) =>
                    {
                        return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance,
                            ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>) act)(x, y);
                    });
                });

            appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<System.IO.FileInfo>>((act) =>
            {
                return new System.Comparison<System.IO.FileInfo>((x, y) =>
                {
                    return ((Func<System.IO.FileInfo, System.IO.FileInfo, System.Int32>) act)(x, y);
                });
            });


            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.Boolean>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<System.Boolean>((arg0) =>
                {
                    ((Action<System.Boolean>) act)(arg0);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<System.Int64>>((act) =>
            {
                return new System.Predicate<System.Int64>((obj) =>
                {
                    return ((Func<System.Int64, System.Boolean>) act)(obj);
                });
            });

            appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<System.Int32>>((act) =>
            {
                return new System.Comparison<System.Int32>((x, y) =>
                {
                    return ((Func<System.Int32, System.Int32, System.Int32>) act)(x, y);
                });
            });


            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.Int32>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<System.Int32>((arg0) =>
                {
                    ((Action<System.Int32>) act)(arg0);
                });
            });

            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.Single>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<System.Single>((arg0) =>
                {
                    ((Action<System.Single>) act)(arg0);
                });
            });
            appdomain.DelegateManager
                .RegisterDelegateConvertor<System.Converter<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int64>>(
                    (act) =>
                    {
                        return new System.Converter<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int64>(
                            (input) =>
                            {
                                return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int64>) act)(input);
                            });
                    });
            appdomain.DelegateManager.RegisterMethodDelegate<System.Int64>();
            appdomain.DelegateManager.RegisterMethodDelegate<ETModel.AEvent>();

            appdomain.DelegateManager.RegisterMethodDelegate<System.String, System.String>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.UI.Text>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.EventSystems.PointerEventData>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Texture2D>();

            appdomain.DelegateManager
                .RegisterFunctionDelegate<UnityEngine.GameObject, UnityEngine.Vector3, UnityEngine.Vector3>();
            appdomain.DelegateManager.RegisterMethodDelegate<global::DragParameterObject>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Texture2D>();


            appdomain.DelegateManager.RegisterMethodDelegate<System.Int32, System.String>();
            appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Vector2>();
            
            appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Vector3>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Object>();
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Threading.SendOrPostCallback>((act) =>
            {
                return new System.Threading.SendOrPostCallback((state) =>
                {
                    ((Action<System.Object>) act)(state);
                });
            });
            appdomain.DelegateManager
                .RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.String>();
            appdomain.DelegateManager
                .RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance,
                    ILRuntime.Runtime.Intepreter.ILTypeInstance>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.String, System.Boolean>();
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<System.String>>((act) =>
            {
                return new System.Predicate<System.String>((obj) =>
                {
                    return ((Func<System.String, System.Boolean>) act)(obj);
                });
            });
            appdomain.DelegateManager
                .RegisterFunctionDelegate<UnityEngine.GameObject, UnityEngine.Vector3, System.Boolean>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Int32, UnityEngine.GameObject>();
            
            appdomain.DelegateManager
                .RegisterMethodDelegate<System.Collections.Generic.Dictionary<System.String, System.String>>();


            // 图库适配
            appdomain.DelegateManager
                .RegisterMethodDelegate<System.Threading.Tasks.Task<ILRuntime.Runtime.Intepreter.ILTypeInstance>>();

            appdomain.DelegateManager
                .RegisterMethodDelegate<System.Collections.Generic.KeyValuePair<UnityEngine.GameObject,
                    ILRuntime.Runtime.Intepreter.ILTypeInstance>>();
            appdomain.DelegateManager
                .RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance,
                    System.Collections.Generic.List<ILRuntime.Runtime.Intepreter.ILTypeInstance>,
                    ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance>();
            appdomain.DelegateManager
                .RegisterMethodDelegate<System.Collections.Generic.KeyValuePair<System.String, System.String>>();
            appdomain.DelegateManager
                .RegisterMethodDelegate<System.Collections.Generic.KeyValuePair<System.String, System.Int32>>();
            appdomain.DelegateManager
                .RegisterMethodDelegate<System.Collections.Generic.KeyValuePair<System.Int32, System.Int32>>();
            appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Transform, UnityEngine.RectTransform>();
            appdomain.DelegateManager
                .RegisterDelegateConvertor<System.Converter<UnityEngine.Transform, UnityEngine.RectTransform>>((act) =>
                {
                    return new System.Converter<UnityEngine.Transform, UnityEngine.RectTransform>((input) =>
                    {
                        return ((Func<UnityEngine.Transform, UnityEngine.RectTransform>) act)(input);
                    });
                });
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Transform>();

            appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.UI.Text, System.Boolean>();
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<UnityEngine.UI.Text>>((act) =>
            {
                return new System.Predicate<UnityEngine.UI.Text>((obj) =>
                {
                    return ((Func<UnityEngine.UI.Text, System.Boolean>) act)(obj);
                });
            });

            

            appdomain.DelegateManager
                .RegisterMethodDelegate<System.Collections.Generic.KeyValuePair<System.String,
                    ILRuntime.Runtime.Intepreter.ILTypeInstance>>();

            appdomain.DelegateManager.RegisterMethodDelegate<System.IDisposable>();
            appdomain.DelegateManager
                .RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>();

            appdomain.DelegateManager
                .RegisterMethodDelegate<System.Collections.Generic.KeyValuePair<System.Int32, System.String>>();
            appdomain.DelegateManager
                .RegisterDelegateConvertor<System.Converter<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>>(
                    (act) =>
                    {
                        return new System.Converter<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>(
                            (input) =>
                            {
                                return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>) act)(input);
                            });
                    });


            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Material>();

            appdomain.DelegateManager
                .RegisterMethodDelegate<System.Collections.Generic.KeyValuePair<System.Int32, System.Int64>>();
            appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance>();


            CLRBindings.Initialize(appdomain);
            // 注册适配器
            Assembly assembly = typeof(Init).Assembly;
            foreach (Type type in assembly.GetTypes())
            {
                object[] attrs = type.GetCustomAttributes(typeof(ILAdapterAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                object obj = Activator.CreateInstance(type);
                CrossBindingAdaptor adaptor = obj as CrossBindingAdaptor;
                if (adaptor == null)
                {
                    continue;
                }

                appdomain.RegisterCrossBindingAdaptor(adaptor);
            }

            LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);
            // ILSerial.RegisterILRuntimeCLRRedirection(appDomain);
        }

        private static object PType_CreateInstance(ILRuntime.Runtime.Enviorment.AppDomain appDomain, string typeName)
        {
            return appDomain.Instantiate(typeName);
        }
    }
}