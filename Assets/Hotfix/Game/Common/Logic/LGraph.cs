using System;
using System.Collections.Generic;
using Blaze.Core;
using Blaze.Manage.Csv.Enum;
using Blaze.Manage.Csv.Poco;
using Game.Common.Slots;
using Hotfix.Game.Common.Data;
using Sirenix.Utilities;

namespace Hotfix.Game.Common.Logic
{
    /// <summary>
    /// 本地背包系统
    /// </summary>
    public class LGraph : Singeton<LGraph>
    {
        /// <summary>
        /// 按照资源标号，存放一些简单类型的背包数据
        /// </summary>
        private readonly Dictionary<long, ResPack> _packData = new Dictionary<long, ResPack>();

        /// <summary>
        /// 观察数据的用户  数据id -> 用户组
        /// </summary>
        private readonly Dictionary<long, Action<long>> _watchData = new Dictionary<long, Action<long>>();

        /// <summary>
        /// 观察数据添加  数据id -> 添加数量监听
        /// </summary>
        private readonly Dictionary<long, Action<long>> _watchAdd = new Dictionary<long, Action<long>>();

        // 初始化背包数据
        public void Init()
        {
            // 初始化默认背包
            CsvHelper.GetResIndexCsv().GetTable()
                .ForEach(delegate(ResIndexRow row)
                {
                    var resInfo = new ResPack();
                    resInfo.ResNum = row.InitNum;
                    resInfo.ResTypeId = row.Sn;
                    resInfo.IsInit = true;
                    _packData[row.Sn] = resInfo;
                });

            // 加载存档中的真实值
            SlotCommon._.ReadSlot().LocalPack.ForEach(delegate(ResPackSave pack)
            {
                _packData[pack.ResTypeId].ResNum = pack.ResNum;
                _packData[pack.ResTypeId].ResetTime = pack.ResetTime;
            });

            // 通知
            foreach (var pair in _packData)
            {
                // 通知观察此数据的消费者
                if (_watchData.ContainsKey(pair.Key))
                    _watchData[pair.Key]?.Invoke(pair.Value.ResNum);
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="resTypeId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public long GetData(ResType resType) => GetData((long) resType);

        public long GetData(long resTypeId)
        {
            if (_packData.ContainsKey(resTypeId))
                return _packData[resTypeId].ResNum;

            return 0;
        }

        /// <summary>
        /// 获取一个数据
        /// </summary>
        /// <param name="resTypeId">数据编号</param>
        /// <param name="dataCb">回调函数</param>
        public void GetData(ResType resType, Action<long> dataCb) => GetData((long) resType, dataCb);

        public void GetData(long resTypeId, Action<long> dataCb)
        {
            // 添加观察
            if (_packData.ContainsKey(resTypeId))
                dataCb?.Invoke(_packData[resTypeId].ResNum);
        }


        /// <summary>
        /// 检查资源是否足够
        /// </summary>
        /// <param name="resType"></param>
        /// <param name="costNum"></param>
        /// <param name="statusCb"></param>
        public void CheckRes(ResType resType, long costNum, Action<bool> statusCb)
        {
            var resTypeId = (long) resType;
            if (_packData.ContainsKey(resTypeId) && _packData[resTypeId].ResNum >= costNum)
            {
                statusCb?.Invoke(true);
                return;
            }

            statusCb?.Invoke(false);
        }

        /// <summary>
        /// 花费背包中某种资源
        /// </summary>
        /// <param name="resTypeId"></param>
        /// <param name="costNum"></param>
        /// <param name="endCb"></param>
        public void CostRes(ResType resType, long costNum, Action<bool> endCb, AnalysisParam analysis = default) =>
            CostRes((long) resType, costNum, endCb, analysis);

        public void CostRes(ResInfo resInfo, Action<StatusData> endCb, AnalysisParam analysis = default)
        {
            CostRes(resInfo.Type, resInfo.Num, delegate(bool b)
            {
                StatusData status = StatusData.Success;
                if (!b)
                {
                    status = StatusData.ResNotEnough;
                    status.Desc = resInfo.GetResName() + "不足";
                    status.ResNum = resInfo.Num;
                    status.ResType = resInfo.Type;
                }

                endCb?.Invoke(status);
            }, analysis);
        }

        public void CostRes(long resTypeId, long costNum, Action<bool> endCb, AnalysisParam analysis = default)
        {
            if (_packData.ContainsKey(resTypeId))
            {
                if (_packData[resTypeId].ResNum >= costNum)
                {
                    _packData[resTypeId].ResNum = _packData[resTypeId].ResNum - costNum;
                }
                else
                {
                    // 资源不足
                    endCb?.Invoke(false);
                    return;
                }
            }
            else
            {
                // 错误的资源类型
                endCb?.Invoke(false);
                return;
            }

            Save();
            Change(resTypeId);
            endCb?.Invoke(true);


            switch ((ResType) Enum.ToObject(typeof(ResType), (int) resTypeId))
            {
                case ResType.Petals:
                    break;
            }
        }

        /// <summary>
        /// 花费背包中某种资源
        /// </summary>
        /// <param name="resType"></param>
        /// <param name="costNum"></param>
        /// <param name="endCb"></param>
        public void AddRes(ResType resType, long costNum, Action<bool> endCb = null,AnalysisParam analysis = default) =>
            AddRes((long) resType, costNum, endCb,analysis);

        public void AddRes(long resTypeId, long costNum, Action<bool> endCb = null,AnalysisParam analysis = default)
        {
            if (_packData.ContainsKey(resTypeId))
                _packData[resTypeId].ResNum = _packData[resTypeId].ResNum + costNum;
            else
            {
                // 错误的资源类型
                endCb?.Invoke(false);
                return;
            }

            Save();
            Change(resTypeId);
            endCb?.Invoke(true);

            if (_watchAdd.ContainsKey(resTypeId))
                _watchAdd[resTypeId]?.Invoke(costNum);


            switch ((ResType) Enum.ToObject(typeof(ResType), (int) resTypeId))
            {
                case ResType.Petals:

                    break;
                case ResType.Yanhua1002:
                case ResType.Yanhua1003:
                case ResType.Yanhua1004:
                    break;

                case ResType.ChapterCard:
                    break;

                case ResType.BranchKey:
                    break;
                case ResType.LoveCard:
                    break;
                case ResType.FurnitureCoin:
                    break;
            }
        }

        /// <summary>
        /// 写入背包中某种资源
        /// </summary>
        /// <param name="resTypeId"></param>
        /// <param name="costNum"></param>
        /// <param name="endCb"></param>
        public void WriteRes(long resTypeId, long costNum, Action<bool> endCb = null)
        {
            if (_packData.ContainsKey(resTypeId))
            {
                _packData[resTypeId].ResNum = costNum;
                Save();
                Change(resTypeId);
                endCb?.Invoke(true);
            }
            else
            {
                endCb?.Invoke(false);
            }
        }


        /// <summary>
        /// 持续观察一个数据的变化
        /// </summary>
        /// <param name="resTypeId">数据编号</param>
        /// <param name="dataCb">回调函数</param>
        public void WatchData(ResType resType, Action<long> dataCb) => WatchData((long) resType, dataCb);


        /// <summary>
        /// 当数据添加时，发出通知
        /// </summary>
        public void WatchAdd(ResType resType, Action<long> dataCb)
        {
            var resTypeId = (long) resType;
            // 添加观察
            if (!_watchAdd.ContainsKey(resTypeId))
            {
                _watchAdd[resTypeId] = dataCb;
            }
            else
            {
                _watchAdd[resTypeId] += dataCb;
            }
        }

        public void WatchData(long resTypeId, Action<long> dataCb)
        {
            // 添加观察
            if (!_watchData.ContainsKey(resTypeId))
            {
                _watchData[resTypeId] = dataCb;
            }
            else
            {
                _watchData[resTypeId] += dataCb;
            }

            if (_packData.ContainsKey(resTypeId))
            {
                dataCb?.Invoke(_packData[resTypeId].ResNum);
            }
        }


        public void RemoveWatch(ResType resTypeId, Action<long> dataCb) => RemoveWatch((long) resTypeId, dataCb);

        public void RemoveWatch(long resTypeId, Action<long> dataCb)
        {
            _watchData[resTypeId] -= dataCb;
        }


        /// <summary>
        /// 通知观察数据的用户
        /// </summary>
        /// <param name="resTypeId"></param>
        private void Change(long resTypeId)
        {
            // 通知观察此数据的消费者
            if (_watchData.ContainsKey(resTypeId))
            {
                _watchData[resTypeId]?.Invoke(_packData[resTypeId].ResNum);
            }
        }

        // 存储数据到存档中
        private void Save()
        {
            var pack = SlotCommon._.ReadSlot().LocalPack;
            pack.Clear();
            // pack.AddRange(_packData.Values.ToList());
            _packData.Values.ForEach(delegate(ResPack resPack) { pack.Add(ResPackSave.Create(resPack)); });
            SlotCommon._.Save();
        }
    }
}