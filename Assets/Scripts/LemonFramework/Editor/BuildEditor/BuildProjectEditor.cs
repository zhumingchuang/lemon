using LemonFramework.ResModule;
using PlasticGui.Help.Conditions;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LemonFramework.Editor
{
    public class BuildProjectEditor
    {
        private static BuildProjectModel _model;


        [MenuItem (EditorDefine.BUILDPROJECTSETINGS)]
        public static void BuildProjectSettingsMenu ()
        {
            BuildProjectSetings (true);
        }

        public static BuildProjectView BuildProjectSetings (bool show = false)
        {
            if (_model != null && _model.View != null)
            {
                EditorViewManager.Close (_model);
            }
            _model = new BuildProjectModel ();
            _model.OnCreateEvent += OnCreate;
            EditorViewManager.Show<BuildProjectView> (_model, show);
            return _model.View as BuildProjectView;
        }

        private static void OnCreate (string[] url, List<ManifestInfo> manifestInfos)
        {
            if (string.IsNullOrEmpty (url[0]) || string.IsNullOrEmpty (url[1]))
            {
                ((BuildProjectView)_model.View).ShowNotification (new GUIContent ("关键词不能为空!!!"));
                return;
            }

            string dllConfig = JsonUtility.ToJson (_model.DllConfig);
            File.WriteAllText (Path.Combine (EditorDefine.CONFIGDIR, "CodeConfig.txt"), dllConfig, System.Text.Encoding.UTF8);


            if (url[0] != GlobalProto.AssetBundleServerUrl || url[1] != GlobalProto.Address)
            {
                var hotfixAssembly = AssetDatabaseTools.FindAssets (EditorDefine.LFHOTFIXASSEMBLY, ".asmdef");
                var globalProtoPath = AssetDatabaseTools.FindAssets ("GlobalProto", ".cs", Path.GetDirectoryName (hotfixAssembly));
                var data = File.ReadAllText (globalProtoPath, System.Text.Encoding.UTF8);

                if (url[0] != GlobalProto.AssetBundleServerUrl)
                {
                    string filter1 = "AssetBundleServerUrl =";
                    var serverUrl = RegexUtility.GetMiddleValue (data, filter1, ";");
                    data = data.Replace (filter1 + serverUrl, $@"{filter1}""{url[0]}""");
                }

                if (url[0] != GlobalProto.AssetBundleServerUrl)
                {
                    string filter1 = "Address =";
                    var Address = RegexUtility.GetMiddleValue (data, filter1, ";");
                    data = data.Replace (filter1 + Address, $@"{filter1}""{url[1]}""");
                }

                File.WriteAllText (globalProtoPath, data, System.Text.Encoding.UTF8);
            }

            string manifestJson = JsonUtility.ToJson (manifestInfos);
            System.IO.File.WriteAllText (Path.Combine (EditorDefine.CONFIGDIR, "ManifestsConfig.txt"), manifestJson, System.Text.Encoding.UTF8);
            AssetDatabase.Refresh ();
        }
    }
}
