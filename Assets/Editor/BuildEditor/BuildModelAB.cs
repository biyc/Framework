using System.IO;
using Blaze.Bundle.PrefabBundle;
using Blaze.Ci;
using Blaze.Common;
using Blaze.Resource.Common;
using Blaze.Utility.Helper;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Editor.BuildEditor
{
    public class BuildModelAB : OdinEditorWindow

    {
        // [MenuItem("Tools/tt")]
        // public static void Tt()
        // {
        //     var str = "yzll";
        //     var ds = CryptoHelper.XxteaEncryptToString(str);
        //     Debug.Log(ds);
        //     Debug.Log(CryptoHelper.XxteaDecryptByString(ds));
        // }


        /// <summary>
        /// 包的类型
        /// </summary>
        [ShowInInspector] public static EnumPackageType PackageType;

        /// <summary>
        /// 构建目标平台
        /// </summary>
        [ShowInInspector] public static EnumRuntimeTarget TargetPlatform = EnumRuntimeTarget.Android;

        /// <summary>
        /// 模型名字
        /// </summary>
        [ShowInInspector, ReadOnly] private static string _modelName;


        private static BuildModelAB _window;

        [MenuItem("Assets/BuildModelAB", false, 3)]
        public static void BuildSinglePrefabAB()
        {
            var obj = Selection.objects;
            if (obj.Length == 0) return;
            if (obj.Length > 1)
            {
                EditorUtility.DisplayDialog("文件夹数量超出", "请选择一个预制体文件夹", "yes", "no");
                return;
            }

            var path = AssetDatabase.GetAssetPath(obj[0]);
            if (!Directory.Exists(path) || !path.StartsWith("Assets/Projects/Models") || path.Split('/').Length != 4)
            {
                EditorUtility.DisplayDialog("", "请选择有效文件夹", "yes", "no");
                return;
            }

            _modelName = obj[0].name;
            _window = GetWindow<BuildModelAB>();
            _window.Show();
        }

        /// <summary>
        /// 打ab包
        /// </summary>
        [Button("BuildBundle")]
        public static void Build()
        {
            ModelBundleStep1._.Execution(PackageType, TargetPlatform, _modelName);
            ModelBundleStep2._.Execution();
            ModelBundleStep3._.Execution();
            _window.Close();
        }
    }
}