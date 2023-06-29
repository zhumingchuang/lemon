using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace LemonFramework.Editor
{
    public static class BuildAssemblieEditorHelper
    {

        /// <summary>
        /// 根据路径 和选择程序集打包Dll
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="codePath"></param>
        public static void BuildMuteAssembly_Assembly_Code (string assemblyName, List<string> assemblyPath, List<string> codePath)
        {
            if (string.IsNullOrEmpty (assemblyName)) return;
            if (assemblyPath.Count == 0 && codePath.Count == 0) return;

            List<string> excludeReferences = new List<string> ();
            List<string> codeDirectorys = new List<string> ();
            var hotfix = AssetDatabaseTools.FindAssets (EditorDefine.CNFHOTFIXASSEMBLY, ".asmdef");
            var hotfixPath = Path.GetDirectoryName (hotfix);
            codeDirectorys.Add (hotfixPath);

            excludeReferences.Add (Path.Combine (EditorDefine.SCRIPTASSEMBLIESDIR, $"{EditorDefine.CNFHOTFIXASSEMBLY}.dll").Replace ("\\", "/"));

            if (assemblyPath != null && assemblyPath.Count > 0)
            {
                for (int i = 0; i < assemblyPath.Count; i++)
                {
                    excludeReferences.Add (Path.Combine (EditorDefine.SCRIPTASSEMBLIESDIR, $"{Path.GetFileNameWithoutExtension (assemblyPath[i])}.dll").Replace ("\\", "/"));

                    var codeDirectory = Path.GetDirectoryName (assemblyPath[i]);

                    var asmdefs = AssetDatabase.FindAssets ("t:asmdef", new[] { codeDirectory });
                    if (asmdefs.Length > 1)
                    {
                        Debug.LogError ("热更新文件夹下存在多个程序集");
                        return;
                    }

                    var asmref = AssetDatabase.FindAssets ("t:asmref", new[] { codeDirectory });
                    if (asmref.Length > 0)
                    {
                        Debug.LogError ("热更新文件夹下存在多个程序集");
                        return;
                    }

                    codeDirectorys.Add (codeDirectory);
                }
            }

            if (codePath != null && codePath.Count > 0)
            {
                codeDirectorys.AddRange (codePath);
            }

            BuildMuteAssembly (assemblyName, codeDirectorys.ToArray (), Array.Empty<string> (), excludeReferences.ToArray ());
            AfterCompiling (assemblyName);
            AssetDatabase.Refresh ();
        }

        /// <summary>
        /// 根据添加路径打包Dll
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="assemblyPath"></param>
        public static void BuildMuteAssembly_External (string assemblyName, List<string> assemblyPath)
        {
            if (string.IsNullOrEmpty (assemblyName)) return;
            if (assemblyPath != null && assemblyPath.Count > 0)
            {
                List<string> codeDirectorys = new List<string> ();

                codeDirectorys.AddRange (assemblyPath);

                BuildMuteAssembly (assemblyName, codeDirectorys.ToArray (), Array.Empty<string> (), Array.Empty<string> ());
                AfterCompiling (assemblyName);
                AssetDatabase.Refresh ();
            }
        }

        /// <summary>
        /// 根据添加路径打包Dll
        /// </summary>
        /// <param name="assemblyPath"></param>
        public static void BuildMuteAssembly_CodePath (string assemblyName, List<string> assemblyPath)
        {
            if (string.IsNullOrEmpty (assemblyName)) return;
            if (assemblyPath != null && assemblyPath.Count > 0)
            {
                List<string> excludeReferences = new List<string> ();
                List<string> codeDirectorys = new List<string> ();
                var hotfix = AssetDatabaseTools.FindAssets (EditorDefine.CNFHOTFIXASSEMBLY, ".asmdef");
                var hotfixPath = Path.GetDirectoryName (hotfix);
                codeDirectorys.Add (hotfixPath);

                excludeReferences.Add (Path.Combine (EditorDefine.SCRIPTASSEMBLIESDIR, $"{EditorDefine.CNFHOTFIXASSEMBLY}.dll").Replace ("\\", "/"));

                codeDirectorys.AddRange (assemblyPath);

                BuildMuteAssembly (assemblyName, codeDirectorys.ToArray (), Array.Empty<string> (), excludeReferences.ToArray ());
                AfterCompiling (assemblyName);
                AssetDatabase.Refresh ();
            }

        }


        /// <summary>
        /// 根据程序集路径打包DLL
        /// </summary>
        /// <param name="assemblyPath"></param>
        public static void BuildMuteAssembly_AssemblyPath (string assemblyName, List<string> assemblyPath)
        {
            if (assemblyPath != null && assemblyPath.Count > 0)
            {
                List<string> excludeReferences = new List<string> ();
                List<string> codeDirectorys = new List<string> ();
                var hotfix = AssetDatabaseTools.FindAssets (EditorDefine.CNFHOTFIXASSEMBLY, ".asmdef");
                var hotfixPath = Path.GetDirectoryName (hotfix);
                codeDirectorys.Add (hotfixPath);

                excludeReferences.Add (Path.Combine (EditorDefine.SCRIPTASSEMBLIESDIR, $"{EditorDefine.CNFHOTFIXASSEMBLY}.dll").Replace ("\\", "/"));

                for (int i = 0; i < assemblyPath.Count; i++)
                {
                    excludeReferences.Add (Path.Combine (EditorDefine.SCRIPTASSEMBLIESDIR, $"{Path.GetFileNameWithoutExtension (assemblyPath[i])}.dll").Replace ("\\", "/"));

                    var codeDirectory = Path.GetDirectoryName (assemblyPath[i]);

                    var asmdefs = AssetDatabase.FindAssets ("t:asmdef", new[] { codeDirectory });
                    if (asmdefs.Length > 1)
                    {
                        Debug.LogError ("热更新文件夹下存在多个程序集");
                        return;
                    }

                    var asmref = AssetDatabase.FindAssets ("t:asmref", new[] { codeDirectory });
                    if (asmref.Length > 0)
                    {
                        Debug.LogError ("热更新文件夹下存在多个程序集");
                        return;
                    }

                    codeDirectorys.Add (codeDirectory);

                }

                BuildMuteAssembly (assemblyName, codeDirectorys.ToArray (), Array.Empty<string> (), excludeReferences.ToArray ());
                AfterCompiling (assemblyName);
                AssetDatabase.Refresh ();
            }
        }


        public static void BuildMuteAssembly (string assemblyName, string[] CodeDirectorys, string[] additionalReferences, string[] excludeReferences, CodeOptimization codeOptimization = CodeOptimization.Release)
        {
            List<string> scripts = new List<string> ();
            for (int i = 0; i < CodeDirectorys.Length; i++)
            {
                DirectoryInfo dti = new DirectoryInfo (CodeDirectorys[i]);
                FileInfo[] fileInfos = dti.GetFiles ("*.cs", System.IO.SearchOption.AllDirectories);
                for (int j = 0; j < fileInfos.Length; j++)
                {
                    scripts.Add (fileInfos[j].FullName);
                }
            }

            string dllPath = Path.Combine (EditorDefine.BuildOutputDir, $"{assemblyName}.dll");
            string pdbPath = Path.Combine (EditorDefine.BuildOutputDir, $"{assemblyName}.pdb");
            if (File.Exists (dllPath))
                File.Delete (dllPath);
            if (File.Exists (pdbPath))
                File.Delete (pdbPath);

            Directory.CreateDirectory (EditorDefine.BuildOutputDir);

            AssemblyBuilder assemblyBuilder = new AssemblyBuilder (dllPath, scripts.ToArray ());

            //启用UnSafe
            assemblyBuilder.compilerOptions.AllowUnsafeCode = true;

            BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup (EditorUserBuildSettings.activeBuildTarget);

            assemblyBuilder.compilerOptions.CodeOptimization = codeOptimization;
            assemblyBuilder.compilerOptions.ApiCompatibilityLevel = PlayerSettings.GetApiCompatibilityLevel (buildTargetGroup);
            // assemblyBuilder.compilerOptions.ApiCompatibilityLevel = ApiCompatibilityLevel.NET_4_6;

            assemblyBuilder.excludeReferences = excludeReferences;

            assemblyBuilder.additionalReferences = additionalReferences;

            assemblyBuilder.flags = AssemblyBuilderFlags.None;
            //AssemblyBuilderFlags.None                 正常发布
            //AssemblyBuilderFlags.DevelopmentBuild     开发模式打包
            //AssemblyBuilderFlags.EditorAssembly       编辑器状态
            assemblyBuilder.referencesOptions = ReferencesOptions.UseEngineModules;

            assemblyBuilder.buildTarget = EditorUserBuildSettings.activeBuildTarget;

            assemblyBuilder.buildTargetGroup = buildTargetGroup;

            assemblyBuilder.buildStarted += delegate (string assemblyPath) { Debug.LogFormat ("build start：" + assemblyPath); };

            assemblyBuilder.buildFinished += delegate (string assemblyPath, CompilerMessage[] compilerMessages)
            {
                int errorCount = compilerMessages.Count (m => m.type == CompilerMessageType.Error);
                int warningCount = compilerMessages.Count (m => m.type == CompilerMessageType.Warning);

                Debug.LogFormat ("Warnings: {0} - Errors: {1}", warningCount, errorCount);

                if (warningCount > 0)
                {
                    Debug.LogFormat ("有{0}个Warning!!!", warningCount);
                }

                if (errorCount > 0)
                {
                    for (int i = 0; i < compilerMessages.Length; i++)
                    {
                        if (compilerMessages[i].type == CompilerMessageType.Error)
                        {
                            Debug.LogError (compilerMessages[i].message);
                        }
                    }
                }
            };


            //开始构建
            if (!assemblyBuilder.Build ())
            {
                Debug.LogErrorFormat ("build fail：" + assemblyBuilder.assemblyPath);
                return;
            }
        }


        private static void AfterCompiling (string assemblyName)
        {
            while (EditorApplication.isCompiling)
            {
                Thread.Sleep (1000);
                Debug.Log ("Compiling wait");
            }
            Debug.Log ("Compiling finish");

            Directory.CreateDirectory (EditorDefine.CODEDIR);
            File.Copy (Path.Combine (EditorDefine.BuildOutputDir, $"{assemblyName}.dll"), Path.Combine (EditorDefine.CODEDIR, $"{assemblyName}.dll.bytes"), true);
            File.Copy (Path.Combine (EditorDefine.BuildOutputDir, $"{assemblyName}.pdb"), Path.Combine (EditorDefine.CODEDIR, $"{assemblyName}.pdb.bytes"), true);
            AssetDatabase.Refresh ();
            Debug.Log ("copy Code.dll to Bundles/Code success!");
        }
    }
}
