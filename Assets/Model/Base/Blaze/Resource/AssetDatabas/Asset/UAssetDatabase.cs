//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/22 | Initialize core skeleton |
*/

using System;
using System.IO;
using System.Text;
using Blaze.Resource.Asset;
using Blaze.Utility;
using Blaze.Utility.Helper;
using UniRx;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Blaze.Resource.AssetDatabas.Asset
{
    public class UAssetDatabase : IUAsset
    {
        public UAssetDatabase(string assetPath, Type type, bool isLoadAll = false, bool isSync = true)
            : base(assetPath)
        {
            Object _object = null;
            Object[] _objAll = null;

#if UNITY_EDITOR
            if (isLoadAll)
            {
                _objAll = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath);
                if (isSync)
                    CompleteAll(_objAll);
                else
                {
                    Observable.NextFrame().Subscribe(delegate(Unit unit) { CompleteAll(_objAll); });
                }
            }
            else
            {
                _object = type != null
                    ? AssetDatabase.LoadAssetAtPath(assetPath, type)
                    : AssetDatabase.LoadMainAssetAtPath(assetPath);
                if (isSync)
                    Complete(_object);
                else
                {
                    Observable.NextFrame().Subscribe(delegate(Unit unit) { Complete(_object); });
                }
            }
#endif
            Res.GetAnalyzer().OnAssetOpen(assetPath);
        }

        protected override bool IsValid()
        {
            return Asset.Get() != null;
        }

        public override byte[] ReadAllBytes()
        {
            var realpath = PathHelper.Combine(PathHelper.GetCurrentPath(), _assetPath);
            return File.ReadAllBytes(realpath);
        }

        public override string ReadAllText()
        {
            var realpath = PathHelper.Combine(PathHelper.GetCurrentPath(), _assetPath);
            return File.ReadAllText(realpath, Encoding.Default);
        }

        // protected override void Dispose(bool bManaged)
        // {
        //     // if (!_disposed)
        //     // {
        //     //     Res.GetAnalyzer().OnAssetClose(_assetPath);
        //     //     Debug.LogFormat("UAssetDatabaseAsset {0} released {1}", _assetPath, bManaged);
        //     //     // _disposed = true;
        //     // }
        // }
    }
}