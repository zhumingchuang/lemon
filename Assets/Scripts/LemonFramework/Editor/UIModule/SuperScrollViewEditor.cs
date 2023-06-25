//using System;
//using UnityEditor;
//using UnityEditor.AnimatedValues;
//using UnityEngine;
//using UnityEngine.UI;
//using CNFramework.UIModule;

//namespace CNFramework.EditorUIModule
//{
//    [CanEditMultipleObjects]
//    [CustomEditor(typeof(SuperScrollView), true)]
//    public class SuperScrollViewEditor : UnityEditor.Editor
//    {
//        private SerializedProperty m_Content;

//        private SerializedProperty m_Interactive;

//        private SerializedProperty m_SrollStatus;

//        private SerializedProperty m_MovementType;

//        private SerializedProperty m_Elasticity;

//        private SerializedProperty m_Inertia;

//        private SerializedProperty m_DecelerationRate;

//        private SerializedProperty m_ScrollSensitivity;

//        private SerializedProperty m_Viewport;

//        private SerializedProperty m_HorizontalScrollbar;

//        private SerializedProperty m_VerticalScrollbar;

//        private SerializedProperty m_HorizontalScrollbarVisibility;

//        private SerializedProperty m_VerticalScrollbarVisibility;

//        private SerializedProperty m_HorizontalScrollbarSpacing;

//        private SerializedProperty m_VerticalScrollbarSpacing;

//        private SerializedProperty m_OnValueChanged;

//        private AnimBool m_ShowElasticity;

//        private AnimBool m_ShowDecelerationRate;

//        private bool m_ViewportIsNotChild;

//        private bool m_HScrollbarIsNotChild;

//        private bool m_VScrollbarIsNotChild;

//        private static string s_HError;

//        private static string s_VError;

//        static SuperScrollViewEditor()
//        {
//            SuperScrollViewEditor.s_HError = "For this visibility mode, the Viewport property and the Horizontal Scrollbar property both needs to be set to a Rect Transform that is a child to the Scroll Rect.";
//            SuperScrollViewEditor.s_VError = "For this visibility mode, the Viewport property and the Vertical Scrollbar property both needs to be set to a Rect Transform that is a child to the Scroll Rect.";
//        }

//        public SuperScrollViewEditor()
//        {
//        }

//        private void CalculateCachedValues()
//        {
//            this.m_ViewportIsNotChild = false;
//            this.m_HScrollbarIsNotChild = false;
//            this.m_VScrollbarIsNotChild = false;
//            if ((int)base.targets.Length == 1)
//            {
//                Transform _transform = ((ScrollRect)base.target).transform;
//                if (this.m_Viewport.objectReferenceValue == null || ((RectTransform)this.m_Viewport.objectReferenceValue).transform.parent != _transform)
//                {
//                    this.m_ViewportIsNotChild = true;
//                }
//                if (this.m_HorizontalScrollbar.objectReferenceValue == null || ((Scrollbar)this.m_HorizontalScrollbar.objectReferenceValue).transform.parent != _transform)
//                {
//                    this.m_HScrollbarIsNotChild = true;
//                }
//                if (this.m_VerticalScrollbar.objectReferenceValue == null || ((Scrollbar)this.m_VerticalScrollbar.objectReferenceValue).transform.parent != _transform)
//                {
//                    this.m_VScrollbarIsNotChild = true;
//                }
//            }
//        }

//        protected virtual void OnDisable()
//        {
//            SuperScrollViewEditor superScrollViewEditor = this;
//            this.m_ShowElasticity.valueChanged.RemoveListener(Repaint);
//            SuperScrollViewEditor superScrollViewEditor1 = this;
//            this.m_ShowDecelerationRate.valueChanged.RemoveListener(Repaint);
//        }

//        protected virtual void OnEnable()
//        {
//            this.m_Content = base.serializedObject.FindProperty("m_Content");
//            this.m_Interactive = base.serializedObject.FindProperty("_interactive");
//            this.m_SrollStatus = base.serializedObject.FindProperty("_scrollStatus");
//            this.m_MovementType = base.serializedObject.FindProperty("m_MovementType");
//            this.m_Elasticity = base.serializedObject.FindProperty("m_Elasticity");
//            this.m_Inertia = base.serializedObject.FindProperty("m_Inertia");
//            this.m_DecelerationRate = base.serializedObject.FindProperty("m_DecelerationRate");
//            this.m_ScrollSensitivity = base.serializedObject.FindProperty("m_ScrollSensitivity");
//            this.m_Viewport = base.serializedObject.FindProperty("m_Viewport");
//            this.m_HorizontalScrollbar = base.serializedObject.FindProperty("m_HorizontalScrollbar");
//            this.m_VerticalScrollbar = base.serializedObject.FindProperty("m_VerticalScrollbar");
//            this.m_HorizontalScrollbarVisibility = base.serializedObject.FindProperty("m_HorizontalScrollbarVisibility");
//            this.m_VerticalScrollbarVisibility = base.serializedObject.FindProperty("m_VerticalScrollbarVisibility");
//            this.m_HorizontalScrollbarSpacing = base.serializedObject.FindProperty("m_HorizontalScrollbarSpacing");
//            this.m_VerticalScrollbarSpacing = base.serializedObject.FindProperty("m_VerticalScrollbarSpacing");
//            this.m_OnValueChanged = base.serializedObject.FindProperty("m_OnValueChanged");
//            SuperScrollViewEditor superScrollViewEditor = this;
//            this.m_ShowElasticity = new AnimBool(Repaint);
//            SuperScrollViewEditor superScrollViewEditor1 = this;
//            this.m_ShowDecelerationRate = new AnimBool(Repaint);
//            this.SetAnimBools(true);
//        }

//        public override void OnInspectorGUI()
//        {
//            this.SetAnimBools(false);
//            base.serializedObject.Update();
//            this.CalculateCachedValues();
//            EditorGUILayout.PropertyField(this.m_Content, Array.Empty<GUILayoutOption>());
//            EditorGUILayout.PropertyField(this.m_Interactive, Array.Empty<GUILayoutOption>());
//            EditorGUILayout.PropertyField(this.m_SrollStatus, Array.Empty<GUILayoutOption>());
//            EditorGUILayout.PropertyField(this.m_MovementType, Array.Empty<GUILayoutOption>());
//            if (EditorGUILayout.BeginFadeGroup(this.m_ShowElasticity.faded))
//            {
//                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
//                EditorGUILayout.PropertyField(this.m_Elasticity, Array.Empty<GUILayoutOption>());
//                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
//            }
//            EditorGUILayout.EndFadeGroup();
//            EditorGUILayout.PropertyField(this.m_Inertia, Array.Empty<GUILayoutOption>());
//            if (EditorGUILayout.BeginFadeGroup(this.m_ShowDecelerationRate.faded))
//            {
//                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
//                EditorGUILayout.PropertyField(this.m_DecelerationRate, Array.Empty<GUILayoutOption>());
//                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
//            }
//            EditorGUILayout.EndFadeGroup();
//            EditorGUILayout.PropertyField(this.m_ScrollSensitivity, Array.Empty<GUILayoutOption>());
//            EditorGUILayout.Space();
//            EditorGUILayout.PropertyField(this.m_Viewport, Array.Empty<GUILayoutOption>());
//            EditorGUILayout.PropertyField(this.m_HorizontalScrollbar, Array.Empty<GUILayoutOption>());
//            if (this.m_HorizontalScrollbar.objectReferenceValue && !this.m_HorizontalScrollbar.hasMultipleDifferentValues)
//            {
//                EditorGUI.indentLevel = (EditorGUI.indentLevel + 1);
//                EditorGUILayout.PropertyField(this.m_HorizontalScrollbarVisibility, EditorGUIUtility.TrTextContent("Visibility"), Array.Empty<GUILayoutOption>());
//                if (this.m_HorizontalScrollbarVisibility.enumValueIndex == 2 && !this.m_HorizontalScrollbarVisibility.hasMultipleDifferentValues)
//                {
//                    if (this.m_ViewportIsNotChild || this.m_HScrollbarIsNotChild)
//                    {
//                        EditorGUILayout.HelpBox(SuperScrollViewEditor.s_HError, MessageType.Error);
//                    }
//                    EditorGUILayout.PropertyField(this.m_HorizontalScrollbarSpacing, EditorGUIUtility.TrTextContent("Spacing"), Array.Empty<GUILayoutOption>());
//                }
//                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
//            }
//            EditorGUILayout.PropertyField(this.m_VerticalScrollbar, Array.Empty<GUILayoutOption>());
//            if (this.m_VerticalScrollbar.objectReferenceValue && !this.m_VerticalScrollbar.hasMultipleDifferentValues)
//            {
//                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
//                EditorGUILayout.PropertyField(this.m_VerticalScrollbarVisibility, EditorGUIUtility.TrTextContent("Visibility"), Array.Empty<GUILayoutOption>());
//                if (this.m_VerticalScrollbarVisibility.enumValueIndex == 2 && !this.m_VerticalScrollbarVisibility.hasMultipleDifferentValues)
//                {
//                    if (this.m_ViewportIsNotChild || this.m_VScrollbarIsNotChild)
//                    {
//                        EditorGUILayout.HelpBox(SuperScrollViewEditor.s_VError, MessageType.Error);
//                    }
//                    EditorGUILayout.PropertyField(this.m_VerticalScrollbarSpacing, EditorGUIUtility.TrTextContent("Spacing"), Array.Empty<GUILayoutOption>());
//                }
//                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
//            }
//            EditorGUILayout.Space();
//            EditorGUILayout.PropertyField(this.m_OnValueChanged, Array.Empty<GUILayoutOption>());
//            base.serializedObject.ApplyModifiedProperties();
//        }

//        private void SetAnimBool(AnimBool a, bool value, bool instant)
//        {
//            if (instant)
//            {
//                a.value = value;
//                return;
//            }
//            a.target = value;
//        }

//        private void SetAnimBools(bool instant)
//        {
//            this.SetAnimBool(this.m_ShowElasticity, (this.m_MovementType.hasMultipleDifferentValues ? false : this.m_MovementType.enumValueIndex == 1), instant);
//            this.SetAnimBool(this.m_ShowDecelerationRate, (this.m_Inertia.hasMultipleDifferentValues ? false : this.m_Inertia.boolValue), instant);
//        }
//    }
//}