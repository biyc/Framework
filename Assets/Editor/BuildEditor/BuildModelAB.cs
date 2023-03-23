using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        private static List<string> _modelNames;

        /// <summary>
        /// 模型名字
        /// </summary>
        [ShowInInspector, ReadOnly] private static List<string> ModelNames;

        /// <summary>
        /// 输出路径
        /// </summary>
        [ShowInInspector, ValueDropdown("_outPaths")]
        public static string OutPath;

        private static List<string> _outPaths;


        /// <summary>
        /// 是否打包所有模型
        /// </summary>
        [ShowInInspector, OnValueChanged("IsBuildAllModelValueChange")]
        public static bool IsBuildAllModel = false;


        private static BuildModelAB _window;

        private static ModelABBuildConfig _abBuildConfig;


        [MenuItem("Assets/BuildModelAB", false, 3)]
        public static void BuildSinglePrefabAB()
        {
            var obj = Selection.objects;
            if (obj.Length == 0) return;
            _modelNames = new List<string>();

            for (var i = 0; i < obj.Length; i++)
            {
                var path = AssetDatabase.GetAssetPath(obj[i]);
                if (!Directory.Exists(path) || !path.StartsWith("Assets/Projects/Models") ||
                    path.Split('/').Length != 4)
                {
                    EditorUtility.DisplayDialog("", "请选择有效文件夹", "yes", "no");
                    return;
                }
            }

            _modelNames = obj.ToList().ConvertAll(m => m.name);
            ModelNames = _modelNames;
            IsBuildAllModel = false;
            _outPaths = ModelABOutPathConfig.GetModelConfig().OutPaths;
            _window = GetWindow<BuildModelAB>();
            _window.Show();
        }

        /// <summary>
        /// 打ab包
        /// </summary>
        [Button("BuildBundle")]
        public static void Build()
        {
            _abBuildConfig = new ModelABBuildConfig()
            {
                ExtraOutPath = OutPath,
                PackageType = PackageType,
                RuntimeTarget = TargetPlatform
            };

            if (!string.IsNullOrEmpty(OutPath) && !Directory.Exists(OutPath))
            {
                EditorUtility.DisplayDialog("", "该输出路径不存在，请检查！", "Ok");
                return;
            }

            _window.Close();
            if (IsBuildAllModel)
            {
                var dirs = Directory.GetDirectories("Assets/Projects/Models");
                dirs.ForEach(m => BuildModel(new DirectoryInfo(m).Name));
            }
            else
                _modelNames.ForEach(async v => { await BuildModel(v); });
        }

        /// <summary>
        ///打包模型ab包
        /// </summary>
        /// <param name="name"></param>
        private static async Task<bool> BuildModel(string name)
        {
            if (!ModelPreProcess._.Execution(name))
            {
                _window.Close();
                return false;
            }

            _abBuildConfig.Name = name;
            ModelBundleStep1._.Execution(_abBuildConfig);
            ModelBundleStep2._.Execution();
            return await ModelBundleStep3._.Execution();
        }

        /// <summary>
        /// 是否打包所有模型资源
        /// </summary>
        private static void IsBuildAllModelValueChange()
        {
            ModelNames = IsBuildAllModel ? new List<string>() {"AllModel"} : _modelNames;
        }
    }
}