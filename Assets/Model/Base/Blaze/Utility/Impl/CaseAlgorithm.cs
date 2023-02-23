//  This file is part of Blaze - A Unity framework for FGui&UniRx Game Enhanced.
//  Copyright (c) DONOPO and contributors

//  THE COPYRIGHT OF THIS FRAMEWORK IS OWNED BY DONOPO, WHICH IS FORBIDDEN BY
//  ANY OTHER COMPANY AND BELONGS TO DONOPO'S TRADE SECRETS.

/*
| VER | AUTHOR | DATE       | DESCRIPTION              |
|
| 1.0 | tim    | 2020/02/16 | Initialize core skeleton |
*/

using System;

namespace Blaze.Utility.Impl
{
    /// <summary>
    /// 命名法则格式化
    /// </summary>
    public static class CaseAlgorithm
    {
        public enum CaseMode
        {
            PascalCase,
            CamelCase
        }

        public static string Get(string phrase, CaseMode caseMode = CaseMode.CamelCase, char delimiterChar = ' ')
        {
            // You might want to do some sanity checks here like making sure
            // there's no invalid characters, etc.

            if (string.IsNullOrEmpty(phrase)) return phrase;

            // .Split() will simply return a string[] of size 1 if no delimiter present so
            // no need to explicitly check this.
            var words = phrase.Split(delimiterChar);

            // Set first word accordingly.
            string ret = setWordCase(words[0], caseMode);

            // If there are other words, set them all to pascal case.
            if (words.Length > 1)
            {
                for (int i = 1; i < words.Length; ++i)
                    ret += setWordCase(words[i], CaseMode.PascalCase);
            }

            return ret;
        }

        private static string setWordCase(string word, CaseMode caseMode)
        {
            switch (caseMode)
            {
                case CaseMode.CamelCase:
                    return lowerFirstLetter(word);
                case CaseMode.PascalCase:
                    return capitaliseFirstLetter(word);
                default:
                    throw new NotImplementedException(
                        string.Format("Case mode '{0}' is not recognised.", caseMode.ToString()));
            }
        }

        private static string lowerFirstLetter(string word)
        {
            return char.ToLower(word[0]) + word.Substring(1);
        }

        private static string capitaliseFirstLetter(string word)
        {
            return char.ToUpper(word[0]) + word.Substring(1);
        }
    }
}