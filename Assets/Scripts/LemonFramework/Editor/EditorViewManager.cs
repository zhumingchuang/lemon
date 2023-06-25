using LemonFramework.Editor.UIModule;
using System;
using UnityEditor;
using UnityEngine;

namespace LemonFramework.Editor
{
    internal static class EditorViewManager
    {
        public static void Close (IEditorModel model)
        {
            if (model != null)
            {
                model.UnSetup ();
                if (model.View != null)
                {
                    model.View.CloseView ();
                    model.View = null;
                }
            }
        }

        public static void Show<T> (IEditorModel model, bool show = true) where T : EditorWindow, IEditorView
        {
            T t = ScriptableObject.CreateInstance<T> ();
            if (model != null)
            {
                model.View = t;
                model.Setup ();
            }
            t.ShowView (show);
        }
    }
}