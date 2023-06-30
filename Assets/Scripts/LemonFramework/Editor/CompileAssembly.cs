using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Linq;

namespace LemonFramework.Editor
{
    public static class CompileAssembly
    {
        public static string FullPath
        {
            get
            {
                return System.IO.Path.Combine(System.IO.Path.GetFullPath(EditorDefine.CODEDIR), EditorDefine.LFHOTFIXASSEMBLY + ".dll.bytes");
            }
        }

        public static bool RelatedAssemblyMergeDll(string fullPath, string assembly)
        {
            string hotfixGuid = string.Empty;
            List<string> assemblyFile = new List<string>();
            List<string> assemblyName = new List<string>();
            var hotfixGuids = AssetDatabase.FindAssets(assembly);
            for (int j = 0; j < hotfixGuids.Length; j++)
            {
                var tempHotfixPath = AssetDatabase.GUIDToAssetPath(hotfixGuids[j]);
                if (tempHotfixPath.EndsWith($"{assembly}.asmdef"))
                {
                    assemblyFile.Add(System.IO.Path.GetDirectoryName(tempHotfixPath));
                    assemblyName.Add(assembly);
                    hotfixGuid = hotfixGuids[j];
                    break;
                }
                if (j >= hotfixGuids.Length - 1)
                {
                    UnityEngine.Debug.LogError($"缺少程序集{assembly}");
                    return false;
                }
            }

            string[] guids = AssetDatabase.FindAssets("t:AssemblyDefinitionAsset");
            for (int i = 0; i < guids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                var asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(AssemblyDefinitionAsset)) as AssemblyDefinitionAsset;
                var temptxt = asset.text.Replace(" ", "");
                var references = RegexUtility.GetMiddleValue(temptxt, $@"""references"":[", "],");
                if (references.Contains(assembly) || references.Contains(hotfixGuid))
                {
                    assemblyFile.Add(System.IO.Path.GetDirectoryName(assetPath));
                    assemblyName.Add(asset.name);
                }
            }
            string[] guidsScript = AssetDatabase.FindAssets("t:Script", assemblyFile.ToArray());
            List<string> codesPath = new List<string>();
            for (int i = 0; i < guidsScript.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guidsScript[i]);
                codesPath.Add(assetPath);
            }
            return CompileDll(fullPath, assemblyName.ToArray(), codesPath.ToArray());
        }

        public static bool MergeDll(string fullPath, string[] assembly)
        {
            List<string> assemblyFile = new List<string>();
            for (int i = 0; i < assembly.Length; i++)
            {
                var hotfixGuids = AssetDatabase.FindAssets(assembly[i]);
                for (int j = 0; j < hotfixGuids.Length; j++)
                {
                    var tempHotfixPath = AssetDatabase.GUIDToAssetPath(hotfixGuids[j]);
                    if (tempHotfixPath.EndsWith($"{assembly[i]}.asmdef"))
                    {
                        assemblyFile.Add(System.IO.Path.GetDirectoryName(tempHotfixPath));
                        break;
                    }
                    if (j >= hotfixGuids.Length - 1)
                    {
                        UnityEngine.Debug.LogError($"缺少程序集{assembly[i]}");
                        return false;
                    }
                }
            }

            string[] guidsScript = AssetDatabase.FindAssets("t:Script", assemblyFile.ToArray());
            List<string> codesPath = new List<string>();
            for (int i = 0; i < guidsScript.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guidsScript[i]);
                codesPath.Add(assetPath);
            }
            return CompileDll(fullPath, assembly, codesPath.ToArray());
        }

        public static bool CompileDll(string outputAssembly, string[] assembly, params string[] code)
        {
            List<string> defines = new List<string>();
            List<string> references = new List<string>();
            for (int i = 0; i < assembly.Length; i++)
            {
                var referencedAssemblies = Assembly.Load(assembly[i]).GetReferencedAssemblies();
                for (int j = 0; j < referencedAssemblies.Length; j++)
                {
                    var childDll = Assembly.Load(referencedAssemblies[j].FullName);
                    if (!references.Contains(childDll.Location))
                    {
                        references.Add(childDll.Location);
                    }

                    //if ("netstandard" == referencedAssemblies[j].Name)
                    {
                        var childReferenced = childDll.GetReferencedAssemblies();
                        for (int k = 0; k < childReferenced.Length; k++)
                        {
                            var tempAssembly = Assembly.Load(childReferenced[k].FullName);
                            if (!references.Contains(tempAssembly.Location))
                            {
                                references.Add(tempAssembly.Location);
                            }
                        }
                    }
                }
            }
            var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            defines.AddRange(symbols.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            //var log = EditorUtility.CompileCSharp(code, references.ToArray(), defines.ToArray(), outputAssembly);
            //for (int i = 0; i < log.Length; i++)
            //{
            //    if (log[i].Contains("error"))
            //    {
            //        Debug.LogError(log[i]);
            //        return false;
            //    }
            //}
            return true;
        }
    }
}