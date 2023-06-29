using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditorInternal;
using UnityEditor;

namespace LemonFramework.Editor
{
    public class CreateAssemblyModel : IEditorModel
    {
        private IEditorView _view;
        private string _keyword;
        public event Action<string> OnCreateEvent;
        public event Action<Dictionary<string, bool>> OnAssemblyChangerEvent;
        public IEditorView View
        {
            get
            {
                return _view;
            }
            set
            {
                _view = value;
                if (_view is CreateAssemblyView)
                {
                    (_view as CreateAssemblyView).OnCreateEvent += new Action(this.OnCreate);
                    (_view as CreateAssemblyView).OnAssemblyChangerEvent += OnAssemblyChanger;
                }
            }
        }

        public string Keyword
        {
            get
            {
                CreateAssemblyView view = View as CreateAssemblyView;
                if (view != null)
                {
                    _keyword = view.Keyword;
                }
                return _keyword;
            }
            set
            {
                _keyword = value;
                CreateAssemblyView view = View as CreateAssemblyView;
                if (view != null)
                {
                    view.Keyword = value;
                }
            }
        }

        private string[] selectAssembly;
        public string[] SelectAssembly
        {
            get
            {
                return selectAssembly;
            }
            set
            {
                selectAssembly = value;
                CreateAssemblyView view = View as CreateAssemblyView;
                if (view != null)
                {
                    view.SelectAssembly = selectAssembly;
                }
            }
        }

        public CreateAssemblyModel() { }

        private void OnCreate()
        {
            Action<string> action = OnCreateEvent;
            action?.Invoke(Keyword);
        }
        private void OnAssemblyChanger(Dictionary<string, bool> assembly)
        {
            Action<Dictionary<string, bool>> action = OnAssemblyChangerEvent;
            action?.Invoke(assembly);
        }

        public void Setup()
        {
            Keyword = _keyword;
            SelectAssembly = EditorPrefs.GetString(EditorDefine.SELECTASSEMBLY).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        }

        public void UnSetup()
        {
            _keyword = Keyword;
        }
    }
}
