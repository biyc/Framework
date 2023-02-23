//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/21 | Initialize core skeleton |
*/

using System;
using Blaze.Manage.Data;
using Blaze.Resource.Asset;

namespace Blaze.Resource.Common
{
    /// <summary>
    /// 资源提供接口
    /// </summary>
    public interface IAssetProvider
    {
        /// <summary>
        /// 资源提供器准备好，完成回调
        /// </summary>
        ICompleted<bool> Ready();


        /// <summary>
        /// 获取作为一个具象的Unity的对象
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IUAsset GetAsset(string assetPath, Type type, bool isLoadAll = false);

        /// <summary>
        /// 获取作为一个具象的Unity的对象
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IUAsset GetAssetSync(string assetPath, Type type, bool isLoadAll = false);

        /// <summary>
        /// 资源是否存在
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        bool IsAssetExists(string assetPath);


        /// <summary>
        /// 初始化 资源提器
        /// </summary>
        void Open();

        /// <summary>
        /// 关闭 资源提器
        /// </summary>
        void Close();
    }
}