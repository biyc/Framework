using System;
using System.Collections.Generic;
using System.Linq;
using Blaze.Common;

namespace Blaze.Manage.Csv
{
    /// <summary>
    /// 所有数据表的基类
    /// </summary>
    /// <typeparam name="ROW_TYPE"></typeparam>
    public abstract class CsvBaseTable<ROW_TYPE> : ICsvBaseTable
    {
        protected List<ROW_TYPE> _content = new List<ROW_TYPE>();
        protected Dictionary<string, ROW_TYPE> _unidContent = new Dictionary<string, ROW_TYPE>();
        protected Dictionary<string, List<ROW_TYPE>> _unidContents = new Dictionary<string, List<ROW_TYPE>>();
        private List<CsvRowDesc> _csvHeadDesc;
        private bool _isLoad = false;
        protected int _SN = 0;

        /// <summary>
        /// 设置表的序列号
        /// </summary>
        /// <param name="sn"></param>
        public void SetSN(int sn)
        {
            _SN = sn;
        }

        /// <summary>
        /// 激活加载数据
        /// </summary>
        public void Active()
        {
            CheckLoad();
        }


        /// <summary>
        ///  获取CSV头描述信息
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public List<CsvRowDesc> GetCsvHeadDesc()
        {
            if (_csvHeadDesc == null)
            {
                _csvHeadDesc = CsvReader.GetHead(GetCsvName());
            }

            return _csvHeadDesc;
        }


        private void CheckLoad()
        {
            if (DefaultDebug.DisableCsvDataCache)
            {
                _isLoad = false;
                _content.Clear();
                _unidContent.Clear();
                _unidContents.Clear();
            }

            // 检查是否加载数据，没有加载则加载数据
            if (!_isLoad)
            {
                // var list = CsvReader.ToListWithDeleteFirstLines(GetCsv(GetCsvName()), 3);
                var list = CsvReader.GetBody(GetCsvName());
                if (list != null)
                {
                    // 对原始数据直接进行处理（全局替换字符串等）
                    var data = PreLoad(list);
                    // 加载数据到实体类中
                    Load(data);
                    _isLoad = true;
                }
            }
        }

        /// <summary>
        /// 初始化 unid 索引
        /// </summary>
        /// <param name="unid"></param>
        /// <param name="row"></param>
        public void InitRowByUnid(string unid, ROW_TYPE row)
        {
            // 数据装载到单行快速索引中
            if (!_unidContent.ContainsKey(unid))
                _unidContent.Add(unid, row);

            // 数据装载到多行列表中
            List<ROW_TYPE> rows = null;
            // 取出列表
            if (!_unidContents.ContainsKey(unid))
                rows = new List<ROW_TYPE>();
            else
                rows = _unidContents[unid];
            // 添加数据
            rows.Add(row);
            _unidContents[unid] = rows;
        }

        /// <summary>
        /// 获取到所有的 uni id
        /// </summary>
        /// <returns></returns>
        public List<string> GetUnids()
        {
            CheckLoad();
            return _unidContent.Keys.ToList();
        }

        /// <summary>
        /// 获取指定行数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ROW_TYPE GetRowByUnid(String unid)
        {
            try
            {
                CheckLoad();
                return _unidContent.ContainsKey(unid.ToString()) ? _unidContent[unid] : default;
            }
            catch (Exception e)
            {
                // Tuner.Error($"配表中不存在该Unid : {unid}, 请检查。");
                return default;
            }
        }

        public ROW_TYPE GetRowByUnid(int unid)
        {
            return GetRowByUnid(unid.ToString());
        }

        public List<ROW_TYPE> GetRowsByUnid(int unid)
        {
            return GetRowsByUnid(unid.ToString());
        }

        public List<ROW_TYPE> GetRowsByUnid(String unid)
        {
            CheckLoad();
            if (_unidContents.ContainsKey(unid))
                return _unidContents[unid];
            else
                return new List<ROW_TYPE>();
        }

        /// <summary>
        /// 获取所有带子编号的数据组
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<ROW_TYPE>> GetGroups()
        {
            CheckLoad();
            return _unidContents;
        }

        /// <summary>
        /// 获取指定行数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ROW_TYPE GetRow(int index)
        {
            CheckLoad();
            return _content[index];
        }

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <returns></returns>
        public List<ROW_TYPE> GetTable()
        {
            CheckLoad();
            return _content;
        }

        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public int GetLength()
        {
            CheckLoad();
            return _content.Count;
        }

        /// <summary>
        /// 释放CSV数据
        /// </summary>
        public void Dispose()
        {
            _content.Clear();
            _unidContent.Clear();
            _isLoad = false;
        }

        public virtual List<string> PreLoad(List<string> list)
        {
            return list;
        }

        /// <summary>
        /// 加载CSV数据
        /// </summary>
        /// <param name="list"></param>
        protected abstract void Load(List<string> list);

        /// <summary>
        /// 获取CSV名
        /// </summary>
        /// <returns></returns>
        public abstract String GetCsvName();
    }
}