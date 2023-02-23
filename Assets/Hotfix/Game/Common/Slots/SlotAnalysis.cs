using Blaze.Utility.Base;
using System.Collections;
using System.Collections.Generic;
using Hotfix.Game.Common.Data;

namespace Game.Common.Slots
{
    public class SlotAnalysis : BaseSlotOperate<SAnalysis>
    {
    }

    public class SAnalysis
    {
        // 限制资源最后恢复时间
        public string LastRestoreTime;

        // 今日已上报的统计点
        public List<string> TodayRecord = new List<string>();
    }
}