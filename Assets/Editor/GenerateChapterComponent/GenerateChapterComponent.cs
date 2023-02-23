using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.IO;

public class GenerateChapterComponent : EditorWindow
{
    private const string ComponentTemplatePath = "Assets/Editor/GenerateChapterComponent/ChapterComponentTemplate.txt";

    private const string ComponentScrollTemplatePath =
        "Assets/Editor/GenerateChapterComponent/ChapterScrollComponentTemplate.txt";

    private const string ChapterScrollComponentSimpleTiaoManTemplatePath =
        "Assets/Editor/GenerateChapterComponent/ChapterScrollComponentSimpleTiaoManTemplate.txt";

    private const string ScrollPrefabPath = "Assets/Projects/UI/Chapter/Core/ChapterScroll.prefab";


    [MenuItem("Assets/GenerateComponent", false, 2)]
    public static void GenerateScriptsRight()
    {
        var obj = Selection.gameObjects;
        if (obj.Length != 1)
        {
            EditorUtility.DisplayDialog("物体选择错误", "不是有效的物体！,请选择单个章节页的预制体", "关闭");
            return;
        }

        _chapterPrefab = obj[0];
        GenerateScripts();
    }

    [MenuItem("QuickTool/GenerateComponent")]
    public static void GenerateScripts()
    {
        var window = GetWindow<GenerateChapterComponent>();
        window.Show();
    }

    string _path;
    string _chapter;
    string _name;
    string[] options = new string[] {"Bottom", "Medium", "Top", "TopMost"};
    private string[] tiaoType = new string[] {"TiaoManOp", "SimpleTiaoManOp"};
    private int _tiaoIndex;
    int _index;
    static Object _chapterPrefab;
    bool _isScroll;

    private void OnGUI()
    {
        _chapterPrefab = EditorGUILayout.ObjectField("ChapterPrefab", _chapterPrefab, typeof(UnityEngine.Object), true);
        _index = EditorGUILayout.Popup(_index, options);
        _isScroll = EditorGUILayout.Toggle("是否滑动组件", _isScroll);
        if (_isScroll)
            _tiaoIndex = EditorGUILayout.Popup(_tiaoIndex, tiaoType);
        if (_chapterPrefab != null && GUILayout.Button("Ok"))
        {
            ClickOk();
            Close();
        }
    }

    void ClickOk()
    {
        _path = UnityEditor.AssetDatabase.GetAssetPath(_chapterPrefab);
        if (!_path.Contains("Assets/Projects/UI/"))
        {
            Debug.LogError("必须是'Assets/Projects/UI/'路径下的预制体");
            return;
        }

       // AutoGenCode.OpenWindow();
        var v = _path.Split('/');
        _name = v[v.Length - 1].Split('.')[0];

        string templatepath = "";
        if (_isScroll)
        {
            templatepath = _tiaoIndex == 0
                ? ComponentScrollTemplatePath
                : ChapterScrollComponentSimpleTiaoManTemplatePath;
        }
        else
        {
            templatepath = ComponentTemplatePath;
        }

        var templatetxt = UnityEditor.AssetDatabase
            .LoadAssetAtPath<TextAsset>(templatepath).text;
        templatetxt = templatetxt.Replace("#Name", _name);
        templatetxt = templatetxt.Replace("#PrefabPath", _path);
        templatetxt = templatetxt.Replace("#Layer", $"UILayerEnum.{options[_index]}");
        if (_isScroll)
            templatetxt = templatetxt.Replace("#ScrollPrefabPath", ScrollPrefabPath);
        CreateDic(templatetxt);
        AssetDatabase.Refresh();
    }

    void CreateDic(string txt)
    {
        _chapter = _path.Replace("Assets/Projects/UI/", "");
        var index = _chapter.LastIndexOf('/');
        var s = _chapter.Remove(index + 1);
        string dicPath = Application.dataPath.Replace("Assets", $"Assets/Hotfix/UI/Component/{s}");
        string txtPath = dicPath + _name + "Component.cs";
        if (!Directory.Exists(dicPath))
        {
            Directory.CreateDirectory(dicPath);
        }

        if (!File.Exists(txtPath))
        {
            var v = File.Create(txtPath);
            v.Close();
            File.WriteAllText(txtPath, txt);
        }
    }
}