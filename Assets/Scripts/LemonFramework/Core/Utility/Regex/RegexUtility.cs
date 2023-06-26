using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace LemonFramework
{
    public static class RegexUtility
    {
        /// <summary>
        /// 获得字符串中开始和结束字符串中间值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="s">开始位置</param>
        /// <param name="e">结束位置</param>
        public static string GetMiddleValue(string str, string s, string e)
        {
            if (s.Contains("["))
            {
                s = s.Replace("[", @"\[");
            }
            if (s.Contains("]"))
            {
                s = s.Replace("]", @"\]");
            }
            if (e.Contains("["))
            {
                e = e.Replace("[", @"\[");
            }
            if (e.Contains("]"))
            {
                e = e.Replace("]", @"\]");
            }

            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }
    }
}
