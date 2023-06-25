using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace LemonFramework.Editor.UIModule
{
    /// <summary>
    /// MVP文件创建
    /// </summary>
    public static class MVPFileCreator
    {
        private static CreateMVPModel _model;

        [MenuItem(EditorStrDef.UI.CREATE_MVP)]
        public static void CreateMenu()
        {
            Create(true);
        }

        public static CreateMVPView Create(bool show=false)
        {
            if (_model != null && _model.View != null)
            {
                EditorViewManager.Close(_model);
            }
            _model = new CreateMVPModel()
            {
                Keyword = string.Empty
            };
            _model.OnCreateEvent += new Action<string>(OnCreate);
            EditorViewManager.Show<CreateMVPView>(_model, show);
            return _model.View as CreateMVPView;
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
                ((CreateMVPView)_model.View).ShowNotification(new GUIContent("关键词不能为空!!!"));
                return;
            }
            var assembly = _model.SelectAssemblyPath;
            assembly = Path.GetDirectoryName(assembly);
            if (string.IsNullOrEmpty(assembly))
            {
                ((CreateMVPView)_model.View).ShowNotification(new GUIContent("请选择程序集!!!"));
                return;
            }

            EditorPrefs.SetString(EditorStrDef.SELECTUIASSEMBLY, ((CreateMVPView)_model.View).SelectAssemblyName);
            string[] pathGUID = UnityEditor.AssetDatabase.FindAssets("MVPFileCreator");
            if (pathGUID.Length > 1)
            {
                Debug.LogError("模板创建工具有同名文件");
                return;
            }
            var path = AssetDatabase.GUIDToAssetPath(pathGUID[0]);
            var pathTemplate = string.Empty;
            if (path.EndsWith("Editor/UI/MVPFileCreator.cs"))
            {
                pathTemplate = path.Substring(0, path.LastIndexOf("Editor/UI/MVPFileCreator.cs"));
            }
            else
            {
                Debug.LogError("查找脚本位置错误!!!");
                return;
            }
            string[] files;
            if (assembly.EndsWith("_Hotfix"))
            {
                UnityEngine.Debug.Log(pathTemplate);
                files = Directory.GetFiles(Path.Combine(pathTemplate, "Res/TextAsset/HotfixMVPTools"), "*.txt");
            }
            else
            {
                files = Directory.GetFiles(Path.Combine(pathTemplate, "Res/TextAsset/MVPTools"), "*.txt");
            }
            string str1 = Path.Combine(assembly, $"UI/View");
            str1 = Path.Combine(str1, string.Concat(keyword, "View"));
            if (Directory.Exists(str1))
            {
                Directory.Delete(str1, true);
            }
            Directory.CreateDirectory(str1);
            for (int i = 0; i < files.Length; i++)
            {
                string str2 = files[i];
                string fileName = Path.GetFileName(str2);
                if (fileName == "TemplateViewId.txt")
                {
                    string str3 = Path.Combine(assembly, $"UI/Core/ViewId.cs");
                    string directoryName = Path.GetDirectoryName(str3);
                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }
                    if (!File.Exists(str3))
                    {
                        File.WriteAllText(str3, File.ReadAllText(str2), Encoding.UTF8);
                    }
                    string str4 = string.Concat(keyword, "View");
                    string str5 = File.ReadAllText(str3, Encoding.UTF8);
                    if (!str5.Contains(str4))
                    {
                        string[] strArrays = str5.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        int num = 0;
                        while (num < (int)strArrays.Length)
                        {
                            string str6 = strArrays[num].TrimEnd(Array.Empty<char>());
                            if (!str6.Contains("//[AutoBuildPlaceHolder]#"))
                            {
                                num++;
                            }
                            else
                            {
                                int num1 = str6.IndexOf("#");
                                int num2 = int.Parse(str6.Substring(num1 + 1));
                                string str7 = string.Format("    public const int {0} = {1};\r\n    //[AutoBuildPlaceHolder]#{2}", str4, num2, num2 + 1);
                                str5 = str5.Replace(str6, str7);
                                File.WriteAllText(str3, str5, Encoding.UTF8);
                                break;
                            }
                        }
                    }
                }
                else if (fileName != "TemplateUIRegister.txt")
                {
                    fileName = fileName.Replace("Template", keyword).Replace(".txt", ".cs");
                    File.WriteAllText(Path.Combine(str1, fileName), File.ReadAllText(str2, Encoding.UTF8).Replace("[#]", keyword), Encoding.UTF8);
                }
                else
                {
                    UnityEngine.Debug.Log(str2);
                    string str8 = File.ReadAllText(str2);
                    string str9 = Path.Combine(assembly, $"UI/Core/UIRegister.cs");
                    string directoryName1 = Path.GetDirectoryName(str9);
                    UnityEngine.Debug.Log(str8);
                    if (!Directory.Exists(directoryName1))
                    {
                        Directory.CreateDirectory(directoryName1);
                    }
                    if (!File.Exists(str9))
                    {
                        File.WriteAllText(str9, str8);
                    }
                    string[] strArrays1 = File.ReadAllLines(str9);
                    List<string> strs = new List<string>(strArrays1);
                    int num3 = 0;
                    for (int j = (int)strArrays1.Length - 1; j >= 0; j--)
                    {
                        if (strArrays1[j].Trim() == "}")
                        {
                            num3++;
                            if (num3 >= 3)
                            {
                                strs.Insert(j, string.Concat(new string[] { "\r\n        Container.Regist<IView, ", keyword, "View>(ViewId.", keyword, "View);\r\n        Container.Regist<I", keyword, "Presenter, ", keyword, "Presenter>();" }));
                                File.WriteAllLines(str9, strs.ToArray());
                                break;
                            }
                        }
                    }
                }
            }
            Debug.LogFormat($"创建{keyword}成功!!");
            AssetDatabase.Refresh();
            EditorViewManager.Close(_model);
        }
    }
}