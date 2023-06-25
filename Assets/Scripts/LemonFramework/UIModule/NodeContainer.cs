using System;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    /// <summary>
    /// 节点容器
    /// </summary>
    public static class NodeContainer
    {
        /// <summary>
        /// tag搜寻节点字典
        /// </summary>
        private static Dictionary<string, Transform> _tagDic;

        /// <summary>
        /// 名字搜寻节点字典
        /// </summary>
        private static Dictionary<string, Transform> _nameDic;

        static NodeContainer ()
        {
            _tagDic = new Dictionary<string, Transform> ();
            _nameDic = new Dictionary<string, Transform> ();
        }

        /// <summary>
        /// 通过名字找节点
        /// </summary>
        public static Transform FindNodeWithName (string name)
        {
            Transform _transform = null;
            Dictionary<string, Transform> strs = _nameDic;
            if (strs != null)
            {
                strs.TryGetValue (name, out _transform);
            }
            if (_transform == null)
            {
                _transform = GameObject.Find (name).transform;
                _nameDic[name] = _transform;
            }
            return _transform;
        }

        /// <summary>
        /// 通过tag找节点
        /// </summary>
        public static Transform FindNodeWithTag (string tag)
        {
            Transform _transform = null;
            Dictionary<string, Transform> strs = _tagDic;
            if (strs != null)
            {
                strs.TryGetValue (tag, out _transform);
            }
            if (_transform == null)
            {
                _transform = GameObject.FindWithTag (tag).transform;
                _tagDic[tag] = _transform;
            }
            return _transform;
        }
    }
}