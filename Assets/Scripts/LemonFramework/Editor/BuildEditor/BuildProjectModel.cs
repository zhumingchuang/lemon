using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using LemonFramework.ResModule;

namespace LemonFramework.Editor
{
    public class BuildProjectModel : IEditorModel
    {
        public event Action<string[], List<ManifestInfo>> OnCreateEvent;

        private string assetBundleServerUrl;
        public string AssetBundleServerUrl
        {
            get
            {
                BuildProjectView view = View as BuildProjectView;
                if (view != null)
                {
                    assetBundleServerUrl = view.AssetBundleServerUrl;
                }
                return assetBundleServerUrl;
            }
            set
            {
                assetBundleServerUrl = value;
                BuildProjectView view = View as BuildProjectView;
                if (view != null)
                {
                    view.AssetBundleServerUrl = value;
                }
            }
        }

        private string address;
        public string Address
        {
            get
            {
                BuildProjectView view = View as BuildProjectView;
                if (view != null)
                {
                    address = view.Address;
                }
                return address;
            }
            set
            {
                address = value;
                BuildProjectView view = View as BuildProjectView;
                if (view != null)
                {
                    view.Address = value;
                }
            }
        }

        private DllConfig dllConfig;
        public DllConfig DllConfig
        {
            get
            {
                BuildProjectView view = View as BuildProjectView;
                if (view != null)
                {
                    dllConfig = view.DllConfig;
                }
                return dllConfig;
            }
            set
            {
                dllConfig = value;
                BuildProjectView view = View as BuildProjectView;
                if (view != null)
                {
                    view.DllConfig = value;
                }
            }
        }

        private List<ManifestInfo> manifestInfos;
        public List<ManifestInfo> ManifestInfos
        {
            get
            {
                BuildProjectView view = View as BuildProjectView;
                if (view != null)
                {
                    manifestInfos = view.ManifestInfos;
                }
                return manifestInfos;
            }
            set
            {
                manifestInfos = value;
                BuildProjectView view = View as BuildProjectView;
                if (view != null)
                {
                    view.ManifestInfos = value;
                }
            }
        }

        private IEditorView _view;
        public IEditorView View
        {
            get
            {
                return _view;
            }
            set
            {
                _view = value;
                if (_view is BuildProjectView)
                {
                    (_view as BuildProjectView).OnCreateEvent += OnCreate;
                }
            }
        }

        public BuildProjectModel()
        {
            manifestInfos = GetManifestsConfig();
            manifestInfos = manifestInfos == null ? new List<ManifestInfo>() : manifestInfos;
        }

        private void OnCreate()
        {
            Action<string[], List<ManifestInfo>> action = OnCreateEvent;
            action?.Invoke(new[] { AssetBundleServerUrl, Address }, ManifestInfos);
        }

        /// <summary>
        /// 获取资源配置文件
        /// </summary>
        private List<ManifestInfo> GetManifestsConfig()
        {
            List<ManifestInfo> manifestInfo = null;
            if (File.Exists(Path.Combine(EditorDefine.CONFIGDIR, "ManifestsConfig.txt")))
            {
                string configStr = AssetDatabase.LoadAssetAtPath<TextAsset>(Path.Combine(EditorDefine.CONFIGDIR, "ManifestsConfig.txt")).text;
                manifestInfo = JsonUtility.FromJson<List<ManifestInfo>> (configStr);
            }
            return manifestInfo;
        }

        public void Setup()
        {
            ManifestInfos = manifestInfos;
            AssetBundleServerUrl = GlobalProto.AssetBundleServerUrl;
            Address = GlobalProto.Address;

            var dllConfig = System.IO.File.ReadAllText(Path.Combine(EditorDefine.CONFIGDIR, "CodeConfig.txt"));
            if(dllConfig!=null)
            {
                try
                {
                    DllConfig = JsonUtility.FromJson<DllConfig>(dllConfig);
                }
                catch (Exception)
                {
                    DllConfig = new DllConfig();
                }
            }
        }

        public void UnSetup()
        {
            manifestInfos = ManifestInfos;
        }
    }
}
