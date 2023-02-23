//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using Blaze.Manage.Csv.Poco;
using Blaze.Utility;
using Blaze.Utility.Extend;
using UniRx.InternalUtil;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Blaze.Manage.Csv
{
    /// <summary>
    /// 基础类型处理器，复杂类型需直接配置解析类
    /// </summary>
    public class CsvTypeHelper
    {
        public static string ToNull(string value)
        {
            Tuner.Error("CSV 文件有格式有错误，请检查数据：" + value);
            return value;
        }


        // 去除转义
        public static string Comma(string value)
        {
            value = value.Replace("^comma", ",");
            value = value.Replace("^nline", "\n");
            value = value.Replace("^rline", "\r");
            return value;
        }

        // :Int
        public static int ToInt(string value)
        {
            value = Comma(value);
            return String.IsNullOrEmpty(value) ? 0 : value.ToInt();
        }


        // :Int
        public static BigInteger ToBigInt(string value)
        {
            value = Comma(value);
            return String.IsNullOrEmpty(value) ? BigInteger.Zero : BigInteger.Parse(value);
        }

        // :Str
        public static String ToString(string value)
        {
            value = Comma(value);
            return value;
        }

        // :Float
        public static float ToFloat(string value)
        {
            value = Comma(value);
            if (string.IsNullOrEmpty(value)) return 0;
            return value.ToFloat();
        }

        // :Bool
        public static bool ToBool(string value)
        {
            value = Comma(value);
            if (string.IsNullOrEmpty(value)) return false;
            return value.ToBool();
        }

        // :List:Int
        public static List<int> ToIntArray(string value)
        {
            value = Comma(value);
            if (String.IsNullOrEmpty(value))
            {
                return new List<int>();
            }

            // 样板数据   1|1|3
            List<int> res = new List<int>();
            foreach (string s in value.Split('|'))
            {
                res.Add(s.ToInt());
            }

            return res;
        }

        // :Dict:Int:Int
        public static Dictionary<int, int> ToIntIntDictionary(string value)
        {
            value = Comma(value);
            // 样板数据   1:2|3:4|3:4

            Dictionary<int, int> res = new Dictionary<int, int>();
            foreach (string data in value.Split('|'))
            {
                var dicValue = data.Split(':');
                res.Add(dicValue[0].ToInt(), dicValue[1].ToInt());
            }

            return res;
        }

        /// <summary>
        /// :Array:Int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int[] ToArrayInt(string value)
        {
            value = Comma(value);
            return value.ToIntArray('|');
        }

        /// <summary>
        /// :Array:Vector2
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector2[] ToArrayVector2(string value)
        {
            value = Comma(value);
            // 样板数据   1:2|3:4|5:1
            var len = value.Split('|').Length;
            Vector2[] result = new Vector2[len];
            int idx = 0;
            foreach (string data in value.Split('|'))
            {
                var dicValue = data.Split(':');
                result[idx] = new Vector2(dicValue[0].ToInt(), dicValue[1].ToInt());
                idx++;
            }

            return result;
        }

        /// <summary>
        /// :List:Array:Int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<int[]> ToIntArrayList(string value)
        {
            value = Comma(value);
            if (value == "") return new List<int[]>();
            return value.ToListIntArray();
        }

        /// <summary>
        /// :Array:Float
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float[] ToArrayFloat(string value)
        {
            value = Comma(value);
            return value.ToFloatArray('|');
        }

        /// <summary>
        /// :List:Float
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<float> ToFloatList(string value)
        {
            value = Comma(value);
            return value.ToFloatArray('|').ToList();
        }


        /// <summary>
        /// :Vec2
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector2 ToVector2(string value)
        {
            value = Comma(value);
            return value.ToVector2('|');
        }

        public static UnityEngine.Vector3 ToVector3(string value)
        {
            value = Comma(value);
            if (string.IsNullOrEmpty(value))
                return Vector3.zero;
            var parts = value.ToFloatArray(',');
            if (parts.Length >= 3)
            {
                return new Vector3(parts[0], parts[1], parts[2]);
            }
            else
            {
                return Vector3.zero;
            }
        }

        public static List<UnityEngine.Vector3> ToListVector3(string value)
        {
            value = Comma(value);
            var list = new List<UnityEngine.Vector3>();
            value.Trim().Split('|').ToList().ForEach(delegate(string input) { list.Add(ToVector3(input)); });
            return list;
        }

        /// <summary>
        /// :Color
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Color ToRGBColor(string value)
        {
            value = Comma(value);
            return value.ToRGBColor();
        }


        /// <summary>
        /// :List:Str
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<string> ToListStr(string value)
        {
            value = Comma(value);
            if (String.IsNullOrEmpty(value))
            {
                return new List<string>();
            }

            return value.Split('|').ToList();
        }

        /// <summary>
        /// :Treasure
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Treasure ToTreasure(string value)
        {
            value = Comma(value);
            if (String.IsNullOrEmpty(value))
            {
                return null;
            }

            var data = value.Split(',');

            // 1002,10,0.3
            var treasure = new Treasure();
            treasure.Id = data[0].ToInt();
            treasure.Num = data[1].ToInt();
            treasure.Probability = data[2].ToFloat();
            return treasure;
        }

        /// <summary>
        /// 枚举解析器
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ToEnum<T>(string value)
        {
            if (!string.IsNullOrEmpty(value))
                return (T) System.Enum.Parse(typeof(T), value);
            return default;
        }

        // 根据CSV中的类型描述，获取特定的类型字符串
        public static string GetType(string value)
        {
            switch (value.Trim())
            {
                case ":Int":
                    return "int";
                case "Uni:Int":
                    return "int";
                case ":Str":
                    return "string";
                case "Uni:Str":
                    return "string";
                case ":List:Float":
                    return "List<float>";
                case ":Float":
                    return "float";
                case "Uni:Float":
                    return "float";
                case ":Bool":
                    return "bool";
                case "Uni:Bool":
                    return "bool";
                case ":List:Int":
                    return "List<int>";
                case "Uni:List:Int":
                    return "List<int>";
                case ":Dict:Int:Int":
                    return "Dictionary<int, int>";
                case ":Array:Int":
                    return "int[]";
                case ":Array:Vector2":
                    return "Vector2[]";
                case ":List:Array:Int":
                    return "List<int[]>";
                case ":Array:Float":
                    return "float[]";
                case ":Vec2":
                    return "Vector2";
                case ":Vec3":
                    return "UnityEngine.Vector3";
                case ":List:Vec3":
                    return "List<UnityEngine.Vector3>";
                case ":Color":
                    return "Color";
                case ":List:Str":
                    return "List<string>";
                case ":BigInt":
                    return "BigInteger";
                case "Uni:BigInt":
                    return "BigInteger";
                case ":Treasure":
                    return "Treasure";
                default:
                {
                    // 如果是枚举类型，则提取枚举类型
                    if (value.Contains(":Enum<"))
                        return value.Trim().Replace(":Enum<", "").Replace(">", "");
                    return value.Replace(":", "");
                }
            }
        }

        public static string GetTransFunc(string value)
        {
            switch (value.Trim())
            {
                case ":Int":
                    return "ToInt";
                case "Uni:Int":
                    return "ToInt";
                case ":Str":
                    return "ToString";
                case "Uni:Str":
                    return "ToString";
                case ":Float":
                    return "ToFloat";
                case ":List:Float":
                    return "ToFloatList";
                case "Uni:Float":
                    return "ToFloat";
                case ":Bool":
                    return "ToBool";
                case "Uni:Bool":
                    return "ToBool";
                case ":List:Int":
                    return "ToIntArray";
                case "Uni:List:Int":
                    return "ToIntArray";
                case ":Dict:Int:Int":
                    return "ToIntIntDictionary";
                case "Uni:Dict:Int:Int":
                    return "ToIntIntDictionary";
                case ":Array:Int":
                    return "ToArrayInt";
                case ":Array:Vector2":
                    return "ToArrayVector2";
                case ":List:Array:Int":
                    return "ToIntArrayList";
                case ":Array:Float":
                    return "ToArrayFloat";
                case ":Vec2":
                    return "ToVector2";
                case ":Vec3":
                    return "ToVector3";
                case ":List:Vec3":
                    return "ToListVector3";
                case ":Color":
                    return "ToRGBColor";
                case ":List:Str":
                    return "ToListStr";
                case ":BigInt":
                    return "ToBigInt";
                case "Uni:BigInt":
                    return "ToBigInt";
                case ":Treasure":
                    return "ToTreasure";
                default:
                {
                    // 如果是枚举类型，则提取枚举类型
                    if (value.Contains(":Enum<"))
                        return value.Trim().Replace(":Enum", "ToEnum");
                    return "ToNull";
                }
            }
        }
    }
}