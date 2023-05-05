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
}