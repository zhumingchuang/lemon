using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Linq;
using System.Reflection;

namespace LemonFramework.Editor
{
    public class CreateAssemblyView : EditorWindow, IEditorView
    {
        public event Action OnCreateEvent;
        public event Action<Dictionary<string, bool>> OnAssemblyChangerEvent;
        private Vector2 scrollPositionL;
        private Dictionary<string, bool> dictAssembly = new Dictionary<string, bool>();

        private string _keyword;
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

        public string[] SelectAssembly { get; set; }
        public CreateAssemblyView()
        {

        }

        public void CloseView()
        {
            base.Close();
        }

        public void ShowView(bool auto)
        {
            if(auto)
            {
                CreateAssemblyView window = GetWindow<CreateAssemblyView>();
                window.titleContent = (new GUIContent("AssemblySettings"));
                window.position = new Rect(Screen.resolutions[0].width, Screen.resolutions[0].height - 400, 620f, 500f);
                window.Show();
            }
            else
            {
                titleContent = (new GUIContent("AssemblySettings"));
            }

            dictAssembly.Clear();

            var assemblies = AssemblyTools.FindAssemblyCNF(EditorDefine.CNFHOTFIXASSEMBLY);
            for (int i = 0; i < assemblies.Count; i++)
            {
                if (SelectAssembly != null && SelectAssembly.Contains(assemblies[i]))
                {
                    dictAssembly[assemblies[i]] = true;
                }
                else
                {
                    dictAssembly[assemblies[i]] = false;
                }
            }
        }

        private GUIStyle btnStyle;
        public GUIStyle BtnStyle
        {
            get
            {
                if (btnStyle == null)
                {
                    btnStyle = new GUIStyle(GUI.skin.button);
                    btnStyle.alignment = TextAnchor.MiddleCenter;
                    btnStyle.fontSize = 15;
                }
                return btnStyle;
            }
        }

        private void OnGUI()
        {
            GuiTools.BeginArea(new Rect(0, 0, 160, this.position.height));
            scrollPositionL = EditorGUILayout.BeginScrollView(scrollPositionL, GUILayout.ExpandHeight(false));

            EditorGUILayout.Space();
            bool flag = false;
            for (int i = 0; i < dictAssembly.Count; i++)
            {
                var item = dictAssembly.ElementAt(i);
                var ison = dictAssembly[item.Key];
                dictAssembly[item.Key] = GUILayout.Toggle(item.Value, new GUIContent(item.Key));
                if (ison != dictAssembly[item.Key])
                {
                    flag = true;
                }
                GuiTools.DrawSeparatorLine();
            }
            if (flag)
            {
                Action<Dictionary<string, bool>> action = this.OnAssemblyChangerEvent;
                action?.Invoke(dictAssembly);
            }
            EditorGUILayout.Space();

            EditorGUILayout.EndScrollView();
            GuiTools.EndArea();

            GuiTools.BeginArea(new Rect(160, 0, this.position.width - 160, this.position.height));

            EditorGUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("自动生成程序集文件", Array.Empty<GUILayoutOption>());
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("请输入程序集名称", Array.Empty<GUILayoutOption>());
            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"文件位置Asset/Scripts/{_keyword}   -文件夹", Array.Empty<GUILayoutOption>());
            EditorGUILayout.Space();

            _keyword = EditorGUILayout.TextField(_keyword, Array.Empty<GUILayoutOption>());
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();

            if (GUI.Button(new Rect((this.position.width - 260) * 0.5f, this.position.height * 0.8f, 80, 30), "Create", BtnStyle))
            {
                Action action = this.OnCreateEvent;
                action?.Invoke();
            }
            GuiTools.EndArea();
        }
    }
}

