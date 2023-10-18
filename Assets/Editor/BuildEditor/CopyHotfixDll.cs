using System;
using System.IO;
using System.Text.RegularExpressions;
using ETModel;
using UnityEditor;
using UnityEngine;

namespace ETEditor
{
    [InitializeOnLoad]
    public class Startup
    {
        private const string ScriptAssembliesDir = "Library/ScriptAssemblies";
        private const string HotfixDll = "Unity.Hotfix.dll";
        private const string HotfixPdb = "Unity.Hotfix.pdb";

        static Startup()
        {
            var DataCodeDir = "Data/AppHotfix/";
            var path = Application.dataPath.Replace("Assets", DataCodeDir);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            File.Copy(Path.Combine(ScriptAssembliesDir, HotfixDll), Path.Combine(DataCodeDir, "Hotfix.dll.bytes"),
                true);
            File.Copy(Path.Combine(ScriptAssembliesDir, HotfixPdb), Path.Combine(DataCodeDir, "Hotfix.pdb.bytes"),
                true);
            Log.Info($"复制Hotfix.dll, Hotfix.pdb到Res/Code完成");
        }
    }
}