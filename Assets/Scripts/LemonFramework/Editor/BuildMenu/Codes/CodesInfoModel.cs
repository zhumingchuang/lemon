using System;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

namespace LemonFramework.Editor
{
    public class CodesInfoModel : IEditorModel
    {
        public event Action<List<string>> OnCodesChangerEvent;

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
                if (_view is CodesInfoView)
                {
                    (_view as CodesInfoView).OnFocusEvent += OnFocus;
                    (_view as CodesInfoView).OnCodesChanger += OnCodesChanger;
                }
            }
        }

        private List<string> hotfixCodePath;
        public List<string> HotfixCodePath
        {
            get
            {
                return hotfixCodePath;
            }
            set
            {
                hotfixCodePath = value;
                if (_view is CodesInfoView)
                {
                    (_view as CodesInfoView).HotfixCodePath = hotfixCodePath;
                }
            }
        }

        private List<string> codesPath;
        public List<string> CodesPath
        {
            get
            {
                return codesPath;
            }
            set
            {
                codesPath = value;
                if (_view is CodesInfoView)
                {
                    (_view as CodesInfoView).CodesPath = codesPath;
                }
            }
        }
        
        private void OnCodesChanger(List<string> codesPath)
        {
            OnCodesChangerEvent?.Invoke(codesPath);
        }

        private void OnFocus()
        {
            UpdatePath();
        }

        public void UpdatePath()
        {
            HotfixCodePath.Clear();
            CodesPath.Clear();
            var selectAssembly = EditorPrefs.GetString(EditorDefine.SELECTASSEMBLY).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var referenced = AssemblyTools.FindAssemblyCNF(EditorDefine.LFHOTFIXASSEMBLY);
            for (int i = 0; i < selectAssembly.Length; i++)
            {
                if(referenced.Contains(selectAssembly[i]))
                {
                    string assetPath = AssetDatabaseTools.FindAssets(selectAssembly[i], ".asmdef");
                    HotfixCodePath.Add(assetPath);
                }
            }

            CodesPath = EditorPrefs.GetString(EditorDefine.CNFCodesPaht).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public void Setup()
        {
            HotfixCodePath = new List<string>();
            CodesPath = new List<string>();
            UpdatePath();
        }

        public void UnSetup()
        {

        }
    }
}
