using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace LemonFramework.Editor
{
    public static class AssemblyFileCreator
    {
        private static CreateAssemblyModel _model;
        [MenuItem(EditorDefine.CREATEASSEMBLY)]
        public static void CreateMenu()
        {
            Create(true);
        }

        public static CreateAssemblyView Create(bool show = false)
        {
            if (_model != null && _model.View != null)
            {
                EditorViewManager.Close(_model);
            }
            _model = new CreateAssemblyModel()
            {
                Keyword = string.Empty
            };
            _model.OnCreateEvent += new Action<string>(OnCreate);
            _model.OnAssemblyChangerEvent += new Action<Dictionary<string, bool>>(AssemblyChanger);
            EditorViewManager.Show<CreateAssemblyView>(_model, show);
            return _model.View as CreateAssemblyView;
        }


        private static void AssemblyChanger(Dictionary<string, bool> assembly)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in assembly)
            {
                if (item.Value)
                {
                    sb.Append(item.Key + ",");
                }
            }
            EditorPrefs.SetString(EditorDefine.SELECTASSEMBLY, sb.ToString());
        }

        private static void OnCreate(string keyword)
        {
            string str;
            if (keyword != null)
            {
                str = keyword.Trim();
            }
            else
            {
                str = null;
            }
            keyword = str;
            if (string.IsNullOrEmpty(keyword))
            {
                ((CreateAssemblyView)_model.View).ShowNotification(new GUIContent("程序集不能为空!!!"));
                return;
            }

            if (AssetDatabaseTools.AssetExistence(keyword, ".asmdef"))
            {
                ((CreateAssemblyView)_model.View).ShowNotification(new GUIContent($"存在相同程序集{keyword}"));
                return;
            }

            string path = Path.Combine(Application.dataPath, $"Scripts/{keyword}");
            string hotfixPath = Path.Combine(path, $"{keyword}_Hotfix");
            string hotfixAssembly = Path.Combine(hotfixPath, $"{keyword}.Hotfix.asmdef");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!Directory.Exists(hotfixPath))
            {
                Directory.CreateDirectory(hotfixPath);
            }
            bool isClose = false;

            if (!File.Exists(hotfixAssembly))
            {
                CreatorAssembly(hotfixAssembly, $"{keyword}.Hotfix", EditorDefine.LFHOTFIXASSEMBLY);
                AssetDatabase.Refresh();
            }

            if (isClose)
            {
                AssetDatabase.Refresh();
                EditorViewManager.Close(_model);
            }
        }

        private static void CreatorAssembly(string path, string addAssembly)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine($@"""reference"":""{addAssembly}""");
            sb.AppendLine("}");
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }

        private static void CreatorAssembly(string path, string keyword, string addAssembly)
        {
            string[] guids = AssetDatabase.FindAssets(addAssembly);
            for (int i = 0; i < guids.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (assetPath.EndsWith($"{addAssembly}.asmdef"))
                {
                    var json = File.ReadAllLines(assetPath);
                    for (int k = 0; k < json.Length; k++)
                    {
                        json[k] = json[k].Replace(" ", "");
                    }
                    LinkedList<string> link = new LinkedList<string>(json);
                    link.Find($@"""name"":""{addAssembly}"",").Value = $@"""name"":""{keyword}"",";
                    link.AddAfter(link.Find($@"""references"":["), $@"""{addAssembly}"",");
                    File.WriteAllLines(path, link, Encoding.UTF8);
                    return;
                }
            }
        }
    }
}
