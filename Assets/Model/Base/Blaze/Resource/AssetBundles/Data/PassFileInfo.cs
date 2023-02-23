using System;
using System.Collections.Generic;
using Blaze.Common;
using Blaze.Core;
using Blaze.Utility.Helper;

namespace Blaze.Resource.AssetBundles.Data
{
    /// <summary>
    /// 核心装配文件
    /// </summary>
    [Named("PassFileInfo")]
    [Persist(CamelFileName = true)]
    public class PassFileInfo : Persistable<PassFileInfo>
    {
        public List<string> FilesHash = new List<string>();
    }
}