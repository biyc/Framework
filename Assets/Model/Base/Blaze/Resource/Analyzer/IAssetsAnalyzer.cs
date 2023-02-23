//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/21 | Initialize core skeleton |
*/

namespace Blaze.Resource.Analyzer
{
    /// <summary>
    /// 资源使用分析
    /// </summary>
    public interface IAssetsAnalyzer
    {
        /// <summary>
        /// 当资产被打开
        /// </summary>
        /// <param name="assetPath"></param>
        void OnAssetOpen(string assetPath);

        /// <summary>
        /// 当资产被访问
        /// </summary>
        /// <param name="assetPath"></param>
        void OnAssetAccess(string assetPath);

        /// <summary>
        /// 当资产被关闭
        /// </summary>
        /// <param name="assetPath"></param>
        void OnAssetClose(string assetPath);
    }
}