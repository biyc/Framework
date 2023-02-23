//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | Tim    | 2019/10/18 | Initialize core skeleton |
*/

using System;
using System.IO;
using Blaze.Common;
using Blaze.Utility;
using Blaze.Utility.Extend;
using Blaze.Utility.Helper;
using Blaze.Utility.Impl;
using JetBrains.Annotations;
using UnityEngine;

namespace Blaze.Core
{
    /// <summary>
    /// 可持久化对象基类,继承与可命名基类
    /// 默认的存储类型是JSON
    /// </summary>
    [Serializable]
    public class Persistable<T> : Namedable, IPersistable where T : class, IPersistable
    {
        #region Initialize

        [NonSerialized] private string _fullPathFile;

        [NonSerialized] private string _ext;
        [NonSerialized] private string _path;
        [NonSerialized] private EnumPersistType _type;
        [NonSerialized] private string _name;

        [NonSerialized] private PersistAttribute _attr;

        /// <summary>
        /// 对持久化对象进行可持久化参数设置
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="type"></param>
        /// <param name="ext"></param>
        private void Init([CanBeNull] string filePath = null,
            EnumPersistType type = EnumPersistType.JSON,
            string ext = "json", bool camelFileName = true)
        {
            _type = type;
            _ext = ext;
            _path = filePath ?? PathHelper.GetPersistentPath();
            _name = GetName() ?? GetType().ToString();
            if (camelFileName)
            {
                _name = CaseAlgorithm.Get(_name, CaseAlgorithm.CaseMode.PascalCase).Replace(" ", "");
            }
            else
            {
                _name = _name.Replace(' ', '_').ToLower();
            }

            _fullPathFile = _path.TrimEnd('/') + "/" + _name + "." + _ext.ToLower().TrimStart('.');
        }

        /// <summary>
        /// 获取持久化定义路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static string GetFullPathName(string filePath = null)
        {
            NamedAttribute attrName = (AttrHelper.GetAttribute<NamedAttribute>(typeof(T)));
            PersistAttribute attr = AttrHelper.GetAttribute<PersistAttribute>(typeof(T));

            var m_ext = attr.Ext;
            var m_path = filePath ?? attr.Path ?? PathHelper.GetPersistentPath();

            if (!string.IsNullOrEmpty(filePath))
            {
                m_path = filePath;
            }
            else if (string.IsNullOrEmpty(attr.Path) || attr.Path.Equals(PathHelper.GetPersistentPath()))
            {
                m_path = PathHelper.GetPersistentPath();
            }
            else
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    m_path = PathHelper.GetStreamingPath() + "/Config";
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    m_path = PathHelper.GetStreamingPath() + "/Config";
                }
                else
                {
                    m_path = attr.Path;
                }
            }

            var m_name = attrName.Name ?? typeof(T).ToString();
            if (attr.CamelFileName)
            {
                m_name = CaseAlgorithm.Get(m_name, CaseAlgorithm.CaseMode.PascalCase).Replace(" ", "");
            }
            else
            {
                m_name = m_name.Replace(' ', '_').ToLower();
            }

            var m_fullPathFile = m_path.TrimEnd('/') + "/" + m_name + "." + m_ext.ToLower().TrimStart('.');
            return m_fullPathFile;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Persistable()
        {
            Config();
        }

        /// <summary>
        /// 对持久化对象进行配置重新激活
        /// </summary>
        public void Config(string pathName = null)
        {
            _attr = AttrHelper.GetAttribute<PersistAttribute>(GetType());
            if (_attr != null)
            {
                if (pathName != null)
                {
                    Init(pathName, _attr.Type, _attr.Ext, _attr.CamelFileName);
                }
                else
                {
                    Init(_attr.Path, _attr.Type, _attr.Ext, _attr.CamelFileName);
                }
            }
            else
            {
                Init();
            }
        }

        /// <summary>
        /// 保存持久化
        /// </summary>
        public string Save()
        {
            switch (_type)
            {
                case EnumPersistType.JSON:
                    return this.SaveJson(_fullPathFile);
                case EnumPersistType.YAML:
                    return this.SaveYaml(_fullPathFile);
            }

            return null;
        }

        /// <summary>
        /// 创建一个新的持久化对象, 这种情况很少见,一般是起到克隆的作用.
        /// </summary>
        /// <returns></returns>
        public T Create()
        {
            if (File.Exists(_fullPathFile))
            {
                try
                {
                    switch (_type)
                    {
                        case EnumPersistType.JSON:
                            var json = PersistHelper.LoadJson<T>(_fullPathFile) as Persistable<T>;
                            json.Config(_fullPathFile);
                            return json as T;
                        case EnumPersistType.YAML:
                            var yaml = PersistHelper.LoadYaml<T>(_fullPathFile) as Persistable<T>;
                            yaml.Config(_fullPathFile);
                            return yaml as T;
                    }
                }
                catch (Exception e)
                {
                    Tuner.Error("Deserialize fail! still empty {0}", e);
                    return null;
                }
            }

            Tuner.Warn("File {0} not exist!", _fullPathFile);
            return null;
        }

        /// <summary>
        /// 获取全新的对象,具有持久化配置信息
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static T Load(string filePath = null)
        {
            string fullFilePa;
            PersistAttribute attr = AttrHelper.GetAttribute<PersistAttribute>(typeof(T));
            if (filePath == null)
            {
                fullFilePa = GetFullPathName();
            }
            else
            {
                fullFilePa = GetFullPathName(filePath);
            }

            if (File.Exists(fullFilePa) || fullFilePa.Contains(PathHelper.GetStreamingPath()))
            {
                try
                {
                    switch (attr.Type)
                    {
                        case EnumPersistType.JSON:
                            var json = PersistHelper.LoadJson<T>(fullFilePa) as Persistable<T>;
                            json.Config(filePath);
                            return json as T;
                        case EnumPersistType.YAML:
                            var yaml = PersistHelper.LoadYaml<T>(fullFilePa) as Persistable<T>;
                            yaml.Config(filePath);
                            return yaml as T;
                    }
                }
                catch (Exception e)
                {
                    Tuner.Error("Deserialize fail! still empty {0}", e);
                    return null;
                }
            }

            Tuner.Warn("File {0} not exist!", filePath);
            return null;
        }

        /// <summary>
        /// 获取全新的对象,具有持久化配置信息
        /// </summary>
        /// <param name="fileNamePath">文件路径</param>
        /// <returns></returns>
        public static T LoadFromFile(string fileNamePath)
        {
            PersistAttribute attr = AttrHelper.GetAttribute<PersistAttribute>(typeof(T));

            if (File.Exists(fileNamePath))
            {
                try
                {
                    switch (attr.Type)
                    {
                        case EnumPersistType.JSON:
                            var json = PersistHelper.LoadJson<T>(fileNamePath) as Persistable<T>;
                            json.Config(fileNamePath);
                            return json as T;
                        case EnumPersistType.YAML:
                            var yaml = PersistHelper.LoadYaml<T>(fileNamePath) as Persistable<T>;
                            yaml.Config(fileNamePath);
                            return yaml as T;
                    }
                }
                catch (Exception e)
                {
                    Tuner.Error("Deserialize fail! still empty {0}", e);
                    return null;
                }
            }

            Tuner.Warn("File {0} not exist!", fileNamePath);
            return null;
        }

        #endregion
    }
}