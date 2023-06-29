using System;
using UnityEditor;
using UnityEngine;

namespace LemonFramework.Editor.UIModule
{
    public class ViewNodeSeacher
    {
        public ViewNodeSeacher()
        {
        }

        private static void CopyContent(string content)
        {
            TextEditor textEditor = new TextEditor();
            textEditor.text=content;
            textEditor.OnFocus();
            textEditor.Copy();
        }

        private static void CutString(string cutFlag, int flagCount, ref string content)
        {
            if (flagCount > 0)
            {
                int num = content.IndexOf(cutFlag);
                if (num >= 0)
                {
                    content = content.Substring(num + 1);
                    int num1 = flagCount - 1;
                    flagCount = num1;
                    ViewNodeSeacher.CutString(cutFlag, num1, ref content);
                    return;
                }
                content = string.Empty;
            }
        }

        private static void GetNodePath(Transform nodeTrans, ref string path)
        {
            if (nodeTrans != null)
            {
                string _name = nodeTrans.name;
                path = string.Format("{0}{1}", _name, (string.IsNullOrEmpty(path) ? string.Empty : string.Format("/{0}", path)));
                Transform _parent = nodeTrans.parent;
                if (_parent != null)
                {
                    ViewNodeSeacher.GetNodePath(_parent, ref path);
                }
            }
        }

        private static string Search()
        {
            Transform _activeTransform = Selection.activeTransform;
            string empty = string.Empty;
            ViewNodeSeacher.GetNodePath(_activeTransform, ref empty);
            return empty;
        }

        [MenuItem("GameObject/LemonFramework/UISearcher(Canvas) &C", false, 11)]
        private static void SearchByCanvas()
        {
            string str = ViewNodeSeacher.Search();
            if (string.IsNullOrEmpty(str))
            {
                Debug.Log("<color=yellow>## Uni Warning <Ming>## Path Empty</color>");
                return;
            }
            Debug.LogFormat("Uni Log Copy Done: {0}", new object[] { str });
            ViewNodeSeacher.CopyContent(str);
        }

        [MenuItem("GameObject/LemonFramework/UISearcher(UIRoot) &R", false, 11)]
        private static void SearchByRoot()
        {
            string str = ViewNodeSeacher.Search();
            ViewNodeSeacher.CutString("/", 2, ref str);
            if (string.IsNullOrEmpty(str))
            {
                Debug.Log("<color=yellow> Uni Warning Path Empty</color>");
                return;
            }
            Debug.LogFormat(" Uni Log Copy Done: {0}", new object[] { str });
            ViewNodeSeacher.CopyContent(str);
        }
    }
}