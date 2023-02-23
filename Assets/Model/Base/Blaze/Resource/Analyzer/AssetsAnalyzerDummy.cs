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
    /// 资源使用分析接口哑元
    /// </summary>
    public class AssetsAnalyzerDummy : IAssetsAnalyzer
    {
        public void OnAssetOpen(string assetPath)
        {
        }

        public void OnAssetAccess(string assetPath)
        {
        }

        public void OnAssetClose(string assetPath)
        {
        }
    }
}