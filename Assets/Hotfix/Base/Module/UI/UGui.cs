//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/26 | Initialize core skeleton |
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Blaze.Common;
using Blaze.Core;
using UnityEngine;

namespace ETHotfix
{
    /// <summary>
    /// FGui控制器
    /// </summary>
    public static class UGui
    {
        #region 静态工具

        /// <summary>
        /// 查找子物体（递归查找）
        /// </summary>
        /// <param name="trans">父物体</param>
        /// <param name="goName">子物体的名称</param>
        /// <returns>找到的相应子物体</returns>
        public static Transform FindChild(Transform trans, string goName)
        {
            Transform child = trans.Find(goName);
            if (child != null)
                return child;

            Transform go = null;
            for (int i = 0; i < trans.childCount; i++)
            {
                child = trans.GetChild(i);
                go = FindChild(child, goName);
                if (go != null)
                    return go;
            }

            return null;
        }

        /// <summary>
        /// 查找子物体（递归查找）  where T : UnityEngine.Object
        /// </summary>
        /// <param name="trans">父物体</param>
        /// <param name="goName">子物体的名称</param>
        /// <returns>找到的相应子物体</returns>
        public static T FindChild<T>(Transform trans, string goName) where T : UnityEngine.Object
        {
            Transform child = trans.Find(goName);
            if (child != null)
            {
                return child.GetComponent<T>();
            }

            Transform go = null;
            for (int i = 0; i < trans.childCount; i++)
            {
                child = trans.GetChild(i);
                go = FindChild(child, goName);
                if (go != null)
                {
                    return go.GetComponent<T>();
                }
            }

            return null;
        }


        /// <summary>
        /// TODO 有问题，修理后再用
        /// （父物体改变时，安全区位置有问题）
        /// 设置安全层工具
        /// </summary>
        /// <param name="trans"></param>
        public static void SafetyLayer(Transform trans)
        {
            // 处理页面里面的安全区层
            var safe = UGui.FindChild(trans, "SafetyLayer");
            if (safe != null)
            {
                var ScaleFactor = UIComponent.ScaleFactor;

                var area = Screen.safeArea;
                // 锚点（左下）
                safe.Comp<RectTransform>().anchorMin = new Vector2(0f, 0f);
                safe.Comp<RectTransform>().anchorMax = new Vector2(0f, 0f);
                // 安全区大小
                safe.Comp<RectTransform>().SetSize(new Vector2(area.width * ScaleFactor, area.height * ScaleFactor));

                // 设置中心点位置
                safe.Comp<RectTransform>().anchoredPosition =
                    new Vector2(area.center.x * ScaleFactor, area.center.y * ScaleFactor);


                // Debug.Log("position " + area.position);
                // Debug.Log("size " + area.size);
                // Debug.Log("center " + area.center);
                // Debug.Log("min " + area.min);
                // Debug.Log("max " + area.max);
                // Debug.Log("ScaleFactor " + ScaleFactor);
            }
        }

        #endregion
    }
}