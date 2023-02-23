using System;
using System.Collections.Generic;
using System.Linq;
using Blaze.Manage.Csv.Enum;
using Blaze.Manage.Data;
using Sirenix.Utilities;

namespace Hotfix.Game.Reddot.Data
{
    /// <summary>
    /// 红点信息
    /// </summary>
    public class RedData : DataWatch<RedData>
    {
        public RedData() => Put(this);

        public RedData(RedType type)
        {
            Type = type;
            Put(this);
        }

        public RedData(RedType type,RedMode mode)
        {
            Mode = mode;
            Type = type;
            Put(this);
        }

        /// <summary>
        /// 红点类型
        /// </summary>
        public RedType Type;

        /// <summary>
        /// 当前红点工作模式
        /// </summary>
        public RedMode Mode = RedMode.Single;


        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsLight;

        /// <summary>
        /// 父节点
        /// </summary>
        public RedData Parent;

        /// <summary>
        /// 子节点
        /// </summary>
        private HashSet<RedData> Child = new HashSet<RedData>();

        /// <summary>
        /// 子节点回调
        /// </summary>
        private Dictionary<RedData, Action<RedData>> ChildCb = new Dictionary<RedData, Action<RedData>>();




        /// <summary>
        /// 子节点点亮数量
        /// </summary>
        private int _childLightNum = 0;


        /// <summary>
        /// 切换当前红点模式
        /// </summary>
        public void SwitchMode(RedMode mode)
        {
            switch (mode)
            {
                case RedMode.Childs:
                    Mode = RedMode.Childs;
                    UpdateLight();
                    // 子节点全部消除时，当前节点才消除
                    Child.ForEach(delegate(RedData data)
                    {
                        // 子节点状态改变时，更新当前节点状态
                        if (!ChildCb.ContainsKey(data))
                        {
                            ChildCb[data] = delegate(RedData redData) { UpdateLight(); };
                            data.OnIneresting += ChildCb[data];
                        }
                        // data.OnIneresting += delegate(RedData redData) { UpdateLight(); };
                    });

                    break;
            }
        }


        /// <summary>
        /// 更新红点状态
        /// </summary>
        private void UpdateLight()
        {
            switch (Mode)
            {
                case RedMode.Single:
                    // 独立，不受子节点与父节点影响
                    break;
                case RedMode.Childs:
                    // 当前节点点亮状态根据子节点来决定
                    var count = 0;
                    Child.ForEach(delegate(RedData data)
                    {
                        if (data.IsLight)
                            count++;
                    });
                    _childLightNum = count;
                    var tempStatus = count > 0;
                    // 更新点亮状态
                    if (IsLight != tempStatus)
                    {
                        IsLight = tempStatus;
                        ChangeNotify();
                    }
                    break;
                case RedMode.Base:
                    break;

            }
        }


        /// <summary>
        /// 向当前节点下挂载其它节点
        /// </summary>
        /// <param name="node"></param>
        public void AddChild(RedData node)
        {
            Child.Add(node);
            // 设置当前节点为子节点的父节点
            node.Parent = this;
            SwitchMode(Mode);
        }

        /// <summary>
        /// 移除当前节点下的节点
        /// </summary>
        /// <param name="node"></param>
        public void RemoveChild(RedData node)
        {
            if (Child.Contains(node))
            {
                Child.Remove(node);
                node.Parent = null;
            }
            // 如果上面有监听方法，需要把监听方法移除
            if (ChildCb.ContainsKey(node))
            {
                node.OnIneresting -= ChildCb[node];
                ChildCb.Remove(node);
            }
        }


        /// <summary>
        /// 设置是否点亮红点
        /// 外部逻辑调用
        /// </summary>
        /// <param name="status"></param>
        public void SetLight(bool status)
        {

            switch (Mode)
            {
                case RedMode.Single:
                    // 独立，不受子节点与父节点影响
                    IsLight = status;
                    ChangeNotify();
                    break;
                case RedMode.Childs:
                    // 当前节点点亮状态根据子节点来决定(不能手动设定)
                    break;
                case RedMode.Base:
                    // 设置消除红点时，将子节点都设置为不点亮
                    if (!status)
                    {
                        Child.ForEach(delegate(RedData data)
                        {
                            data.SetLight(status);
                        });
                    }
                    break;
            }

        }


    }
}