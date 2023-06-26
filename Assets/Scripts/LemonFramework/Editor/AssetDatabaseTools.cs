using UnityEditor;
using UnityEngine;

namespace LemonFramework
{
    public static class AssetDatabaseTools
    {
        /// <summary>
        /// 检查资源是否存在
        /// </summary>
        /// <returns></returns>
        public static bool AssetExistence (string filter, string type)
        {
            string[] guids = AssetDatabase.FindAssets (filter);
            for (int i = 0; i < guids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath (guids[i]);
                if (type.StartsWith ("."))
                {
                    if (assetPath.EndsWith (filter + type))
                    {
                        return true;
                    }
                }
                else
                {
                    Debug.LogWarning ($"错误后缀{type}");
                }
            }
            return false;
        }

        /// <summary>
        /// 查找资源GUID
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string AssetsGUID (string filter, string type)
        {
            string[] guids = AssetDatabase.FindAssets (filter);
            for (int i = 0; i < guids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath (guids[i]);
                if (type.StartsWith ("."))
                {
                    if (assetPath.EndsWith (filter + type))
                    {
                        return guids[i];
                    }
                }
                else
                {
                    Debug.LogWarning ($"错误后缀{type}");
                }
            }
            return null;
        }

        /// <summary>
        /// 查找资源路径
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string FindAssets (string filter, string type, params string[] searchInFolders)
        {
            string[] guids = null;
            if (searchInFolders != null && searchInFolders.Length > 0)
            {
                guids = AssetDatabase.FindAssets (filter, searchInFolders);
            }
            else
            {
                guids = AssetDatabase.FindAssets (filter);
            }

            for (int i = 0; i < guids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath (guids[i]);
                if (type.StartsWith ("."))
                {
                    if (assetPath.EndsWith (filter + type))
                    {
                        return assetPath;
                    }
                }
                else
                {
                    Debug.LogWarning ($"错误后缀{type}");
                }
            }
            return null;
        }

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T FindAssets<T> (string filter, string type, params string[] searchInFolders) where T : UnityEngine.Object
        {
            var assetPath = FindAssets (filter, type, searchInFolders);
            return AssetDatabase.LoadAssetAtPath<T> (assetPath);
        }
    }

}