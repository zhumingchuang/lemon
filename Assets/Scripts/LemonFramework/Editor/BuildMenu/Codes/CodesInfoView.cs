using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using Unity.EditorCoroutines.Editor;

namespace LemonFramework.Editor
{
    public class CodesInfoView : EditorWindow, IEditorView
    {
        public List<string> HotfixCodePath { get; set; }

        public List<string> CodesPath { get; set; }

        public event Action OnFocusEvent;

        public event Action<List<string>> OnCodesChanger;

        public void CloseView ()
        {
            base.Close ();
        }

        public void ShowView (bool auto)
        {
            if (auto)
            {
                var window = GetWindow<CodesInfoView> ();
                window.titleContent = (new GUIContent ("CodesInfo"));
                window.position = new Rect (Screen.resolutions[0].width, Screen.resolutions[0].height - 400, 620f, 500f);
                window.Show ();
            }
            else
            {
                titleContent = (new GUIContent ("CodesInfo"));
            }
        }

        public void OnFocus ()
        {
            IEnumerator wait ()
            {
                yield return null;
                OnFocusEvent?.Invoke ();
            }
            this.StartCoroutine (wait ());
        }

        Vector2 scrollPosition1;
        Vector2 scrollPosition2;
        bool hotfixSign = true;
        string _path;
        private void OnGUI ()
        {
            scrollPosition1 = EditorGUILayout.BeginScrollView (scrollPosition1, GUILayout.ExpandHeight (false));

            EditorGUILayout.LabelField ("当前选中的程序集:", Array.Empty<GUILayoutOption> ());
            EditorGUILayout.Space ();

            hotfixSign = GuiTools.BeginFoldOut ("HotfixPath", hotfixSign);
            if (hotfixSign)
            {
                EditorGUILayout.LabelField ("HotfixPath:", Array.Empty<GUILayoutOption> ());

                for (int i = 0; i < HotfixCodePath.Count; i++)
                {
                    GuiTools.BeginGroup (10);
                    EditorGUILayout.LabelField ($"程序集-{Path.GetFileNameWithoutExtension (HotfixCodePath[i])}", Array.Empty<GUILayoutOption> ());
                    EditorGUILayout.LabelField ($"路径-{HotfixCodePath[i]}", Array.Empty<GUILayoutOption> ());
                    GuiTools.EndGroup ();
                    EditorGUILayout.Space ();
                }
            }
            EditorGUILayout.EndScrollView ();

            GuiTools.DrawSeparatorLine ();
            EditorGUILayout.LabelField ("当前文件:", Array.Empty<GUILayoutOption> ());
            scrollPosition2 = EditorGUILayout.BeginScrollView (scrollPosition2, GUILayout.ExpandHeight (false));

            for (int i = 0; i < CodesPath.Count; i++)
            {
                EditorGUILayout.BeginHorizontal ("box");
                if (GuiTools.Button ("X", Color.red, 30, height: 15))
                {
                    CodesPath.RemoveAt (i);
                    OnCodesChanger?.Invoke (CodesPath);
                    return;
                }
                EditorGUILayout.LabelField ($"路径-{Path.GetFullPath (CodesPath[i])}", Array.Empty<GUILayoutOption> ());
                EditorGUILayout.EndHorizontal ();
            }

            EditorGUILayout.EndScrollView ();
            _path = EditorGUILayout.TextField (_path, Array.Empty<GUILayoutOption> ());
            EditorGUILayout.Space ();
            if (GUILayout.Button ("AddCodePath", Array.Empty<GUILayoutOption> ()))
            {
                if (!string.IsNullOrEmpty (_path))
                {
                    if (Directory.Exists (_path))
                    {
                        if (Path.GetFullPath (_path).Replace ('\\', '/').StartsWith (Application.dataPath))
                        {
                            ShowNotification (new GUIContent ($"不能添加Unity内部文件{_path}"));
                        }
                        else
                        {
                            if (string.IsNullOrEmpty (Path.GetFullPath (_path).Replace (Directory.GetCurrentDirectory (), "")))
                            {
                                ShowNotification (new GUIContent ($"不能添加工程跟目录"));
                                return;
                            }

                            for (int i = 0; i < CodesPath.Count; i++)
                            {
                                if (Path.GetFullPath (CodesPath[i]) == Path.GetFullPath (_path))
                                {
                                    ShowNotification (new GUIContent ($"存在相同目录"));
                                    return;
                                }
                            }

                            CodesPath.Add (_path);
                            OnCodesChanger?.Invoke (CodesPath);
                        }
                    }
                    else
                    {
                        ShowNotification (new GUIContent ($"不存在路径"));
                    }
                }
            }
        }
    }
}
