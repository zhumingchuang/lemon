using System;
using System.IO;
using UnityEngine;
namespace LemonFramework.Editor
{
    public static partial class EditorDefine
    {

        #region 菜单字符串
        /// <summary>
        /// 程序集创建
        /// </summary>
        public const string CREATEASSEMBLY = "CNFramework/AssemblySettings #A";
        /// <summary>
        /// Build设置
        /// </summary>
        public const string BUILDGROUP = "CNFramework/BuildGroup #S";
        /// <summary>
        /// Build设置
        /// </summary>
        public const string BUILDPROJECTSETINGS = "CNFramework/BuildProjectSetings #B";
        /// <summary>
        /// 创建MVP文件
        /// </summary>
        public const string CREATE_MVP = "CNFramework/UI/CreateMVP #M";
        /// <summary>
        /// ILRuntime绑定
        /// </summary>
        public const string GENERATE_CLR_BINDING_CODE_BY_ANALYSIS = "CNFramework/ILRuntime/Generate CLR Binding Code by Analysis";
        /// <summary>
        /// 宏定义
        /// </summary>
        public const string MACRODEFINE = "CNFramework/MacroDefine";
        /// <summary>
        /// AB菜单按钮打包界面
        /// </summary>
        public const string ASSETSBUNDLE_GROUPS1 = "CNFramework/AssetsBundle/Groups";
        /// <summary>
        /// Assets右键
        /// </summary>
        public const string ASSETSBUNDLE_GROUPS2 = "Assets/CNFramework/AssetsBundle/Groups";
        /// <summary>
        /// 菜单创建清单
        /// </summary>
        public const string ASSETSBUNDLE_CREATEMANIFEST1 = "CNFramework/AssetsBundle/CreateManifest";
        /// <summary>
        /// Asset右键创建清单
        /// </summary>
        public const string ASSETSBUNDLE_CREATEMANIFEST2 = "Assets/CNFramework/AssetsBundle/CreateManifest";
        /// <summary>
        /// Asset右键添加资源
        /// </summary>
        public const string ASSETSBUNDLE_ADDASSETBUNDLE = "Assets/CNFramework/AssetsBundle/";
        /// <summary>
        /// 打包分组
        /// </summary>
        public const string ASSETSBUNDLE_BUILDGROUPS = "CNFramework/AssetsBundle/Build/Groups";
        /// <summary>
        /// 计算选中资源的 CRC
        /// </summary>
        public const string ASSETSBUNDLE_COMPUTECRC = "CNFramework/AssetsBundle/Compute CRC";
        /// <summary>
        /// 打包资源
        /// </summary>
        public const string ASSETSBUNDLE_BUILDBUNDLES = "CNFramework/AssetsBundle/Build/Bundles";
        /// <summary>
        /// 打包清单
        /// </summary>
        public const string ASSETSBUNDLE_BUILDMANIFESTS = "CNFramework/AssetsBundle/Build/Manifests";
        /// <summary>
        /// 打包播放器
        /// </summary>
        public const string ASSETSBUNDLE_BUILDPLAYER = "CNFramework/AssetsBundle/Build/Player";
        /// <summary>
        /// 资源复制到StreamingAssets
        /// </summary>
        public const string ASSETSBUNDLE_COPYTOSTREAMINGASSETS1 = "CNFramework/AssetsBundle/Copy To StreamingAssets";
        /// <summary>
        /// 资源复制到StreamingAssets
        /// </summary>
        public const string ASSETSBUNDLE_COPYTOSTREAMINGASSETS2 = "Assets/CNFramework/AssetsBundle/Build/Copy To StreamingAssets";
        /// <summary>
        /// 清理所有数据
        /// </summary>
        public const string ASSETSBUNDLE_CLEAR = "CNFramework/AssetsBundle/Build/Clear";
        /// <summary>
        /// 清除历史
        /// </summary>
        public const string ASSETSBUNDLE_CLEARHISTORY = "CNFramework/AssetsBundle/Build/Clear History";
        /// <summary>
        /// AB设置
        /// </summary>
        public const string ASSETSBUNDLE_VIEWSETTINGS1 = "CNFramework/AssetsBundle/View/Settings";
        /// <summary>
        /// AB设置
        /// </summary>
        public const string ASSETSBUNDLE_VIEWSETTINGS2 = "Assets/CNFramework/AssetsBundle/Settings";
        /// <summary>
        /// 清除进度
        /// </summary>
        public const string ASSETSBUNDLE_CLEARPROGRESSBAR = "CNFramework/AssetsBundle/Clear Progress Bar";
        /// <summary>
        /// 查看打包后的资源
        /// </summary>
        public const string ASSETSBUNDLE_VIEWBUILDPATH1 = "CNFramework/AssetsBundle/View/Build Path";
        /// <summary>
        /// 右键查看打包后的资源
        /// </summary>
        public const string ASSETSBUNDLE_VIEWBUILDPATH2 = "Assets/CNFramework/AssetsBundle/View/Build Path";
        /// <summary>
        /// 查看下载目录资源
        /// </summary>
        public const string ASSETSBUNDLE_VIEWDOWNLOADPATH1 = "CNFramework/AssetsBundle/View/Download Path";
        /// <summary>
        /// 右键查看下载目录资源
        /// </summary>
        public const string ASSETSBUNDLE_VIEWDOWNLOADPATH2 = "Assets/CNFramework/AssetsBundle/View/Download Path";
        /// <summary>
        /// 查看临时目录的资源
        /// </summary>
        public const string ASSETSBUNDLE_VIEWTEMPORARY1 = "CNFramework/AssetsBundle/View/Temporary";
        /// <summary>
        /// 查看临时目录的资源
        /// </summary>
        public const string ASSETSBUNDLE_VIEWTEMPORARY2 = "Assets/CNFramework/AssetsBundle/View/Temporary";
        /// <summary>
        /// ILRuntime绑定
        /// </summary>
        public const string GENERATECLRBINDINGBYANALYSIS = "CNFramework/ILRuntime/ILRuntimeCLRBinding";
        /// <summary>
        /// 生成跨域继承适配器
        /// </summary>
        public const string GENERATECROSSBINDADAPTER = "CNFramework/ILRuntime/GenerateCrossbindAdapter";
        /// <summary>
        /// 生成数据表
        /// </summary>
        public const string GENERATE_DATATABLES = "CNFramework/Generate DataTables";
        /// <summary>
        /// 禁用所有日志脚本宏定义
        /// </summary>
        public const string DISABLE_ALL_LOGS = "CNFramework/Log Scripting Define Symbols/Disable All Logs";
        /// <summary>
        /// 开启所有日志脚本宏定义
        /// </summary>
        public const string ENABLE_ALL_LOGS = "CNFramework/Log Scripting Define Symbols/Enable All Logs";
        /// <summary>
        /// 开启调试及以上级别的日志脚本宏定义
        /// </summary>
        public const string ENABLE_DEBUG_AND_ABOVE_LOGS = "CNFramework/Log Scripting Define Symbols/Enable Debug And Above Logs";
        /// <summary>
        /// 开启信息及以上级别的日志脚本宏定义
        /// </summary>
        public const string ENABLE_INFO_AND_ABOVE_LOGS = "CNFramework/Log Scripting Define Symbols/Enable Info And Above Logs";
        /// <summary>
        /// 开启警告及以上级别的日志脚本宏定义
        /// </summary>
        public const string ENABLE_WARNING_AND_ABOVE_LOGS = "CNFramework/Log Scripting Define Symbols/Enable Warning And Above Logs";
        /// <summary>
        /// 开启错误及以上级别的日志脚本宏定义
        /// </summary>
        public const string ENABLE_ERROR_AND_ABOVE_LOGS = "CNFramework/Log Scripting Define Symbols/Enable Error And Above Logs";
        /// <summary>
        /// 开启严重错误及以上级别的日志脚本宏定义
        /// </summary>
        public const string ENABLE_FATAL_AND_ABOVE_LOGS = "CNFramework/Log Scripting Define Symbols/Enable Fatal And Above Logs";
        #endregion


        #region 文件夹位置

        /// <summary>
        /// Unity 代码生成LL位置
        /// </summary>
        public const string SCRIPTASSEMBLIESDIR = "Library/ScriptAssemblies";

        /// <summary>
        /// 热更新代码存放位置
        /// </summary>
        public const string CODEDIR = "Assets/Res/Code";

        /// <summary>
        /// 配置文件prefab存放位置
        /// </summary>
        public const string BUNDLESDIR = "Assets/Bundles/Independent/";

        /// <summary>
        /// 配置文件存放位置
        /// </summary>
        public const string CONFIGDIR = "Assets/Res/Config";

        /// <summary>
        /// DLL打包路径
        /// </summary>
        public const string BuildOutputDir = "./Temp/Bin/Debug";

        /// <summary>
        /// 适配器路径
        /// </summary>
        public const string ADAPTERPATH = "Assets/CNFramework/Runtime/Mono/ILRuntime/Adaptor";

        /// <summary>
        /// 数据表存放位置
        /// </summary>
        public const string DATATABLES = "Assets/Bundles/DataTables";

        /// <summary>
        /// 数据表脚本生成位置
        /// </summary>
        public const string CSHARPCODEPATH = "Assets/CNFramework/Hotfix/DataTable";

        /// <summary>
        /// 数据表脚本模板文件位置
        /// </summary>
        public const string CSHARPCODETEMPLATEFILENAME = "Assets/CNFramework/Res/Configs/DataTableCodeTemplate.txt";


        #endregion

        #region 其他配置
        //程序集
        public const string CNFASSEMBLY = "CNFramework.Runtime";
        public const string CNFHOTFIXASSEMBLY = "CNFramework.Hotfix";
        public const string CNFEDITORMODEL = "CNFramework.Editor";
        //当前程序集key
        public const string SELECTASSEMBLY = "SELECTASSEMBLY_100";
        //当前UI程序集
        public const string SELECTUIASSEMBLY = "SELECTUIASSEMBLY_101";

        //数据表命名空间
        public const string DATA_TABLE_NAME_SPACE = "CNFramework.Hotfix.DataTable";

        #endregion

        //合并代码文件位置 key
        public const string CNFCodesPaht = "CodesPaht_102";
    }
}