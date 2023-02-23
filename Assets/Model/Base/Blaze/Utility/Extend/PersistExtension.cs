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
using Newtonsoft.Json;
using SharpYaml.Serialization;

namespace Blaze.Utility.Extend
{
    /// <summary>
    /// 静态扩充类,主要对于Json和Protobuf和Yaml的扩展
    /// </summary>
    public static class PersistExtension
    {
        #region JSON

        private static SerializerSettings settings = new SerializerSettings();

        static PersistExtension()
        {
            settings.EmitAlias = false;
            settings.EmitTags = false;
            settings.IndentLess = false;
            settings.EmitDefaultValues = false;
            settings.EmitJsonComptible = false;
            settings.EmitCapacityForList = false;
            settings.EmitShortTypeName = false;
            settings.SortKeyForMapping = false;
            settings.SerializeDictionaryItemsAsMembers = true;
        }

        public static string ToJson<T>(this T obj) where T : IPersistable
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public static string SaveJson<T>(this T obj, string path) where T : IPersistable
        {
            var jsonContent = obj.ToJson();
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }

            File.WriteAllText(path, jsonContent);
            return jsonContent;
        }

        #endregion

        #region YAML

        public static string ToYaml<T>(this T obj) where T : IPersistable
        {
            var serializer = new Serializer(settings);
            return serializer.Serialize(obj);
        }

        public static string SaveYaml<T>(this T obj, string path) where T : IPersistable
        {
            var serializer = new Serializer(settings);
            var yaml = serializer.Serialize(obj);
            File.WriteAllText(path, yaml);
            return yaml;
        }

        #endregion
    }
}