//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/03/17 | Initialize core skeleton |
*/

using System.Collections.Generic;
using Blaze.Core;
using Blaze.Resource;
using Blaze.Resource.Asset;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Blaze.Manage.Atlas
{
    public sealed class AtlasManager : Singeton<AtlasManager>
    {

        #region Logic

        // 精灵和图集的映射关系
        private Dictionary<string, Sprite> _spriteAtlasMap = new Dictionary<string, Sprite>();

        /// <summary>
        /// 添加图集精灵
        /// </summary>
        /// <param name="path">图集地址</param>
        public void Load(string path)
        {
            Res.LoadAsset(path, isLoadAll: true).Completed +=
                delegate(IUAsset asset)
                {
                    foreach (Object obj in asset.GetObjects())
                    {
                        if (obj is Sprite)
                        {
                            var sprit = obj as Sprite;
                            _spriteAtlasMap[sprit.name] = sprit;
                        }
                    }
                };
        }

        /// <summary>
        /// 获取一个图集精灵
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Sprite Get(string name)
        {
            if (_spriteAtlasMap.ContainsKey(name))
                return _spriteAtlasMap[name];
            else
                return null;
        }

        #endregion
    }
}