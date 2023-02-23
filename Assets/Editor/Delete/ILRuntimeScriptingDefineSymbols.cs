// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
//
// public class ILRuntimeScriptingDefineSymbols
// {
//     const string ILRuntimePath = "Tools/ScriptingDefineSymbols/USE_ILRuntime";
//
//     [MenuItem(ILRuntimePath, true)]
//     static bool ILRuntimeCheck()
//     {
//         Menu.SetChecked(ILRuntimePath, PlayerSettings
//             .GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup)
//             .Contains("ILRuntime"));
//         return true;
//     }
//
//     [MenuItem(ILRuntimePath)]
//     static void ILRuntimeSet()
//     {
//         var groups = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
//         if (groups.Contains("ILRuntime"))
//             groups = groups.Replace("ILRuntime", "");
//         else
//             groups += ";ILRuntime";
//
//         PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, groups);
//     }
// }