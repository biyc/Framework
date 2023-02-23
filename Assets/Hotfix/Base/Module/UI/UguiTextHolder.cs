//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/26 | Initialize core skeleton |
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    /// <summary>
    /// 场景文本组件持有容器
    /// </summary>
    public class UguiTextHolder
    {
        /// <summary>
        /// 文本对象缓存器
        /// </summary>
        private Dictionary<string, GameObject> _textHolders = new Dictionary<string, GameObject>();


        public void AddTextComponent(string name, GameObject textComp)
        {
            _textHolders[name] = textComp;
        }

        /// <summary>
        /// 改变一个内容
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void ChangeText(string name, string value)
        {
            if (_textHolders.ContainsKey(name))
            {
                _textHolders[name].GetComponent<Text>().text = value;
            }
        }


        // 获取当前持有的对象
        public Dictionary<string, GameObject> GetFieldInfos()
        {
            return _textHolders;
        }

    }
}