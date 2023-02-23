using System;
using System.Collections.Generic;
using Blaze.Manage.Csv.Enum;
using Blaze.Resource;
using Blaze.Resource.Asset;
using Blaze.Utility;
using Hotfix.Game.Common.Logic;
using Sirenix.Utilities;
using UnityEngine;

namespace Blaze.Manage.Csv.Poco
{
    /// <summary>
    /// 资源信息
    /// </summary>
    public class ResInfo
    {
        /// 资源类型
        public long Type = 0;

        /// 资源数量,（家具等是具体家具id)
        public long Num = 0;

        /// 特殊通过字符串描述的资源
        public string Data = "";

        /// 资源名称
        public string Name;

        /// 是否提示资源已解锁 ///对于皮肤是是否直接装备的意思，false时不装备皮肤，默认装备
        public bool IsTipUnlock = true;

        /// 资源在什么地方产生的变动（用于向友盟上报数据时使用）
        public AnalysisParam Analysis = default;


        /// <summary>
        /// 根据 type num 去对应的信息模块下加载 name 等信息
        /// </summary>
        /// <returns></returns>
        public ResInfo LoadInfo()
        {
            switch (GetTypeName())
            {
                case ResType.Furniture:
                case ResType.GiftPack:
                    // 填充礼包名称
                    Name = GiftPack._.Name((int) Num);
                    break;
                default:
                    var csv = CsvHelper.GetResIndexCsv().GetRowByUnid((int) Type);
                    if (csv != null)
                        Name = csv.NameDesc;
                    break;
            }

            return this;
        }

        /// <summary>
        /// CSV 数据装载器 type类型：数量 或者 type类型：描述
        /// </summary>
        /// <param name="data">100001:300</param>
        /// <returns></returns>
        public static ResInfo Create(string data)
        {
            if (string.IsNullOrEmpty(data)) return null;

            try
            {
                var obj = new ResInfo();
                var item = data.Split(':');
                obj.Type = long.Parse(item[0]);
                // obj.Num = long.TryParse(item[1]);
                if (!long.TryParse(item[1], out obj.Num))
                {
                    obj.Data = item[1];
                }

                return obj;
            }
            catch (Exception e)
            {
                Tuner.Error(data);
                throw;
            }
        }

        /// <summary>
        /// 返回真实的数量
        /// </summary>
        /// <returns></returns>
        public int GetRealNum()
        {
            switch (GetTypeName())
            {
                case ResType.Furniture:
               // case ResType.Sculpt:
                case ResType.FriendMoments:
                case ResType.Album:
                case ResType.Cartoon:
                case ResType.Dress:
                case ResType.PhoneChat:
                case ResType.Room:
                    return 1;
                default:
                    return (int) Num;
            }
        }

        /// <summary>
        /// 获取到类型对应的枚举
        /// </summary>
        /// <returns></returns>
        public ResType GetTypeName()
        {
            return (ResType) System.Enum.ToObject(typeof(ResType), (int) Type);
        }

        /// <summary>
        /// 根据大分类类型获取 大类ICON
        /// </summary>
        /// <param name="successCb"></param>
        /// <returns></returns>
        public IUAsset GetTypeIcon(Action<Sprite> successCb, string size = "")
        {
            var row = GetCsvRow();
            if (string.IsNullOrEmpty(row.IconDef))
                Tuner.Error("没有找到图标信息" + GetTypeName());

            var assetPath = $"Assets/Projects/Texture/Icon/{row.IconDef + size}.png";
            // return Res.LoadAsset(assetPath, successCb);
            var ass = Res.LoadAssetSync(assetPath, typeof(Sprite));
            successCb.Invoke(ass.Get<Sprite>());
            return ass;
        }


        /// <summary>
        /// 执行奖励时获取对应图标
        /// </summary>
        /// <param name="successCb"></param>
        /// <returns></returns>
        public IUAsset GetRewardIcon(Action<Sprite> successCb, string size = "@gift")
        {
            string iconPath = "";
            switch (GetTypeName())
            {
                case ResType.Furniture:
                    break;
                default:
                    var csv = CsvHelper.GetResIndexCsv().GetRowByUnid((int) Type);
                    if (csv == null)
                    {
                        Debug.LogError($"ResIndexCsv中没有找到类型为<color=#ff0000>{Type}</color>的资源");
                        successCb?.Invoke(null);
                        return null;
                    }

                    iconPath = $"Assets/Projects/Texture/Icon/{csv.IconDef + size}.png";
                    break;
            }


            var ass = Res.LoadAssetSync(iconPath, typeof(Sprite));
            successCb?.Invoke(ass.Get<Sprite>());
            return ass;
        }

        /// <summary>
        /// 获取当前数据在CSV中的原始描述
        /// </summary>
        /// <returns></returns>
        public ResIndexRow GetCsvRow()
        {
            return CsvHelper.GetResIndexCsv().GetRowByUnid((int) Type);
        }

        /// <summary>
        /// 获取资源中文名称
        /// </summary>
        /// <returns></returns>
        public string GetResName()
        {
            return CsvHelper.GetResIndexCsv().GetRowByUnid((int) Type).NameDesc;
        }
    }


    /// <summary>
    /// 资源信息
    /// </summary>
    public class ResInfoList
    {
        public List<ResInfo> ResInfos = new List<ResInfo>();


        // 100001:300|10002:50
        public static ResInfoList Create(string data)
        {
            var obj = new ResInfoList();
            data.Split('|').ForEach(delegate(string line)
            {
                var resInfo = ResInfo.Create(line);
                if (resInfo != null)
                    obj.ResInfos.Add(resInfo);
            });
            return obj;
        }
    }
}