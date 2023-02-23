using System;
using System.Collections.Generic;
using System.Linq;
using ETHotfix;
using Random = UnityEngine.Random;

namespace Blaze.Utility.Base
{
    /// <summary>
    /// 抽奖器中的自定义小球
    /// </summary>
    public class BingoBallCustom : BingoBall
    {
        public BingoBallCustom()
        {
        }

        public BingoBallCustom(string Value, int Weights)
        {
            this.Value = Value;
            this.Weights = Weights;
        }

        /// <summary>
        /// 自定义奖品
        /// </summary>
        public string Value;
    }


    /// <summary>
    /// 抽奖器中的小球
    /// </summary>
    public class BingoBall
    {
        public BingoBall()
        {
        }

        public BingoBall(int Id, int Weights)
        {
            this.Id = Id;
            this.Weights = Weights;
        }


        /// <summary>
        /// 奖品 ID
        /// </summary>
        public int Id;

        /// <summary>
        /// 权重,
        /// </summary>
        public int Weights;
    }

    public class BingoBallData<DATA_TYPE> : BingoBall
    {
        public DATA_TYPE Data;

        public BingoBallData(int id, int weights, DATA_TYPE data)
        {
            this.Id = id;
            this.Weights = weights;
            this.Data = data;
        }
    }

    /// <summary>
    /// 抽奖器
    /// </summary>
    public class BingoCage<T> where T : BingoBall
    {
        // 最大概率的球
        private T _max;

        // 所有的奖品小球
        private List<T> _balls = new List<T>();

        private int _sumWeights = 0;

        /// <summary>
        /// 向抽奖器中放入小球
        /// </summary>
        /// <param name="ball"></param>
        public void Add(T ball)
        {
            if (_max == null || _max.Weights < ball.Weights)
                _max = ball;
            _balls.Add(ball);
        }

        /// <summary>
        /// 小球放入完毕
        /// </summary>
        public void AddFinish()
        {
            _balls.Sort(delegate(T b1, T b2) { return b1.Weights < b2.Weights ? -1 : 1; });
            //_sumWeights = _balls.Sum(x => x.Weights);
            _sumWeights = 0;
            foreach (var v in _balls)
            {
                _sumWeights += v.Weights;
            }
        }

        /// <summary>
        /// 清除摇奖器中的将球
        /// </summary>
        public void Clean()
        {
            _max = null;
            _sumWeights = 0;
            _balls.Clear();
        }


        /// <summary>
        /// 抽奖器是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return _balls.Count == 0;
        }

        /// <summary>
        /// 抽奖
        /// </summary>
        /// <returns></returns>
        public T Draw()
        {
            AddFinish();

            var num = Random.Range(1, _sumWeights + 1);
            float cur = 0;
            float last = 0;
            foreach (T ball in _balls)
            {
                last = cur;
                cur += ball.Weights;
                if (last < num && num <= cur)
                    return ball;
            }

            // 扔到未定义区域，按照最大概率的奖品奖励
            return _max;
        }


        /// <summary>
        /// 抽取指定多个球(有过滤的抽取)
        /// </summary>
        /// <param name="ballNum"></param>
        /// <param name="filter">每次抽奖后所执行的条件过滤</param>
        /// <returns></returns>
        public List<T> Draw(int ballNum, Action<T, List<T>> filter = null)
        {
            var balls = new List<T>();

            for (int i = 0; i < ballNum; i++)
            {
                // 抽奖前先进行必要的排序与总值计算
                AddFinish();
                var num = Random.Range(1, _sumWeights + 1);
                float cur = 0;
                float last = 0;
                foreach (T ball in _balls)
                {
                    last = cur;
                    cur += ball.Weights;
                    if (last < num && num <= cur)
                    {
                        // 放入结果集
                        balls.Add(ball);
                        // 从奖箱中移除小球，继续抽奖
                        _balls.Remove(ball);
                        filter?.Invoke(ball, _balls);
                        break;
                    }
                }
            }

            return balls;
        }

        /// <summary>
        /// 抽奖10000次，输出概率
        /// </summary>
        public static void Demo()
        {
            Dictionary<int, int> temp = new Dictionary<int, int>();
            var bingoCage = new BingoCage<BingoBall>();
            bingoCage.Add(new BingoBall(1, 19));
            bingoCage.Add(new BingoBall(2, 3));
            bingoCage.Add(new BingoBall(3, 5));
            bingoCage.Add(new BingoBall(4, 6));
            bingoCage.Add(new BingoBall(5, 9));
            bingoCage.AddFinish();
            for (int i = 0; i < 10000; i++)
            {
                var num = bingoCage.Draw().Id;
                if (temp.ContainsKey(num))
                {
                    temp[num]++;
                }
                else
                {
                    temp[num] = 1;
                }
            }

            foreach (KeyValuePair<int, int> pair in temp)
            {
                Tuner.Log(pair.Key + "  " + (pair.Value / 10000f));
            }
        }
    }
}