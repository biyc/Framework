//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/04/01 | Initialize core skeleton |
*/

using System;

namespace Blaze.Manage.Csv
{
    /// <summary>
    /// CsvBean的注解,自动注入侦测, Parent 不为空时表示重载父类
    /// </summary>
    public class CsvTable : Attribute
    {
        private String _value = "";

        public CsvTable()
        {
        }

        public CsvTable(String value)
        {
            this._value = value;
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private bool _extend = false;

        public bool Extend
        {
            get { return _extend; }
            set { _extend = value; }
        }
    }
}