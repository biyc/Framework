using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Blaze.Resource;
using Blaze.Utility.Helper;
using UnityEditor;
using UnityEngine;

namespace ETModel
{
    public sealed class Hotfix : Object
    {
#if ILRuntime
        private ILRuntime.Runtime.Enviorment.AppDomain appDomain;
#endif

        private IStaticMethod start;
        private List<Type> hotfixTypes;

        public Action Update;
        public Action OnGUI;

        public Action LateUpdate;
        public Action OnApplicationQuit;
        public Action<bool> OnApplicationFocus;
        public Action<bool> OnApplicationPause;
        public Action<bool> OnFocusChanged;

        private void Run()
        {
#if ILRuntime
            ILHelper.InitILRuntime(this.appDomain);
#endif
            this.start.Run();
        }

        public List<Type> GetHotfixTypes()
        {
            return this.hotfixTypes;
        }

        public void Dispose()
        {

        }


        public async void LoadHotfixAssembly()
        {
// #if UNITY_EDITOR
//             var assBytes =
//                 File.ReadAllBytes(PathHelper.Combine(PathHelper.GetCurrentPath(), "Data/AppHotfix/Hotfix.dll.bytes"));
//             var pdbBytes =
//                 File.ReadAllBytes(PathHelper.Combine(PathHelper.GetCurrentPath(), "Data/AppHotfix/Hotfix.pdb.bytes"));
//
// #else
            var assBytes = (await Res.LoadAssetAsync("Data/AppHotfix/Hotfix.dll.bytes")).ReadAllBytes();
            var pdbBytes = (await Res.LoadAssetAsync("Data/AppHotfix/Hotfix.pdb.bytes")).ReadAllBytes();
// #endif

#if ILRuntime
            Log.Debug($"当前使用的是ILRuntime模式");
            appDomain = new ILRuntime.Runtime.Enviorment.AppDomain();
            // #if ILRuntimeDebug
            // appDomain.DebugService.StartDebugService(56000);
            // #endif
            if (pdbBytes != null)
                appDomain.LoadAssembly(new MemoryStream(assBytes), new MemoryStream(pdbBytes),
                    new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
            else
                appDomain.LoadAssembly(new MemoryStream(assBytes));

            start = new ILStaticMethod(appDomain, "ETHotfix.Init", "Start", 0);
            hotfixTypes = appDomain.LoadedTypes.Values.Select(x => x.ReflectionType).ToList();
#endif

            // 运行 Hotfix.Start()
            Run();
        }
    }
}