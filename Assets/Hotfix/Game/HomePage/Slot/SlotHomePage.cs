using Blaze.Utility.Base;

namespace Hotfix.Game.Decorate.Slots
{
    public class SlotHomePage : BaseSlotOperate<UHomePage>
    {
    }

    public class UHomePage
    {
        public enum ESkyType
        {
            Light,
            Night
        }

        /// <summary>
        /// 是否是自动
        /// </summary>
        public bool IsAuto = true;

        /// <summary>
        /// 在非自动情况下天气类型
        /// </summary>
        public ESkyType skyType = ESkyType.Light;


        /// <summary>
        /// 是否已显示消息提示（只在第一章播放后显示一次）
        /// </summary>
        public bool IsCompleteTip;
    }
}