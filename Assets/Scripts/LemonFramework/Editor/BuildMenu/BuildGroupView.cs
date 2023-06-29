using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.IO;

namespace LemonFramework.Editor
{
    public class BuildGroupView : EditorWindow, IEditorView
    {
        private static MethodInfo dockAreaAddTabMethod;
        private static FieldInfo editorWindowDockAreaField;
        public Dictionary<string, IEditorView> EditorViews { get; set; }
        public Dictionary<string, System.Func<bool, EditorWindow>> EditorViewsEvent { get; set; }
        object toolDockArea;

        public void CloseView()
        {
            base.Close();
        }

        public void ShowView(bool auto)
        {
            //创建最外层容器
            object containerInstance = EditorContainerWindow.CreateInstance();
            //创建分屏容器
            object splitViewInstance = EditorSplitView.CreateInstance();
            //设置根容器
            EditorContainerWindow.SetRootView(containerInstance, splitViewInstance);
            //添加menu容器和工具容器
            object menuDockAreaInstance = EditorDockArea.CreateInstance();
            EditorDockArea.SetPosition(menuDockAreaInstance, new Rect(0, 0, 150, 600));
            titleContent = (new GUIContent("BuildGroup"));
            position = new Rect(0, 0, 150, 600);
            minSize = new Vector2(150, 600);
            EditorDockArea.AddTab(menuDockAreaInstance, this);
            EditorSplitView.AddChild(splitViewInstance, menuDockAreaInstance);
            EditorEditorWindow.MakeParentsSettingsMatchMe(this);

            object tool = EditorSplitView.CreateInstance();
            EditorSplitView.SetPosition(tool, new Rect(0, 0, 600, 600));
            toolDockArea = EditorDockArea.CreateInstance();
            EditorDockArea.SetPosition(toolDockArea, new Rect(0, 0, 600, 200));
            for (int i = 0; i < EditorViews.Count; i++)
            {
                var item = EditorViews.ElementAt(i);
                EditorDockArea.AddTab(toolDockArea, item.Value as EditorWindow);
                EditorEditorWindow.MakeParentsSettingsMatchMe(item.Value as EditorWindow);
            }
            EditorSplitView.AddChild(tool, toolDockArea);
            //添加tool切割窗体
            EditorSplitView.AddChild(splitViewInstance, tool);
            EditorContainerWindow.SetPosition(containerInstance, new Rect(500, 100, 800, 600));
            EditorSplitView.SetPosition(splitViewInstance, new Rect(0, 0, 800, 600));
            EditorContainerWindow.Show(containerInstance, 0, true, false, true);
            EditorContainerWindow.OnResize(containerInstance);
            return;

            editorWindowDockAreaField = typeof(EditorWindow).GetField("m_Parent", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            Type dockAreaType = typeof(EditorWindow).Assembly.GetType("UnityEditor.DockArea");
            if (dockAreaType != null)
            {
#if UNITY_2018_3_OR_NEWER
                dockAreaAddTabMethod = dockAreaType.GetMethod("AddTab", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[2] { typeof(EditorWindow), typeof(bool) }, null);
#else
				dockAreaAddTabMethod = dockAreaType.GetMethod( "AddTab", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[1] { typeof( EditorWindow ) }, null );
#endif
            }

            for (int i = 0; i < EditorViews.Count; i++)
            {
                var item = EditorViews.ElementAt(i);
                object dockArea = editorWindowDockAreaField.GetValue(this);
#if UNITY_2018_3_OR_NEWER
                dockAreaAddTabMethod.Invoke(dockArea, new object[2] { item.Value, true });
#else
							dockAreaAddTabMethod.Invoke( dockArea, new object[1] { newTab } );
#endif
            }
            Focus();
        }

        bool[] menuGroup=new bool[] 
        {
            true,
            false
        };

        private void OnGUI()
        {
            GuiTools.BeginArea(new Rect(0, 0, 160, this.position.height));

            GUILayout.Space(1);

            menuGroup[0] = GuiTools.BeginFoldOut("BuildCodeGroup", menuGroup[0]);
            if (menuGroup[0])
            {

                if (GUILayout.Button("BuildLogic", Array.Empty<GUILayoutOption>()))
                {
                    string[] logicFiles = Directory.GetFiles(EditorDefine.BuildOutputDir, "Logic_*");
                    foreach (string file in logicFiles)
                    {
                        File.Delete(file);
                    }
                    int random = RandomHelper.RandomNumber(100000000, 999999999);
                    string logicFile = $"Logic_{random}";
                    BuildAssemblieEditorHelper.BuildMuteAssembly_External(logicFile, CodesInfo._model.CodesPath);
                }

                if (GUILayout.Button("BuildSelectPath", Array.Empty<GUILayoutOption>()))
                {
                    BuildAssemblieEditorHelper.BuildMuteAssembly_CodePath("Code",CodesInfo._model.CodesPath);
                }

                if (GUILayout.Button("BuildSelectAssembly", Array.Empty<GUILayoutOption>()))
                {
                    BuildAssemblieEditorHelper.BuildMuteAssembly_AssemblyPath("Code", CodesInfo._model.HotfixCodePath);
                }

                if (GUILayout.Button("BuildCodeDebug", Array.Empty<GUILayoutOption>()))
                {
                    BuildAssemblieEditorHelper.BuildMuteAssembly_Assembly_Code("Code", CodesInfo._model.HotfixCodePath, CodesInfo._model.CodesPath);
                }

                if (GUILayout.Button("BuildCodeRelease", Array.Empty<GUILayoutOption>()))
                {
                    BuildAssemblieEditorHelper.BuildMuteAssembly_Assembly_Code("Code", CodesInfo._model.HotfixCodePath, CodesInfo._model.CodesPath);
                }
            }

            GuiTools.DrawSeparatorLine();

            menuGroup[1] = GuiTools.BeginFoldOut("ToolsGroup", menuGroup[1]);
            if (menuGroup[1])
            {
                for (int i = 0; i < EditorViews.Count; i++)
                {
                    var item = EditorViews.ElementAt(i);
                    if (GUILayout.Button(item.Key, Array.Empty<GUILayoutOption>()))
                    {
                        if (item.Value as EditorWindow == null)
                        {
                            var win = EditorViewsEvent[item.Key].Invoke(false);
                            EditorDockArea.AddTab(toolDockArea, win);
                            EditorEditorWindow.MakeParentsSettingsMatchMe(win);
                            EditorViews[item.Key] = win as IEditorView;
                        }
                        (item.Value as EditorWindow).Focus();
                    }
                }
            }

            GuiTools.EndArea();
        }
    }
}
