using System;
using UnityEngine;

namespace LemonFramework
{
    public static class GameObjectExpand
    {
        public static T Get<T> (this GameObject gameObject, string key) where T : class
        {
            try
            {
                return gameObject.GetComponent<ReferenceCollector> ().Get<T> (key);
            }
            catch (Exception e)
            {
                throw new Exception ($"获取{gameObject.name}的ReferenceCollector Get Type {typeof (T)} error, key: {key}", e);
            }
        }
    }
}