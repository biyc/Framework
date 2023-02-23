using System.Collections.Generic;
using UnityEngine;

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

        public BingoBallCustom(string Value, float Probability)
        {
            this.Value = Value;
            this.Probability = Probability;
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

        public BingoBall(int Id, float Probability)
        {
            this.Id = Id;
            this.Probability = Probability;
        }


        /// <summary>
        /// 奖品 ID
        /// </summary>
        public int Id;

        /// <summary>
        /// 概率 (0-1) 直接的小数，最多5位小数，例如：0.00001
        /// </summary>
        public float Probability;
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

        /// <summary>
        /// 向抽奖器中放入小球
        /// </summary>
        /// <param name="ball"></param>
        public void Add(T ball)
        {
            if (_max == null || _max.Probability < ball.Probability)
                _max = ball;

            _balls.Add(ball);
            _balls.Sort(delegate(T b1, T b2) { return b1.Probability < b2.Probability ? -1 : 1; });
        }

        /// <summary>
        /// 清除摇奖器中的将球
        /// </summary>
        public void Clean()
        {
            _max = null;
            _balls.Clear();
        }

        /// <summary>
        /// 抽奖
        /// </summary>
        /// <returns></returns>
        public T Draw()
        {
            var num = Random.Range(1, 10001) / 10000f;
            float cur = 0;
            float last = 0;
            foreach (T ball in _balls)
            {
                last = cur;
                cur += ball.Probability;
                if (last < num && num <= cur)
                    return ball;
            }

            // 扔到未定义区域，按照最大概率的奖品奖励
            return _max;
        }

        /// <summary>
        /// 抽奖10000次，输出概率
        /// </summary>
        public static void Demo()
        {
            Dictionary<int, int> temp = new Dictionary<int, int>();
            var bingoCage = new BingoCage<BingoBall>();
            bingoCage.Add(new BingoBall(1, 0.01f));
            bingoCage.Add(new BingoBall(2, 0.30f));
            bingoCage.Add(new BingoBall(3, 0.40f));
            bingoCage.Add(new BingoBall(4, 0.15f));
            bingoCage.Add(new BingoBall(5, 0.20f));
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