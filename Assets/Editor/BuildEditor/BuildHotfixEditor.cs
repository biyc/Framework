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


        // [MenuItem("Tools/修改csproj")]
        // private static void OnGeneratedCSProjectFiles()
        // {
        //     var dir = Directory.GetCurrentDirectory();
        //
        //     var files = Directory.GetFiles(dir, "*.csproj");
        //
        //     foreach (var file in files)
        //
        //     {
        //         ChangeVersionOf(file);
        //     }
        // }
        //
        // static void ChangeVersionOf(string file)
        // {
        //     var text = File.ReadAllText(file);
        //
        //     var find = @"TargetFrameworkVersion>[^<]+</TargetFrameworkVersion";
        //
        //     var replace = "TargetFrameworkVersion>v4.6</TargetFrameworkVersion";
        //
        //     if (Regex.Match(text, find, RegexOptions.IgnoreCase).Success)
        //     {
        //         text = Regex.Replace(text, find, replace);
        //     }
        //
        //     // /Users/alyr/AlyrWorkSpace/youloft/LovinHouseProject/LovinHouse2/Assets/Editor/BuildEditor/CsprojTmp.txt
        //     if (!text.Contains("Publish|AnyCPU"))
        //     {
        //         var temp = File.ReadAllText("./Assets/Editor/BuildEditor/CsprojTmp.txt");
        //         var lastIndex =
        //             text.LastIndexOf(
        //                 "<PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' \">");
        //         text = text.Insert(lastIndex, temp);
        //         // text = text.("</Project>", temp);
        //     }
        //
        //     File.WriteAllText(file, text);
        // }
    }
}