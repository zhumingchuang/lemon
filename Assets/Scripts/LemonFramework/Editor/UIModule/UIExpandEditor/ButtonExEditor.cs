//using System;
//using UnityEditor;
//using UnityEditor.UI;
//using UnityEngine;
//using UnityEngine.UI;
//using CNFramework.UIModule;

//namespace CNFramework.EditorUIModule
//{
//    [CanEditMultipleObjects]
//    [CustomEditor(typeof(ButtonEx), true)]
//    public class ButtonExEditor : ButtonEditor
//    {
//        private ButtonEx _src;

//        public ButtonExEditor()
//        {
//        }

//        protected override void OnEnable()
//        {
//            base.OnEnable();
//            this._src = base.target as ButtonEx;
//        }

//        public override void OnInspectorGUI()
//        {
//            this._src.text = EditorGUILayout.ObjectField("Text", this._src.text, typeof(Text), true, Array.Empty<GUILayoutOption>()) as Text;
//            base.OnInspectorGUI();
//            if (this._src == null || Application.isPlaying)
//            {
//                return;
//            }
//            EditorGUILayout.Space();
//            if (this._src.text != null)
//            {
//                if (this._src.interactable)
//                {
//                    this._src.normalTextColor = this._src.text.color;
//                }
//                this._src.highlightedTextColor = EditorGUILayout.ColorField("HighlightedTextColor", this._src.highlightedTextColor, Array.Empty<GUILayoutOption>());
//                this._src.pressedTextColor = EditorGUILayout.ColorField("PressedTextColor", this._src.pressedTextColor, Array.Empty<GUILayoutOption>());
//                this._src.disabledTextColor = EditorGUILayout.ColorField("DisabledTextColor", this._src.disabledTextColor, Array.Empty<GUILayoutOption>());
//                if (!this._src.interactable)
//                {
//                    this._src.text.color = this._src.disabledTextColor;
//                }
//            }
//            base.serializedObject.ApplyModifiedProperties();
//        }
//    }
//}