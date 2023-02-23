//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/21 | Initialize core skeleton |
*/


using UnityEngine;

namespace Blaze.Resource.Asset
{
    /// <summary>
    /// 代表无法加载的资源
    /// </summary>
    public class UAssetFailure : IUAsset
    {
        public UAssetFailure(string assetPath)
            : base(assetPath)
        {
            Complete(null);
            CompleteAll(null);

            if (assetPath!=null && !assetPath.StartsWith("Data/CsvData/Blaze.Manage.Locale$Locale"))
                Debug.LogError("资源没有找到" + assetPath);
        }

        protected override bool IsValid()
        {
            return false;
        }

        public override byte[] ReadAllBytes()
        {
            return null;
        }

        public override string ReadAllText()
        {
            return null;
        }
    }
}