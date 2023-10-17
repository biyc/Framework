using System.Collections.Generic;
using Blaze.Utility.Base;

namespace Hotfix.Game.Reddot.Slots
{
    /// <summary>
    /// 存档读写引用
    /// </summary>
    public class SlotRed : BaseSlotOperate<URed>
    {
    }
    /// <summary>
    /// 存档结构
    /// </summary>
    public class URed
    {

        /// <summary>
        /// 红点状态
        /// 红点名称 -> 状态
        /// </summary>
        public Dictionary<string, bool> Status = new Dictionary<string, bool>();
    }

}