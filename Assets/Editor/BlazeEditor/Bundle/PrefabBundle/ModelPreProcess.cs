using Blaze.Core;
using UnityEditor;

namespace Blaze.Bundle.PrefabBundle
{
    /// <summary>
    /// 模型的提前处理
    /// </summary>
    public class ModelPreProcess : Singeton<ModelPreProcess>
    {
        public void Execution(string name)
        {
            var obj = AssetDatabase.FindAssets("t:Prefab", new[] {$"Assets/Projects/Models/{name}"});
        }
    }
}