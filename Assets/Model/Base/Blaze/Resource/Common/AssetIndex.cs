//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/20 | Initialize core skeleton |
*/

using System.Collections.Generic;
using Blaze.Resource.AssetBundles.Data;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Blaze.Resource.Common
{
    /// <summary>
    /// 包运行期索引 (资源快速索引器)
    /// 所有资源路径
    /// </summary>
    public sealed class AssetIndex
    {
        #region Singleton

        private static readonly AssetIndex _instance = new AssetIndex();
        public static AssetIndex _ => _instance;

        #endregion

        #region Logic

        // 所有的资源路径
        private List<string> _allPath = new List<string>();


        /// <summary>
        /// 查找路径下的所有资源
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string FindAtPath(string path)
        {
            return _allPath.Find(delegate(string fullPath) { return fullPath.Contains(path); });
        }

        /// <summary>
        /// 查找路径下的所有资源
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<string> FindsAtPath(string path, string filter = "")
        {
            // Tuner.Log("查找路径下的所有资源： " + path + "  " + filter);
            // path = path.ToLower();
            return _allPath.FindAll(delegate(string fullPath)
            {
                return fullPath.Contains(path) && fullPath.Contains(filter);
            });
        }

        public string FindAtPath(string path, string filter = "")
        {
            // Tuner.Log("查找路径下的所有资源： " + path + "  " + filter);
            // path = path.ToLower();
            return _allPath.Find(delegate(string fullPath)
            {
                return fullPath.Contains(path) && fullPath.Contains(filter);
            });
        }


        /// <summary>
        /// AssetBundle 通过MF 创建索引
        /// </summary>
        public void Build(ManifestInfo mf)
        {
            mf.ManifestList.ForEach(delegate(ManifestData data)
            {
                var item = data.AssetPath;
                _allPath.Add(item);
            });
        }

        /// <summary>
        /// Database 创建索引
        /// </summary>
        public void BuildAssetDatabaseIndex()
        {
#if UNITY_EDITOR
            string[] files = AssetDatabase.GetAllAssetPaths();
            foreach (string file in files)
            {
                if (!file.StartsWith("Assets/Projects") && !file.StartsWith("Assets/Bundles"))
                    continue;

                _allPath.Add(file);
            }
#endif
        }

        #endregion
    }
}