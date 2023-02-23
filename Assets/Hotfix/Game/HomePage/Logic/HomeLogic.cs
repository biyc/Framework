using System;
using Blaze.Core;
using Blaze.Manage.Data;
using Hotfix.Game.Decorate.Slots;
using UniRx;

public class HomeLogic : Singeton<HomeLogic>
{
    public DataWatch<bool> IsSkyChange = new DataWatch<bool>();


    /// <summary>
    /// 观察界面上是否应该显示提示
    /// </summary>
    public DataWatch<bool> WatchShowMessageTip = new DataWatch<bool>();

    public void Test()
    {
        SlotHomePage._.ReadSlot().IsCompleteTip = false;
        SlotHomePage._.Save();
    }
    
    /// <summary>
    /// 章节完成，通知界面变化
    /// </summary>
    /// <param name="id"></param>
    public void FinishChapter(int id)
    {
        if (id != 20001)
            return;

        var slot = SlotHomePage._.ReadSlot();
        if (slot.IsCompleteTip)
        {
            WatchShowMessageTip.ChangeNotify(false);
            return;
        }
         
        WatchShowMessageTip.ChangeNotify(true);
    }

    /// <summary>
    /// 点击提示后调用完成
    /// </summary>
    public void FinishMessageTip()
    {
        SlotHomePage._.ReadSlot().IsCompleteTip = true;
        SlotHomePage._.Save();
        WatchShowMessageTip.ChangeNotify(false);
    }

    /// <summary>
    /// 改变昼夜
    /// </summary>
    /// <param name="skyType"></param>
    public void SkyChange(UHomePage.ESkyType skyType)
    {
        SlotHomePage._.ReadSlot().skyType = skyType;
        SlotHomePage._.Save(); 
        IsSkyChange.ChangeNotify(true);
    }
    
}