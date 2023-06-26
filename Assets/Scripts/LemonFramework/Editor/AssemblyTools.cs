using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;
using System.IO;

namespace LemonFramework.Editor
{
    public static class AssemblyTools
    {
        /// <summary>
        /// 程序集引用
        /// </summary>
        public static List<string> AssemblyReferenced (string baseAssembly)
        {
            List<string> referenced = new List<string> ();
            var baseGuid = AssetDatabaseTools.AssetsGUID (baseAssembly, ".asmdef");
            var guids = AssetDatabase.FindAssets ("t:asmref");
            for (int i = 0; i < guids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath (guids[i]);
                var adr = AssetDatabase.LoadAssetAtPath<AssemblyDefinitionReferenceAsset> (assetPath);

                if (adr.text.Contains (baseGuid) || adr.text.Contains (baseAssembly))
                {
                    referenced.Add (Path.GetFileNameWithoutExtension (assetPath));
                }
            }
            return referenced;
        }

        /// <summary>
        /// 查找指定程序集是否包含子集
        /// </summary>
        /// <param name="baseAssembly"></param>
        /// <param name="children"></param>
        /// <returns></returns>
        public static bool CheckAssemblyChildren (string baseAssembly, params string[] children)
        {
            var assembly = AssetDatabaseTools.FindAssets<AssemblyDefinitionAsset> (baseAssembly, ".asmdef");
            List<string> guids = new List<string> ();
            for (int i = 0; i < children.Length; i++)
            {
                guids.Add (AssetDatabaseTools.AssetsGUID (children[i], ".asmdef"));
            }

            if (assembly != null)
            {
                var temptxt = assembly.text.Replace (" ", "");
                var references = RegexUtility.GetMiddleValue (temptxt, $@"""references"":[", "],");

                for (int i = 0; i < guids.Count; i++)
                {
                    if (references.Contains (guids[i]) || references.Contains (children[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 查找所有引用CNF指定程序集的子集
        /// </summary>
        /// <param name="baseAssembly"></param>
        /// <returns></returns>
        public static List<string> FindAssemblyCNF (string baseAssembly)
        {
            return FindAssembly (baseAssembly, EditorStrDef.CNFHOTFIXASSEMBLY, EditorStrDef.CNFEDITORMODEL);
        }

        /// <summary>
        /// 查找所有引用指定程序集的子集
        /// </summary>
        /// <param name="baseAssembly"></param>
        /// <returns></returns>
        public static List<string> FindAssembly (string baseAssembly, params string[] exclude)
        {
            List<AssemblyDefinitionAsset> objs = new List<AssemblyDefinitionAsset> ();
            string guid = string.Empty;
            string[] guids = AssetDatabase.FindAssets ("t:AssemblyDefinitionAsset");
            for (int i = 0; i < guids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath (guids[i]);
                var asset = AssetDatabase.LoadAssetAtPath (assetPath, typeof (AssemblyDefinitionAsset)) as AssemblyDefinitionAsset;
                objs.Add (asset);
                if (asset.name == baseAssembly)
                {
                    guid = guids[i];
                }
            }

            List<string> assembly = new List<string> ();
            for (int i = 0; i < objs.Count; i++)
            {
                var temptxt = objs[i].text.Replace (" ", "");
                var references = RegexUtility.GetMiddleValue (temptxt, $@"""references"":[", "],");
                if (!string.IsNullOrEmpty (guid) && references.Contains (guid) || references.Contains (baseAssembly))
                {
                    if (exclude != null)
                    {
                        if (!exclude.Contains (objs[i].name))
                        {
                            assembly.Add (objs[i].name);
                        }
                    }
                }
            }
            return assembly;
        }

    }
}
