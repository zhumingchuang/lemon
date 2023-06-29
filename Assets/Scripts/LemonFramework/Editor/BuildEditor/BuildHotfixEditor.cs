using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditorInternal;
using UnityEngine;

namespace LemonFramework.Editor
{
    [InitializeOnLoad]
    public class Startup
    {
        //代码后缀
        private const string DllSuffix = ".dll";
        private const string PadSuffix = ".pdb";

        //热更新代码后缀
        private const string CodeSuffix = ".dll.bytes";
        private const string CodePdbSuffix = ".pdb.bytes";

        //代码预制体
        private const string CodePrefab = "Code";

        static int CompilationIndex;

        static List<string> assemblyStartedCompilation = new List<string>();

        static Startup()
        {
            //CompilationPipeline.assemblyCompilationStarted += AssemblyCompilationStarted;
            //CompilationPipeline.assemblyCompilationFinished += AssemblyCompilationFinished;
            //CopyCode();
            //CheckConfig();
        }

        private static List<string> hotfixCodePath=new List<string>();
        public static List<string> HotfixCodePath
        {
            get
            {
                hotfixCodePath.Clear();
                var selectAssembly = EditorPrefs.GetString(EditorDefine.SELECTASSEMBLY).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < selectAssembly.Length; i++)
                {
                    if (AssemblyTools.CheckAssemblyChildren(selectAssembly[i], EditorDefine.CNFHOTFIXASSEMBLY))
                    {
                        string assetPath = AssetDatabaseTools.FindAssets(selectAssembly[i], ".asmdef");
                        hotfixCodePath.Add(assetPath);
                    }
                }
                return hotfixCodePath;
            }
        }

        private static List<string> codePath=new List<string>();
        public static List<string> CodePath
        {
            get
            {
                codePath.Clear();
                var selectAssembly = EditorPrefs.GetString(EditorDefine.SELECTASSEMBLY).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < selectAssembly.Length; i++)
                {
                    if (AssemblyTools.CheckAssemblyChildren(selectAssembly[i], EditorDefine.CNFASSEMBLY))
                    {
                        if (!AssemblyTools.CheckAssemblyChildren(selectAssembly[i], EditorDefine.CNFHOTFIXASSEMBLY))
                        {
                            string assetPath = AssetDatabaseTools.FindAssets(selectAssembly[i], ".asmdef");
                            codePath.Add(assetPath);
                        }
                    }
                }
                return codePath;
            }
        }

        [MenuItem("LemonFramework/BuildCodeDebug _F5")]
        public static void BuildCodeDebug()
        {
            for (int i = 0; i < HotfixCodePath.Count; i++)
            {
                Debug.Log(HotfixCodePath[i]);
            }
        }

        [MenuItem("LemonFramework/BuildCodeRelease _F6")]
        public static void BuildCodeRelease()
        {


        }

        [MenuItem("LemonFramework/BuildData _F7")]
        public static void BuildData()
        {

        }
        [MenuItem("LemonFramework/BuildLogic _F8")]
        public static void BuildLogic()
        {

        }



#if UNITY_2020_1_OR_NEWER
        private static void BuildMuteAssembly(string assemblyName, string[] CodeDirectorys, string[] additionalReferences, CodeOptimization codeOptimization)
        {

        }
#else
        private static void BuildMuteAssembly(string assemblyName, string[] CodeDirectorys, string[] additionalReferences/*, CodeOptimization codeOptimization*/)
        {
            List<string> scripts = new List<string>();
            for (int i = 0; i < CodeDirectorys.Length; i++)
            {
                DirectoryInfo dti = new DirectoryInfo(CodeDirectorys[i]);
                FileInfo[] fileInfos = dti.GetFiles("*.cs", System.IO.SearchOption.AllDirectories);
                for (int j = 0; j < fileInfos.Length; j++)
                {
                    scripts.Add(fileInfos[j].FullName);
                }
            }

            string dllPath = Path.Combine(EditorStrDef.BuildOutputDir, $"{assemblyName}.dll");
            string pdbPath = Path.Combine(EditorStrDef.BuildOutputDir, $"{assemblyName}.pdb");
            File.Delete(dllPath);
            File.Delete(pdbPath);

            Directory.CreateDirectory(EditorStrDef.BuildOutputDir);

            AssemblyBuilder assemblyBuilder = new AssemblyBuilder(dllPath, scripts.ToArray());

            //启用UnSafe
            //assemblyBuilder.compilerOptions.AllowUnsafeCode = true;

            BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);

            //assemblyBuilder.compilerOptions.CodeOptimization = codeOptimization;
            assemblyBuilder.compilerOptions.ApiCompatibilityLevel = PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup);
            // assemblyBuilder.compilerOptions.ApiCompatibilityLevel = ApiCompatibilityLevel.NET_4_6;

            assemblyBuilder.additionalReferences = additionalReferences;

            assemblyBuilder.flags = AssemblyBuilderFlags.None;
            //AssemblyBuilderFlags.None                 正常发布
            //AssemblyBuilderFlags.DevelopmentBuild     开发模式打包
            //AssemblyBuilderFlags.EditorAssembly       编辑器状态
            assemblyBuilder.referencesOptions = ReferencesOptions.UseEngineModules;

            assemblyBuilder.buildTarget = EditorUserBuildSettings.activeBuildTarget;

            assemblyBuilder.buildTargetGroup = buildTargetGroup;

            assemblyBuilder.buildStarted += delegate (string assemblyPath) { Debug.LogFormat("build start：" + assemblyPath); };

            assemblyBuilder.buildFinished += delegate (string assemblyPath, CompilerMessage[] compilerMessages)
            {
                int errorCount = compilerMessages.Count(m => m.type == CompilerMessageType.Error);
                int warningCount = compilerMessages.Count(m => m.type == CompilerMessageType.Warning);

                Debug.LogFormat("Warnings: {0} - Errors: {1}", warningCount, errorCount);

                if (warningCount > 0)
                {
                    Debug.LogFormat("有{0}个Warning!!!", warningCount);
                }

                if (errorCount > 0)
                {
                    for (int i = 0; i < compilerMessages.Length; i++)
                    {
                        if (compilerMessages[i].type == CompilerMessageType.Error)
                        {
                            Debug.LogError(compilerMessages[i].message);
                        }
                    }
                }
            };

            //开始构建
            if (!assemblyBuilder.Build())
            {
                Debug.LogErrorFormat("build fail：" + assemblyBuilder.assemblyPath);
                return;
            }
        }

#endif

        private static void AfterCompiling()
        {
            while (EditorApplication.isCompiling)
            {
                Debug.Log("Compiling wait1");
                // 主线程sleep并不影响编译线程
                Thread.Sleep(1000);
                Debug.Log("Compiling wait2");
            }

            Debug.Log("Compiling finish");

            //Directory.CreateDirectory(CodeDir);
            File.Copy(Path.Combine(EditorDefine.BuildOutputDir, "Code.dll"), Path.Combine(EditorDefine.CODEDIR, "Code.dll.bytes"), true);
            File.Copy(Path.Combine(EditorDefine.BuildOutputDir, "Code.pdb"), Path.Combine(EditorDefine.CODEDIR, "Code.pdb.bytes"), true);
            AssetDatabase.Refresh();
            Debug.Log("copy Code.dll to Bundles/Code success!");

            // 设置ab包
            AssetImporter assetImporter1 = AssetImporter.GetAtPath("Assets/Bundles/Code/Code.dll.bytes");
            assetImporter1.assetBundleName = "Code.unity3d";
            AssetImporter assetImporter2 = AssetImporter.GetAtPath("Assets/Bundles/Code/Code.pdb.bytes");
            assetImporter2.assetBundleName = "Code.unity3d";
            AssetDatabase.Refresh();
            Debug.Log("set assetbundle success!");

            Debug.Log("build success!");
        }




        private static void AssemblyCompilationStarted(string dll)
        {
            assemblyStartedCompilation.Add(System.IO.Path.GetFileNameWithoutExtension(dll));
        }

        private static void AssemblyCompilationFinished(string dll, CompilerMessage[] message)
        {
            var err = message.Any((type) => { return type.type == CompilerMessageType.Error; });
            if (!err)
            {
                CompilationIndex++;
                if (CompilationIndex == assemblyStartedCompilation.Count)
                {
                    var dllName = GetHotfixAssembly();
                    for (int i = 0; i < dllName.Count; i++)
                    {
                        if (assemblyStartedCompilation.Contains(dllName[i]))
                        {
                            string fullPath = System.IO.Path.Combine(System.IO.Path.GetFullPath(EditorDefine.CODEDIR), dllName[i] + ".dll.bytes");
                            string[] assembly = { EditorDefine.CNFHOTFIXASSEMBLY, dllName[i] };
                            if (!CompileAssembly.MergeDll(fullPath, assembly))
                            {
                                return;
                            }
                        }
                    }

                    DirectoryInfo folder = new DirectoryInfo(EditorDefine.CODEDIR);
                    foreach (FileInfo file in folder.GetFiles("*.dll.pdb"))
                    {
                        var newPath = file.FullName.Replace(".dll.pdb", ".pdb.bytes");
                        if (File.Exists(newPath))
                        {
                            File.Delete(newPath);
                        }
                        file.MoveTo(newPath);
                    }
                    CheckCodePrefab(dllName);
                }
            }
        }

        private static void CheckCodePrefab(List<string> objs)
        {
            if (!Directory.Exists(EditorDefine.BUNDLESDIR))
            {
                Directory.CreateDirectory(EditorDefine.BUNDLESDIR);
            }
            string path = $"{EditorDefine.BUNDLESDIR}{CodePrefab}.prefab";
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null)
            {
                var tempPrefab = new GameObject(CodePrefab);
                tempPrefab.AddComponent<ReferenceCollector>();
                prefab = PrefabUtility.SaveAsPrefabAsset(tempPrefab, path, out bool sucess);
                UnityEngine.Object.DestroyImmediate(tempPrefab);
                AssetDatabase.Refresh();
            }
            EditorGUIUtility.PingObject(prefab);
            Selection.activeObject = prefab;
            if (prefab)
            {
                ReferenceCollector rc = prefab.GetComponent<ReferenceCollector>();
                bool CheckNull(string type)
                {
                    bool sign = false;
                    for (int i = 0; i < objs.Count; i++)
                    {
                        UnityEngine.Object obj = prefab.Get<UnityEngine.Object>(objs[i] + type);
                        if (obj == null)
                        {
                            var codeSuffix = type == DllSuffix ? CodeSuffix : CodePdbSuffix;
                            string objPath = Path.Combine(EditorDefine.CODEDIR, objs[i] + codeSuffix);
                            var asset = AssetDatabase.LoadAssetAtPath(objPath, typeof(UnityEngine.Object));
                            rc.Add(objs[i] + type, asset);
                            sign = true;
                        }
                    }
                    return sign;
                }

                bool checkDll = CheckNull(DllSuffix);
                bool checkPdb = CheckNull(PadSuffix);
                if (checkDll || checkPdb)
                {
                    EditorUtility.SetDirty(prefab);
                    AssetDatabase.Refresh();
                }
            }
        }

        /// <summary>
        /// 获取所有热更新程序集
        /// </summary>
        /// <returns></returns>
        public static List<string> GetHotfixAssembly()
        {
            if (!Directory.Exists(EditorDefine.CODEDIR)) Directory.CreateDirectory(EditorDefine.CODEDIR);
            var hotfixGuids = AssetDatabase.FindAssets(EditorDefine.CNFHOTFIXASSEMBLY);
            string HotfixGuid = string.Empty;
            for (int i = 0; i < hotfixGuids.Length; i++)
            {
                var tempHotfixPath = AssetDatabase.GUIDToAssetPath(hotfixGuids[i]);
                if (tempHotfixPath.EndsWith($"{EditorDefine.CNFHOTFIXASSEMBLY}.asmdef"))
                {
                    HotfixGuid = hotfixGuids[i];
                    break;
                }
            }

            if (string.IsNullOrEmpty(HotfixGuid))
            {
                Debug.LogError("热更新程序集错误");
                return null;
            }
            List<string> hotfixAssembly = new List<string>();
            string[] guids = AssetDatabase.FindAssets("t:AssemblyDefinitionAsset");
            for (int i = 0; i < guids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                var asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(AssemblyDefinitionAsset)) as AssemblyDefinitionAsset;
                var temptxt = asset.text.Replace(" ", string.Empty);
                var references = RegexUtility.GetMiddleValue(temptxt, $@"""references"":[", "],");
                if (references.Contains(EditorDefine.CNFHOTFIXASSEMBLY) || references.Contains(HotfixGuid))
                {
                    hotfixAssembly.Add(asset.name);
                }
            }
            return hotfixAssembly;
        }

        /// <summary>
        /// 拷贝热更新代码
        /// </summary>
        static void CopyCode()
        {
            if (!Directory.Exists(EditorDefine.CODEDIR)) Directory.CreateDirectory(EditorDefine.CODEDIR);
            var hotfixGuids = AssetDatabase.FindAssets(EditorDefine.CNFHOTFIXASSEMBLY);
            string HotfixGuid = string.Empty;
            for (int i = 0; i < hotfixGuids.Length; i++)
            {
                var tempHotfixPath = AssetDatabase.GUIDToAssetPath(hotfixGuids[i]);
                if (tempHotfixPath.EndsWith($"{EditorDefine.CNFHOTFIXASSEMBLY}.asmdef"))
                {
                    HotfixGuid = hotfixGuids[i];
                }
            }

            if (string.IsNullOrEmpty(HotfixGuid))
            {
                Debug.LogError("热更新程序集错误");
                return;
            }

            List<string> objs = new List<string>();
            string[] guids = AssetDatabase.FindAssets("t:AssemblyDefinitionAsset");
            for (int i = 0; i < guids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                var asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(AssemblyDefinitionAsset)) as AssemblyDefinitionAsset;
                var temptxt = asset.text.Replace(" ", string.Empty);
                var references = RegexUtility.GetMiddleValue(temptxt, $@"""references"":[", "],");
                if (references.Contains(EditorDefine.CNFHOTFIXASSEMBLY) || references.Contains(HotfixGuid))
                {
                    objs.Add(asset.name);
                }
            }
            for (int i = 0; i < objs.Count; i++)
            {
                if (File.Exists(Path.Combine(EditorDefine.SCRIPTASSEMBLIESDIR, objs[i] + DllSuffix)))
                {
                    File.Copy(Path.Combine(EditorDefine.SCRIPTASSEMBLIESDIR, objs[i] + DllSuffix), Path.Combine(EditorDefine.CODEDIR, objs[i] + CodeSuffix), true);
                    File.Copy(Path.Combine(EditorDefine.SCRIPTASSEMBLIESDIR, objs[i] + PadSuffix), Path.Combine(EditorDefine.CODEDIR, objs[i] + CodePdbSuffix), true);
                }
                else
                {
                    objs.RemoveAt(i);
                }
            }
            CheckCode(objs);
        }

        /// <summary>
        /// 校验Code.prefab
        /// </summary>
        static void CheckCode(List<string> objs)
        {
            LoadIndependent(CodePrefab, (prefab) =>
             {
                 if (prefab)
                 {
                     ReferenceCollector rc = prefab.GetComponent<ReferenceCollector>();
                     bool CheckNull(string type)
                     {
                         bool sign = false;
                         for (int i = 0; i < objs.Count; i++)
                         {
                             UnityEngine.Object obj = prefab.Get<UnityEngine.Object>(objs[i] + type);
                             if (obj == null)
                             {
                                 var codeSuffix = type == DllSuffix ? CodeSuffix : CodePdbSuffix;
                                 string objPath = Path.Combine(EditorDefine.CODEDIR, objs[i] + codeSuffix);
                                 var asset = AssetDatabase.LoadAssetAtPath(objPath, typeof(UnityEngine.Object));
                                 rc.Add(objs[i] + type, asset);
                                 sign = true;
                             }
                         }
                         return sign;
                     }
                     bool checkDll = CheckNull(DllSuffix);
                     bool checkPdb = CheckNull(PadSuffix);
                     if (checkDll || checkPdb)
                     {
                         EditorUtility.SetDirty(prefab);
                         AssetDatabase.Refresh();
                     }
                 }
             });
        }

        /// <summary>
        /// 校验Config.prefab
        /// </summary>
        static void CheckConfig()
        {
            LoadIndependent("Config", (prefab) =>
             {
                 if (prefab)
                 {
                     bool hasConfig = Directory.Exists(EditorDefine.CONFIGDIR);
                     List<string> configFiles = new List<string>();
                     if (hasConfig)
                         configFiles = Directory.GetFiles(EditorDefine.CONFIGDIR, "*.txt")?.ToList();
                     if (!hasConfig || configFiles.Count == 0)
                     {
                         EditorApplication.isPlaying = false;
                         EditorUtility.DisplayDialog("提示", "当前还没有生成配置表,请先生成配置表", "确定");
                         return;
                     }
                     ReferenceCollector rc = prefab.GetComponent<ReferenceCollector>();
                     bool CheckNull(string configFile)
                     {
                         string fileName = Path.GetFileNameWithoutExtension(configFile);
                         UnityEngine.Object obj = prefab.Get<UnityEngine.Object>(fileName);
                         if (obj == null)
                         {
                             obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(configFile);
                             rc.Add(fileName, obj);
                             return true;
                         }
                         return false;
                     }
                     bool isUpdate = false;
                     foreach (var configFile in configFiles)
                     {
                         if (CheckNull(configFile))
                         {
                             isUpdate = true;
                         }
                     }

                     if (isUpdate)
                     {
                         EditorUtility.SetDirty(prefab);
                         //AssetDatabase.Refresh();
                     }
                 }
             });
        }

        /// <summary>
        /// 加载内置prefab
        /// </summary>
        static void LoadIndependent(string prefabName, System.Action<GameObject> prefabCall)
        {
            if (!Directory.Exists(EditorDefine.BUNDLESDIR))
                Directory.CreateDirectory(EditorDefine.BUNDLESDIR);
            string path = $"{EditorDefine.BUNDLESDIR}{prefabName}.prefab";
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null)
            {
                prefab = new GameObject(prefabName);
                prefab.AddComponent<ReferenceCollector>();
                PrefabUtility.SaveAsPrefabAsset(prefab, path, out bool sucess);
                UnityEngine.Object.DestroyImmediate(prefab);
                AssetDatabase.Refresh();
            }
            void GetPrefab()
            {
                var tempPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                EditorGUIUtility.PingObject(tempPrefab);
                Selection.activeObject = tempPrefab;
                prefabCall.Invoke(tempPrefab);
                EditorApplication.delayCall -= GetPrefab;
            }
            EditorApplication.delayCall += GetPrefab;
        }
    }
}
