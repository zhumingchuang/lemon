//using CNFramework.UIModule;
//using System;
//using UnityEditor;
//using UnityEditor.UI;
//using UnityEngine;
//using UnityEngine.UI;

//namespace CNFramework.EditorUIModule
//{
//    [CanEditMultipleObjects]
//    [CustomEditor(typeof(ToggleEx), true)]
//    public class ToggleExEditor : ToggleEditor
//    {
//        private ToggleEx _src;

//        public ToggleExEditor() { }

//        protected override void OnEnable()
//        {
//            base.OnEnable();
//            _src = target as ToggleEx;
//        }

//        public override void OnInspectorGUI()
//        {
//            _src.text = EditorGUILayout.ObjectField("Text", _src.text, typeof(Text), true, Array.Empty<GUILayoutOption>()) as Text;
//            base.OnInspectorGUI();
//            if (_src == null || Application.isPlaying)
//            {
//                return;
//            }
//            EditorGUILayout.Space();
//            if (_src.text != null)
//            {
//                if (!_src.isOn && _src.interactable)
//                {
//                    _src.normalTextColor = _src.text.color;
//                }
//                _src.highlightedTextColor = EditorGUILayout.ColorField("HighlightedTextColor", _src.highlightedTextColor, Array.Empty<GUILayoutOption>());
//                _src.pressedTextColor = EditorGUILayout.ColorField("PressedTextColor", _src.pressedTextColor, Array.Empty<GUILayoutOption>());
//                _src.disabledTextColor = EditorGUILayout.ColorField("DisabledTextColor", _src.disabledTextColor, Array.Empty<GUILayoutOption>());
//                _src.isOnTextColor = EditorGUILayout.ColorField("IsOnTextColor", _src.isOnTextColor, Array.Empty<GUILayoutOption>());
//                if (_src.isOn && _src.interactable)
//                {
//                    _src.text.color = _src.isOnTextColor;
//                }
//                if (!_src.interactable)
//                {
//                    _src.text.color = _src.disabledTextColor;
//                }
//            }
//            base.serializedObject.ApplyModifiedProperties();
//        }
//    }
//}