using System;

namespace LemonFramework.ResModule
{
    /// <summary>
    /// AB包加载接口
    /// </summary>
    public interface IResourceManager
    {
        /// <summary>
        /// 同步获取资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="type">资源类型</param>
        /// <returns></returns>
        ILemonAsset LoadAsset (string path, Type type);

        /// <summary>
        /// 异步获取资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="type">资源类型</param>
        /// <param name="callback">加载回掉</param>
        /// <returns></returns>
        ILemonAsset LoadAssetAsync (string path, Type type, Action<ILemonAsset> callback);

        /// <summary>
        /// 异步获取资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="type">资源类型</param>
        /// <param name="callback">加载回掉</param>
        /// <param name="progressCallback">加载进度回掉</param>
        ILemonAsset LoadAssetAsync (string path, Type type, Action<ILemonAsset> callback, Action<float> progressCallback = null);

        /// <summary>
        /// 卸载
        /// </summary>
        void Unload (ILemonAsset asset);
    }
}