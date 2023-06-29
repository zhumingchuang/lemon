using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LemonFramework.Editor
{
    public class BuildProjectView : EditorWindow, IEditorView
    {
        public List<ManifestInfo> ManifestInfos { get; set; }

        public string AssetBundleServerUrl { get; set; }
        public string Address { get; set; }

        public DllConfig DllConfig { get; set; }

        public event Action OnCreateEvent;

        private Vector2 scrollPositionL;
        private Vector2 scrollPositionR;

        private int addCount;
        private bool addSign;

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

        private GUIStyle inputStyle;
        private GUIStyle InputStyle
        {
            get
            {
                if (inputStyle == null)
                {
                    inputStyle = new GUIStyle(GUI.skin.textField)
                    {
                        fontSize = 12,
                        alignment = TextAnchor.MiddleLeft,
                        imagePosition = ImagePosition.ImageAbove,
                        fontStyle = UnityEngine.FontStyle.Normal,
                    };
                }
                return inputStyle;
            }
        }

        public void CloseView()
        {
            base.Close();
        }

        public void ShowView(bool auto)
        {
            if (auto)
            {
                var window = GetWindow<BuildProjectView>();
                window.titleContent = (new GUIContent("BuildProjectSettings"));
                window.position = new Rect(Screen.resolutions[0].width, Screen.resolutions[0].height - 400, 620f, 500f);
                window.Show();
            }
            else
            {
                titleContent = (new GUIContent("BuildProjectSetings"));
            }
        }

        private List<ManifestInfo> tempManifestInfo = new List<ManifestInfo>();

        /// <summary>
        /// 添加资源清单
        /// </summary>
        private void AddManifestInfo()
        {
            EditorGUILayout.BeginHorizontal("box");
            InputStyle.fixedWidth = Screen.resolutions[0].width / 4;
            GUILayout.Label("数量:", GUILayout.MaxWidth(80));
            addCount = EditorGUILayout.DelayedIntField(addCount, InputStyle);
            if (GuiTools.Button("X", Color.red, 80))
            {
                addSign = false;
                addCount = 0;
            }
            EditorGUILayout.EndHorizontal();

            if (addCount > 0)
            {
                if (addCount != tempManifestInfo.Count)
                {
                    tempManifestInfo.Clear();
                    for (int i = 0; i < addCount; i++)
                    {
                        tempManifestInfo.Add(new ManifestInfo());
                    }
                }

                GuiTools.BeginArea(new Rect(0, 50, position.width - 160, position.height * 0.75f - 50));
                scrollPositionR = EditorGUILayout.BeginScrollView(scrollPositionR, GUILayout.ExpandHeight(false));
                for (int i = 0; i < tempManifestInfo.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal("box");
                    InputStyle.fixedWidth = Screen.resolutions[0].width / 4;

                    GUILayout.Label($"ManifestInfo:{i + 1}", GUILayout.MaxWidth(100));
                    tempManifestInfo[i].name = EditorGUILayout.TextField(tempManifestInfo[i].name, InputStyle);

                    GUILayout.Label("autoUpdate:", GUILayout.MaxWidth(80));
                    tempManifestInfo[i].autoUpdate = GUILayout.Toggle(tempManifestInfo[i].autoUpdate, string.Empty);

                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();
                GuiTools.EndArea();
                if (GUI.Button(new Rect((position.width - 160) * 0.5f - 60, position.height * 0.8f, 80, 30), "Add", BtnStyle))
                {
                    bool error = tempManifestInfo.GroupBy(i => i.name).Where(g => g.Count() > 1).Count() > 0;
                    string tips = null;
                    for (int i = 0; i < tempManifestInfo.Count; i++)
                    {
                        for (int j = 0; j < ManifestInfos.Count; j++)
                        {
                            if (tempManifestInfo[i].name == ManifestInfos[j].name)
                            {
                                error = true;
                                tips = ManifestInfos[i].name;
                            }
                        }
                    }
                    if (!error)
                    {
                        ManifestInfos.AddRange(tempManifestInfo);
                        tempManifestInfo = new List<ManifestInfo>();
                        addSign = false;
                        addCount = 0;
                    }
                    else
                    {
                        this.ShowNotification(new GUIContent($"重复定义{tips}"));
                    }
                }
            }
        }

        private void OnGUI()
        {
            GuiTools.BeginArea(new Rect(0, 0, 160, this.position.height));
            scrollPositionL = EditorGUILayout.BeginScrollView(scrollPositionL, GUILayout.ExpandHeight(false));
            EditorGUILayout.Space();

            if (ManifestInfos != null)
            {
                for (int i = 0; i < ManifestInfos.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal("box");
                    ManifestInfos[i].autoUpdate = GUILayout.Toggle(ManifestInfos[i].autoUpdate, new GUIContent(ManifestInfos[i].name));
                    if (GuiTools.Button("X", Color.red, 30, height: 15))
                    {
                        ManifestInfos.RemoveAt(i);
                        return;
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    GuiTools.DrawSeparatorLine();
                }
            }

            EditorGUILayout.EndScrollView();
            GuiTools.EndArea();

            GuiTools.BeginArea(new Rect(160, 0, this.position.width - 160, this.position.height));

            if (addSign)
            {
                AddManifestInfo();
            }

            if (!addSign)
            {
                EditorGUILayout.LabelField("输入资源地址");
                AssetBundleServerUrl = EditorGUILayout.TextField(AssetBundleServerUrl, Array.Empty<GUILayoutOption>());
                EditorGUILayout.LabelField("输入服务器地址");
                Address = EditorGUILayout.TextField(Address, Array.Empty<GUILayoutOption>());

                GuiTools.DrawSeparatorLine();


                EditorGUILayout.LabelField("PlatformName");
                DllConfig.PlatformName = EditorGUILayout.TextField(DllConfig.PlatformName, Array.Empty<GUILayoutOption>());

                EditorGUILayout.LabelField("ManifestName");
                DllConfig.ManifestName = EditorGUILayout.TextField(DllConfig.ManifestName, Array.Empty<GUILayoutOption>());

                if(!DllConfig.UseStreamingAssets)
                {
                    EditorGUILayout.LabelField("输入Code资源地址");
                    DllConfig.Uri = EditorGUILayout.TextField(DllConfig.Uri, Array.Empty<GUILayoutOption>());
                }

                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("UseStreamingAssets", GUILayout.Width(150));
                DllConfig.UseStreamingAssets = EditorGUILayout.Toggle(DllConfig.UseStreamingAssets);
                EditorGUILayout.EndHorizontal();

                if (GUI.Button(new Rect((this.position.width - 360) * 0.5f, this.position.height * 0.8f, 80, 30), "Add", BtnStyle))
                {
                    addSign = true;
                }

                if (GUI.Button(new Rect((this.position.width - 160) * 0.5f, this.position.height * 0.8f, 80, 30), "Save", BtnStyle))
                {
                    Action action = this.OnCreateEvent;
                    action?.Invoke();
                }
            }
            GuiTools.EndArea();
        }
    }
}
