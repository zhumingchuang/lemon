using LemonFramework.ResModule;

namespace LemonFramework.UIModule
{
    /// <summary>
    /// 全局UI设置
    /// </summary>
    public static class UISetting
    {
        /// <summary>
        /// UI资源默认AB包加载器
        /// </summary>
        public static IResourceManager DefaultResourceManager;

        /// <summary>
        /// UI资源默认加载方式
        /// </summary>
        public static ResLoadParam DefaultAssetLoadParam;

        /// <summary>
        /// UI资源默认父节点参数
        /// </summary>
        public static ParentParam DefaultParentParam;

        static UISetting ()
        {
            //UISetting.DefaultResourceManager = ResourcesComponent.Instance;
            UISetting.DefaultAssetLoadParam = ResLoadParam.Default;
            UISetting.DefaultParentParam = ParentParam.Default;
        }
    }
}