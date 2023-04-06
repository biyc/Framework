using System.IO;
using Blaze.Core;
using Blaze.Utility;
using Blaze.Utility.Helper;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Blaze.Bundle.PrefabBundle
{
    /// <summary>
    /// 模型的提前处理
    /// </summary>
    public class ModelPreProcess : Singeton<ModelPreProcess>
    {
        private const string SHADER_PATH = "Assets/Projects/Shader/ArnoldStandardSurfaceTransparent2.shadergraph";

        public bool Execution(string name)
        {
            var pathDir = $"Assets/Projects/3d/Models/{name}";
            var obj = AssetDatabase.FindAssets("t:Model", new[] {pathDir});
            if (obj.Length != 1)
            {
                EditorUtility.DisplayDialog("模型数量错误", $"该路径下只能存在一个模型请检查！[>>] {pathDir}", "ok");
                return false;
            }

            var path = AssetDatabase.GUIDToAssetPath(obj[0]);
            ModelImporter importer = AssetImporter.GetAtPath(path) as ModelImporter;
            if (importer.materialLocation == ModelImporterMaterialLocation.InPrefab)
            {
                importer.materialLocation = ModelImporterMaterialLocation.External;
                AssetDatabase.ImportAsset(path);
            }

            var mats = AssetDatabase.FindAssets("t:Material", new[] {pathDir});
            if (mats.Length == 0)
            {
                EditorUtility.DisplayDialog("模型材质错误", $"该路径下没有材质请检查！[>>] {pathDir}", "ok");
                return false;
            }

            mats.ForEach(m =>
            {
                var matPath = AssetDatabase.GUIDToAssetPath(m);
                var mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);
                if (mat.shader.name == "Shader Graphs/ArnoldStandardSurfaceTransparent")
                    mat.shader = AssetDatabase.LoadAssetAtPath<Shader>(SHADER_PATH);
            });

            var model = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (File.Exists(path) && model.name != name)
            {
                var newPath = PathHelper.Combine(pathDir, $"{name}.fbx");
                File.Move(path, newPath);
               // AssetDatabase.ImportAsset(newPath);
            }

            AssetDatabase.Refresh();
            return true;
        }
    }
}