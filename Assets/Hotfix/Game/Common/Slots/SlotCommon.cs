using Blaze.Utility.Base;
using System.Collections;
using System.Collections.Generic;
using Hotfix.Game.Common.Data;

namespace Game.Common.Slots
{
    public class SlotCommon : BaseSlotOperate<SCommon>
    {
    }

    public class SCommon
    {
        // 是否签署用户协议
        public bool IsAgreement;

        // 体力点数 最后恢复时间
        public long PhysicalLastTime;

        // 体力点数 当前点数
        public int PhysicalCurPoint;

        // 随机用户头像
        public string DefaultHead;

        // 用户昵称
        public string Nickname;

        // 是否已初始化存档
        public bool IsInit;

        // 游戏设置
        public Seting Setting = new Seting();

        // 是否已初始化设置
        public bool IsInitSetting;

        // 本地背包系统
        public List<ResPackSave> LocalPack = new List<ResPackSave>();

        // 限制资源最后恢复时间
        public string LimitResLastRestoreTime;

        // 是否给游戏充过钱（默认是 false ，没有充过钱）
        public bool IsCharge;

        // 支付订单状态 orderId -> orderInfo
        public Dictionary<string, OrderInfo> PayOrder = new Dictionary<string, OrderInfo>();

        // 钻石数量
        public uint DiamondNum;

        // 是否已初始化钻石
        public bool IsInitDiamond;

        // 已使用的兑换码
        public List<string> RedemptionCode = new List<string>();

        /// <summary>
        /// 通过快爆兑换的兑换码
        /// </summary>
        public List<string> KbCode = new List<string>();

        /// <summary>
        /// 通过兑换码兑换的礼包ID
        /// </summary>
        public List<int> CodeGift = new List<int>();

        // 是否已经播放序章引导页面
        public bool IsSuccessGuidChapter;

        // 是否已完成抢先体验版本
        public bool IsSuccessFirstTry;
    }


    public class ResPackSave
    {
        /// 资源类型ID
        public int ResTypeId;

        /// 资源数量
        public long ResNum;
        // public ObscuredLong ResNum;

        /// 资源是否执行过初始化
        public bool IsInit;

        /// 定期恢复型数值，最后恢复时间
        public long ResetTime;

        public static ResPackSave Create(ResPack data)
        {
            var obj = new ResPackSave();
            obj.IsInit = data.IsInit;
            obj.ResTypeId = data.ResTypeId;
            obj.ResNum = data.ResNum;
            obj.ResetTime = data.ResetTime;
            return obj;
        }
    }


    public class OrderInfo
    {
        // 订单ID
        public string OrderID;

        // 产品ID
        public string ProductID;

        // 购买时间
        public long PurchaseTime;

        // 订单的购买状态。可能的值为：0。已购买1.已取消2.待定
        public int PurchaseState;

        //Inapp产品的确认状态 ios中无效
        public bool Acknowledged;

        // 验证TOKEN
        public string PurchaseToken;

        //订阅专用字段，是否自动续订 ios中无效
        public bool AutoRenewing;

        // 是否已经发放奖励
        public bool IsReward;
    }

// 游戏设置
    public class Seting
    {
        // 是否开启音效
        public bool SoundIsOn;

        // 是否开启音乐
        public bool MusicIsOn;

        // 是否开启震动
        public bool VibrateIsOn;

        // 是否开启特效
        public bool EffectsIsOn;
    }
}