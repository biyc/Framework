using System.IO;
using Blaze.Bundle;
using Blaze.Bundle.PrefabBundle;
using Blaze.Ci;
using Blaze.Common;
using Blaze.Resource.Common;
using Blaze.Utility.Helper;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;

namespace Editor.BuildEditor
{
    public class BuildModelAB : OdinEditorWindow

    {
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
        private static string _modelName;

        /// <summary>
        /// 模型名字
        /// </summary>
        [ShowInInspector, ReadOnly] private static string ModelName;

        /// <summary>
        /// 输出路径
        /// </summary>
        //[ShowInInspector] public static string OutPath;


        /// <summary>
        /// 是否打包所有模型
        /// </summary>
        [ShowInInspector, OnValueChanged("IsBuildAllModelValueChange")]
        public static bool IsBuildAllModel = false;

        /// <summary>
        /// 选择输出的路径
        /// </summary>
        [FolderPath, ShowInInspector] public static string FolderPath;


        private static BuildModelAB _window;

        private static ModelABBuildConfig _abBuildConfig;


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
            ModelName = _modelName;
            IsBuildAllModel = false;
            _abBuildConfig = new ModelABBuildConfig();
            _abBuildConfig.OutPath = ModelABConfig.GetModelConfig().OutPath;
            FolderPath = _abBuildConfig.OutPath;
            _window = GetWindow<BuildModelAB>();
            _window.Show();
        }

        /// <summary>
        /// 打ab包
        /// </summary>
        [Button("BuildBundle")]
        public static void Build()
        {
            if (IsBuildAllModel)
            {
                var dirs = Directory.GetDirectories("Assets/Projects/Models");
                dirs.ForEach(m => BuildModel(new DirectoryInfo(m).Name));
            }
            else
                BuildModel(_modelName);

            _window.Close();
        }

        /// <summary>
        ///打包模型ab包
        /// </summary>
        /// <param name="name"></param>
        private static void BuildModel(string name)
        {
            if (!ModelPreProcess._.Execution(name))
            {
                _window.Close();
                return;
            }

            ModelBundleStep1._.Execution(PackageType, TargetPlatform, name);
            ModelBundleStep2._.Execution();
            ModelBundleStep3._.Execution();
        }

        /// <summary>
        /// 是否打包所有模型资源
        /// </summary>
        private static void IsBuildAllModelValueChange()
        {
            ModelName = IsBuildAllModel ? "AllModel" : _modelName;
        }
    }
}