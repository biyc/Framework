using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class MiniTolls
{
    /// <summary>
    /// 快捷键隐藏或者显示层级面板中的物体
    /// </summary>
    [MenuItem("GameObject/MiniTools/SetActive &q", false, 1000)]
    public static void Show()
    {
        var obj = Selection.gameObjects;
        foreach (var v in obj)
        {
            v.gameObject.SetActive(!v.activeSelf);
            EditorUtility.SetDirty(v);
        }
    }

    /// <summary>
    /// 删除物体
    /// </summary>
    [MenuItem("GameObject/MiniTools/Delete &d", false, 1000)]
    public static void Delete()
    {
        var obj = Selection.gameObjects;
        foreach (var v in obj)
            GameObject.DestroyImmediate(v);
    }


    /// <summary>
    /// 对层级面板中同层的物体进行顺序命名
    /// </summary>
    /// <param name="command"></param>
    [MenuItem("GameObject/MiniTools/RenameSort  &s", false, 1000)]
    public static void RenameSort(MenuCommand command)
    {
        var obj = Selection.transforms.ToList();
        obj.Sort((x, y) => x.GetSiblingIndex().CompareTo(y.GetSiblingIndex()));
        var name = obj[0].name;
        for (int i = 1; i < obj.Count; i++)
        {
            obj[i].name = name + i;
        }
    }

    /// <summary>
    /// 解锁全部章节，方便测试
    /// </summary>
    [MenuItem("Tools/Biyc/UnlockChapter")]
    public static void UnlockChapter()
    {
        var path = Application.dataPath.Replace("Assets", "") + "Data/CsvData/Hotfix.Game.Common$DefaultConfig.csv";
        var lines = File.ReadAllLines(path);
        for (int i = 0; i < lines.Length-1; i++)
        {
            var v = lines[i];
            if (v.StartsWith("8032"))
            {
                var s = v.Remove(v.Length - 2, 1);
                s = s.Insert(v.Length - 2, "40");
                lines[i] = s;
            }
        }
        File.WriteAllLines(path,lines);
    }
}