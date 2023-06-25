using System;
using UnityEditor;
using UnityEngine;

namespace LemonFramework.Editor.UIModule
{
    public class CreateMVPView : EditorWindow, IEditorView
    {
        private string _keyword;

        /// <summary>
        /// 脚本名称
        /// </summary>
        public string Keyword
        {
            get
            {
                return _keyword;
            }
            set
            {
                _keyword = value;
            }
        }


        public string SelectAssemblyName { get; set; }

        public int selectIndex { get; set; }

        public string[] CNFAssembly { get; set; }

        public event Action OnCreateEvent;

        public CreateMVPView() { }

        public void CloseView()
        {
            base.Close();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
            selectIndex = EditorGUILayout.Popup(selectIndex, CNFAssembly);
            if(CNFAssembly.Length>0)
            SelectAssemblyName = CNFAssembly[selectIndex];
            if (string.IsNullOrEmpty(SelectAssemblyName))
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("当前没有选择程序集", Array.Empty<GUILayoutOption>());
            }
            else
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField($"当前程序集{SelectAssemblyName}", Array.Empty<GUILayoutOption>());
            }
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("自动生成View/Presenter文件", Array.Empty<GUILayoutOption>());
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("自动生成View/ViewBase文件", Array.Empty<GUILayoutOption>());
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("请输入关键字", Array.Empty<GUILayoutOption>());
            EditorGUILayout.Space();
            _keyword = EditorGUILayout.TextField(_keyword, Array.Empty<GUILayoutOption>());
            EditorGUILayout.Space();
            if (GUILayout.Button("Create", Array.Empty<GUILayoutOption>()))
            {
                Action action = this.OnCreateEvent;
                action?.Invoke();
            }
            EditorGUILayout.EndVertical();
        }

        public void ShowView(bool auto)
        {
            if(auto)
            {
                CreateMVPView window = GetWindow<CreateMVPView>();
                window.titleContent = (new GUIContent("MVPCreator"));
                window.position = (new Rect(Screen.resolutions[0].width, Screen.resolutions[0].height - 200, 400f, 300f));
                window.Show();
            }
            else
            {
                titleContent = (new GUIContent("MVPCreator"));
            }
        }
    }
}