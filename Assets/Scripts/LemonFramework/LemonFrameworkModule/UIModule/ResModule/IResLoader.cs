using LemonFramework.ResModule;

namespace LemonFramework.ResModule
{
    /// <summary>
    /// 资源加载接口
    /// </summary>
    public interface IResLoader
    {
        /// <summary>
        /// AB包加载
        /// </summary>
        IResourceManager ResourceManager
        {
            get;
            set;
        }
    }
}