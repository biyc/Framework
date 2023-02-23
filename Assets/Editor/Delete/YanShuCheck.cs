// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
// using System.IO;
// using System.Linq;
// using TMPro;
// using Sirenix.Utilities;
// using Sirenix.Utilities.Editor;
// using UnityEngine.UI;
//
// public class YanShuCheck : EditorWindow
// {
//     static string CsvAssetPath = Application.dataPath.Replace("Assets", "Data/CsvData");
//     static string CsAssetPath = Application.dataPath.Replace("Assets", "Assets/Hotfix");
//     static string checkStr = "";
//     static List<char> unContainsChars = new List<char>();
//
//     [MenuItem("Tools/YanShuCheck")]
//     public static void Check()
//     {
//         CheckString(CsvAssetPath,false);
//         CheckString(CsAssetPath,true);
//         CheckPrefabs();
//         AssetDatabase.Refresh();
//     }
//
//     static void CheckPrefabs()
//     {
//         var paths = AssetDatabase.GetAllAssetPaths();
//         foreach (var path in paths)
//         {
//             if (!path.StartsWith("Assets/Projects")) continue;
//             if (!path.EndsWith(".prefab")) continue;
//             try
//             {
//                 var obj = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
//                 if (obj == null) continue;
//                 var temps = obj.transform.GetComponentsInChildren<TextMeshProUGUI>(true);
//                 temps.ForEach(text =>
//                 {
//                     if (text.text.Contains("晏殊"))
//                     {
//                         var str = text.text.Replace("晏殊", "晏舒");
//                         text.text = str;
//                         EditorUtility.SetDirty(obj);
//                         Debug.LogError(obj.name+" 存在错别字晏殊，已正确修改为晏舒");
//                     }
//                 });
//                 var texts = obj.transform.GetComponentsInChildren<Text>(true);
//                 texts.ForEach(text =>
//                 {
//                     if (text.text.Contains("晏殊"))
//                     {
//                         var str = text.text.Replace("晏殊", "晏舒");
//                         text.text = str;
//                         EditorUtility.SetDirty(obj);
//                         Debug.LogError(obj.name+" 存在错别字晏殊，已正确修改为晏舒");
//                     }
//                 });
//             }
//             catch
//             {
//             }
//         }
//     }
//
//     static void CheckString(string path,bool isScript)
//     {
//         var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
//             .Where(x => x.EndsWith(".csv") || x.EndsWith(".cs")).ToList();
//         foreach (var v in files)
//         {
//             var contents = File.ReadAllText(v);
//             if (contents.Contains("晏殊"))
//             {
//                 if (isScript)
//                 {
//                     contents=contents.Replace("晏殊", "晏舒");
//                     File.WriteAllText(v,contents);
//                 }
//                 else
//                     Debug.LogError(contents);
//             }
//
//         }
//     }
// }