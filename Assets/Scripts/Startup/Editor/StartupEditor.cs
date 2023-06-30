using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace LemonFramework.Editor
{
    [CustomEditor (typeof (LemonFramework.Startup), true)]
    public class StartupEditorInspector : UnityEditor.Editor
    {
        private LemonFramework.Startup startupTarget;

        private void OnEnable ()
        {
            startupTarget = (LemonFramework.Startup)target;
            
        }

        public override void OnInspectorGUI ()
        {
            base.OnInspectorGUI ();

            GUILayout.BeginHorizontal ();
            {
                EditorGUILayout.LabelField ("Default Manifest:", GUILayout.MinWidth (EditorGUIUtility.labelWidth));
                //startupTarget .CodeModeType=(LemonFramework.CodeMode) EditorGUILayout.EnumFlagsField (startupTarget.CodeModeType);
            }
            GUILayout.EndHorizontal ();


        }
    }
}
