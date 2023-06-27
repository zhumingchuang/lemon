using System;
using UnityEditor.VersionControl;
using LemonFramework.ResModule;

namespace LemonFramework.UIModule
{
    public static class ResLoaderExtends
    {
        /// <summary>
        /// 生成默认资源信息
        /// </summary>
        private static void GenerateDefaultResInfo (IResLoader resLoader, ref string abPath)
        {
            if (string.IsNullOrEmpty (abPath))
            {
                abPath = string.Format ("Assets/Bundles/UI/{0}.prefab", resLoader.GetType ().Name.ToString ()/*.ToLower()*/);
            }
        }

        /// <summary>
        /// 通过资源信息创建对象
        /// </summary>
        public static void GetObjByResInfo (this IResLoader resLoader, Action<string, Type, ILemonAsset> callback = null, Action<float> progressCallback = null, ResLoadParam defaultParam = null)
        {
            bool async = false;
            ILemonAsset asset = null;
            string abPath = string.Empty;
            Type assetType = null;
            if (resLoader != null)
            {
                ParseResInfo (resLoader, ref abPath, ref assetType, ref async, defaultParam);
                if (!async)
                {
                    IResourceManager resourceManager = resLoader.ResourceManager;
                    if (resourceManager != null)
                    {
                        asset = resourceManager.LoadAsset (abPath, assetType);
                    }
                    else
                    {
                        asset = null;
                    }
                }
                else
                {
                    IResourceManager assetBundleLoader1 = resLoader.ResourceManager;
                    if (assetBundleLoader1 != null)
                    {
                        assetBundleLoader1.LoadAssetAsync (abPath, assetType, (ILemonAsset assetObj) =>
                        {
                            Action<string, Type, ILemonAsset> action = callback;
                            if (action == null)
                            {
                                return;
                            }
                            action (abPath, assetType, assetObj);
                        }, progressCallback);
                    }
                }
            }
            if (!async)
            {
                Action<string, Type, ILemonAsset> action1 = callback;
                if (action1 == null)
                {
                    return;
                }
                action1 (abPath, assetType, asset);
            }
        }

        /// <summary>
        /// 解析资源信息
        /// </summary>
        private static void ParseResInfo (IResLoader resLoader, ref string abPath, ref Type type, ref bool async, ResLoadParam defaultParam = null)
        {
            if (defaultParam != null)
            {
                async = defaultParam.@async;
                type = defaultParam.type;
            }
            object[] customAttributes = resLoader.GetType ().GetCustomAttributes (typeof (ResInfoAttribute), true);
            if (customAttributes != null)
            {
                object[] objArray = customAttributes;
                for (int i = 0; i < objArray.Length; i++)
                {
                    ResInfoAttribute resInfoAttribute = (ResInfoAttribute)objArray[i];
                    if (resInfoAttribute != null)
                    {
                        abPath = resInfoAttribute.abPath;
                        type = resInfoAttribute.type;
                        async = resInfoAttribute.@async;
                    }
                }
            }
            GenerateDefaultResInfo (resLoader, ref abPath);
        }
    }
}