using System;
using UnityEditor;

namespace LemonFramework.Editor.UIModule
{
    public class CreateMVPModel : IEditorModel
    {
        private IEditorView _view;

        private string _keyword;

        public event Action<string> OnCreateEvent;

        public string Keyword
        {
            get
            {
                CreateMVPView view = View as CreateMVPView;
                if (view != null)
                {
                    _keyword = view.Keyword;
                }
                return _keyword;
            }
            set
            {
                _keyword = value;
                CreateMVPView view = View as CreateMVPView;
                if (view != null)
                {
                    view.Keyword = value;
                }
            }
        }

        public IEditorView View
        {
            get
            {
                return _view;
            }
            set
            {
                _view = value;
                if (_view is CreateMVPView)
                {
                    (_view as CreateMVPView).OnCreateEvent += new Action(this.OnCreate);
                }
            }
        }

        private string[] cnFAssembly;
        public string[] CNFAssembly
        {
            get
            {
                return cnFAssembly;
            }
            set
            {
                cnFAssembly = value;
                CreateMVPView view = View as CreateMVPView;
                if (view != null)
                {
                    view.CNFAssembly = value;
                }
            }
        }

        private string selectAssemblyPath;
        public string SelectAssemblyPath
        {
            get
            {
                CreateMVPView view = View as CreateMVPView;
                if(view!=null)
                {
                    var uiAssemblys = AssetDatabase.FindAssets(view.SelectAssemblyName);
                    for (int i = 0; i < uiAssemblys.Length; i++)
                    {
                        var path = AssetDatabase.GUIDToAssetPath(uiAssemblys[i]);
                        if (path.EndsWith(".asmdef"))
                        {
                            selectAssemblyPath = path;
                            return selectAssemblyPath;
                        }
                    }
                }
                return string.Empty;
            }
            set
            {
                selectAssemblyPath = value;
                CreateMVPView view = View as CreateMVPView;
                if (view != null)
                {
                    view.SelectAssemblyName = System.IO.Path.GetFileNameWithoutExtension(selectAssemblyPath);
                }
            }
        }


        public CreateMVPModel() { }

        private void OnCreate()
        {
            Action<string> action = OnCreateEvent;
            action?.Invoke(Keyword);
        }

        public void Setup()
        {
            Keyword = _keyword;
            CNFAssembly = AssemblyTools.FindAssemblyCNF(EditorDefine.CNFHOTFIXASSEMBLY).ToArray();
            var uiAssemblys= AssetDatabase.FindAssets(EditorPrefs.GetString(EditorDefine.SELECTUIASSEMBLY));
            for (int i = 0; i < uiAssemblys.Length; i++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(uiAssemblys[i]);
                if (assetPath.EndsWith(".asmdef"))
                {
                    SelectAssemblyPath = assetPath;
                    break;
                }
            }

            CreateMVPView view = View as CreateMVPView;;
            for (int i = 0; i < CNFAssembly.Length; i++)
            {
                if (CNFAssembly[i] == view.SelectAssemblyName)
                {
                    view.selectIndex = i;
                    break;
                }
            }
        }

        public void UnSetup()
        {
            _keyword = Keyword;
        }
    }
}