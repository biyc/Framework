//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/18 | Initialize core skeleton |
*/

using System;

namespace Blaze.Utility.Helper
{
    public static class TimeHelper
    {
        // UTC 0 初始时间 对应的  Ticks 点，Ticks = 621355968000000000   1 ms = 10000 ticks
        private static readonly long Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;


        /// <summary>
        /// 获取当前世纪流失毫秒数
        /// </summary>
        /// <returns></returns>
        public static long CurrentMillis()
        {
            var serverTime = NetTime._.GetTime();
            if (DateTime.MinValue == serverTime)
            {
                throw new Exception("server time error");
            }

            // var serverTime = DateTime.Now;

            return DateTimeToUtc(serverTime);
        }

        /// <summary>
        /// 获取当前世纪流失秒数
        /// </summary>
        /// <returns></returns>
        public static long CurrentSecond()
        {
            var serverTime = NetTime._.GetTime();
            if (DateTime.MinValue == serverTime)
            {
                throw new Exception("server time error");
            }

            // var serverTime = DateTime.Now;

            return new DateTimeOffset(serverTime).ToUnixTimeSeconds();
        }

        /// <summary>
        /// 当前时间(服务器时间)
        /// </summary>
        /// <returns></returns>
        public static DateTime Now()
        {
            return UtcToDateTime(CurrentMillis());
        }

        /// <summary>
        /// 相差天数（自然天）
        /// </summary>
        /// <returns></returns>
        public static int DiffDays(DateTime startDate, DateTime endDate)
        {
            return Math.Abs(endDate.Date.Subtract(startDate.Date).Days);
        }


        /// <summary>
        /// UTC时间戳转换为日期类,传入时间为毫秒
        /// </summary>
        /// <param name="utcTime">日期类</param>
        ///。./// <returns></returns>
        public static DateTime UtcToDateTime(long utcTime)
        {
            // 根据 UTC 时间戳生成确定的 DateTime，
            // ToLocalTime 根据时区转换为本地显示时间
            return new DateTime(utcTime * 10000 + Epoch, DateTimeKind.Utc).ToLocalTime();
        }

        /// <summary>
        /// 日期类转换为时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DateTimeToUtc(DateTime dateTime)
        {
            return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
        }


        /// <summary>
        /// 通过本地时间算出的事件戳，不走服务器
        /// 用于高性能并且对奖励没有要求的地方
        /// 或者程序快速计算时间差的时候
        /// </summary>
        /// <returns>utc 毫秒数</returns>
        public static long ClientNow()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        }


        /// <summary>
        /// 通过本地时间算出的事件戳，不走服务器
        /// 用于高性能并且对奖励没有要求的地方
        /// 或者程序快速计算时间差的时候
        /// </summary>
        /// <returns>utc 秒数</returns>
        public static long ClientNowSeconds()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        }

        /// <summary>
        /// 1分钟(S)
        /// </summary>
        public static readonly long DEF_1MIN = 60;

        /// <summary>
        /// 1小时(S)
        /// </summary>
        public static readonly long DEF_1H = 3600;

        /// <summary>
        /// 24小时(S)
        /// </summary>
        public static readonly long DEF_24H = 86400;


        /// <summary>
        /// 1分钟(毫秒)
        /// </summary>
        public static readonly long DEF_MIN_MILLI = 60000;
        /// <summary>
        /// 1小时(毫秒)
        /// </summary>
        public static readonly long DEF_1H_MILLI = 3600000;

        /// <summary>
        /// 24小时(毫秒)
        /// </summary>
        public static readonly long DEF_24H_MILLI = 86400000;

        /// <summary>
        /// 一周(毫秒)
        /// </summary>
        public static readonly long DEF_WEEK_MILLI = 604800000;

        public static string ParsTime(int times)
        {
            if (times > 0 && times < 60)
            {
                if (times >= 10)
                    return "00:" + times;
                else
                    return "00:" + "0" + times;
            }

            if (times >= 60 && times < 3600)
            {
                int _m = (int) (times / 60);
                int _s = times % 60;
                string m = "";
                string s = "";

                if (_m >= 10)
                    m = _m.ToString();
                else
                    m = "0" + _m.ToString();

                if (_s >= 10)
                    s = _s.ToString();
                else
                    s = "0" + _s.ToString();

                // return "00:" + m + ":" + s;
                return m + ":" + s;
            }

            if (times >= 3600)
            {
                int _h = (int) (times / 3600);
                int _m = (int) ((times - 3600 * _h) / 60);
                int _s = (int) ((times - 360 * _h) % 60);

                string h = "";
                string m = "";
                string s = "";

                if (_h >= 10)
                    h = _h.ToString();
                else
                    h = "0" + _h.ToString();

                if (_m >= 10)
                    m = _m.ToString();
                else
                    m = "0" + _m.ToString();

                if (_s >= 10)
                    s = _s.ToString();
                else
                    s = "0" + _s.ToString();

                return h + ":" + m + ":" + s;
            }

            return "00:00:00";
        }
    }
}