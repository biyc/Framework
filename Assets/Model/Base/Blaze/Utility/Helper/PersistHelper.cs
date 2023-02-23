//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | Tim    | 2019/10/18 | Initialize core skeleton |
*/

using System.IO;
using Blaze.Core;
using Blaze.Resource.AssetBundles.Logic.StreamingAssets;
using Blaze.Utility.Helper;
using Newtonsoft.Json;
using SharpYaml.Serialization;

namespace Blaze.Utility.Extend
{
    /// <summary>
    /// 静态扩充类,主要对于Json和Protobuf和Yaml的扩展
    /// </summary>
    public static class PersistHelper
    {
        #region JSON

        /// <summary>
        /// 从Json字符串获取一个可持久化对象
        /// </summary>
        /// <param name="json"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FromJson<T>(string json) where T : IPersistable
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 从一个Json文件获取一个可持久化对象
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T LoadJson<T>(string path) where T : IPersistable
        {
            // 如果是在SteamingAssets的文件
            if (path.Contains(PathHelper.GetStreamingPath()))
                return FromJson<T>(StreamingAssetsLoader._.LoadSync(path));

            return FromJson<T>(File.ReadAllText(path));
        }

        #endregion

        #region YAML

        /// <summary>
        /// 从Yaml字符串获取一个可持久化对象
        /// </summary>
        /// <param name="yaml"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FromYaml<T>(string yaml) where T : IPersistable
        {
            var input = new StringReader(yaml);
            var deserializer = new Serializer();
            return deserializer.Deserialize<T>(input);
        }

        /// <summary>
        /// 从Yaml文件获取一个可持久化对象
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T LoadYaml<T>(string path) where T : IPersistable
        {
            // 如果是在SteamingAssets的文件
            if (path.Contains(PathHelper.GetStreamingPath()))
                return FromYaml<T>(StreamingAssetsLoader._.LoadSync(path));

            return FromYaml<T>(File.ReadAllText(path));
        }

        #endregion
    }
}