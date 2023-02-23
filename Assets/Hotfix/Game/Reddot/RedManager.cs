using System.Collections.Generic;
using Blaze.Core;
using Blaze.Manage.Csv.Enum;
using Hotfix.Game.Reddot.Data;
using Hotfix.Game.Reddot.Slots;

namespace Hotfix.Game.Reddot
{
    /// <summary>
    /// 红点管理器
    /// </summary>
    public class RedManager : Singeton<RedManager>
    {
        // 通过类型，获得已经存在的公共红点
        private readonly Dictionary<RedType, RedData> _redDatas = new Dictionary<RedType, RedData>();

        public void Init()
        {
            // 初始化父子节点指向关系
            InitRelation();

            void Add(RedData redData)
            {
                // 建立关联
                MakeRelation(redData);

                // 添加节点到缓存中
                _redDatas[redData.Type] = redData;
            }

            // 初始化所有的固定根节点
            Add(new RedData(RedType.Root, RedMode.Childs));
            Add(new RedData(RedType.Chapter, RedMode.Childs));
            Add(new RedData(RedType.Sign, RedMode.Childs));
            Add(new RedData(RedType.Mail, RedMode.Childs));
            Add(new RedData(RedType.Phone, RedMode.Childs));
            Add(new RedData(RedType.Mission, RedMode.Childs));
        }


        /// <summary>
        /// 获取公共红点
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public RedData GetPoint(RedType type)
        {
            if (_redDatas.ContainsKey(type))
            {
                return _redDatas[type];
            }

            return null;
        }

        #region 红点树形关系

        // 子节点 -> 父节点
        private readonly Dictionary<RedType, RedType> _relation = new Dictionary<RedType, RedType>();

        private void InitRelation()
        {
            _relation.Clear();
            // 一级红点
            _relation.Add(RedType.Chapter, RedType.Root);
            _relation.Add(RedType.Sign, RedType.Root);
            _relation.Add(RedType.Mail, RedType.Root);
            _relation.Add(RedType.Phone, RedType.Root);
            _relation.Add(RedType.Mission, RedType.Root);

            // 二级红点
            _relation.Add(RedType.ChapterSub, RedType.Chapter);
            _relation.Add(RedType.SignSub, RedType.Sign);
            _relation.Add(RedType.MailSub, RedType.Mail);
            _relation.Add(RedType.PhoneMessages, RedType.Phone);
            _relation.Add(RedType.PhoneDressBook, RedType.Phone);
            _relation.Add(RedType.PhoneDiscover, RedType.Phone);
            _relation.Add(RedType.MissionSub, RedType.Mission);
            // 三级红点
            _relation.Add(RedType.PhoneDiscoverMoment, RedType.PhoneDiscover);
        }

        /// <summary>
        /// 建立关联
        /// </summary>
        /// <param name="redData"></param>
        public void MakeRelation(RedData redData)
        {
            // 存在父节点指向关系，并且父节点不为空
            if (_relation.ContainsKey(redData.Type) && _redDatas.ContainsKey(_relation[redData.Type]))
            {
                // 向父节点中注册当前子节点
                _redDatas[_relation[redData.Type]].AddChild(redData);
            }
        }

        #endregion

        #region 红点存档

        /// <summary>
        /// 从存档中读取状态
        /// </summary>
        /// <returns></returns>
        public bool ReadSlot(string type)
        {
            var slot = SlotRed._.ReadSlot();

            if (slot.Status.ContainsKey(type))
            {
                return slot.Status[type];
            }

            return false;
        }

        /// <summary>
        /// 记录状态到存档
        /// </summary>
        /// <returns></returns>
        public void WriteSlot(string type, bool status)
        {
            var slot = SlotRed._.ReadSlot();
            if (status)
                slot.Status[type] = true;
            else
                // 从存档中移除，缩小存档体积
                slot.Status.Remove(type);
            SlotRed._.Save();
        }

        #endregion


        #region 接入代码

        // private RedData red = new RedData(RedType.ChapterSub, RedMode.Single);
        //
        // void Demo()
        // {
        //     RedManager._.MakeRelation(red);
        //     red.SetLight(true);
        // red.OnMessage += Red;
        // red.OnMessage -= Red;
        // }

        // private void Red(GameObject go,RedData data) {
        //     go.SetActive(data.IsLight);
        // }

        #endregion
    }
}