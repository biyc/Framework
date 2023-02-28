using Blaze.Utility;
using Blaze.Utility.Helper;
using Model.Base.Blaze.Manage.Archive;
using UnityEditor;
using UnityEngine;

public class Keyboard
{
    [MenuItem("快捷键/打开字体目录 #t")]
    private static void OpenFont()
    {
        Selection.objects = null;
        var obj = AssetDatabase.LoadAssetAtPath<Object>("Assets/Projects/Font/textMeshPro.prefab");
        EditorGUIUtility.PingObject(obj);
    } 

    [MenuItem("快捷键/打开存档目录 #p")]
    private static void OpenPersistentDic()
    {
        EditorUtility.OpenWithDefaultApp(Application.persistentDataPath);
    }

    [MenuItem("GameObject/UI/pre - TextMeshPro", false, 10)]
    public static void CreateTMP()
    {
        GameObject go = PrefabUtility.InstantiatePrefab(
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Projects/Font/textMeshPro.prefab"),
            Selection.activeGameObject.transform) as GameObject;
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeGameObject = go;
    }
    
}