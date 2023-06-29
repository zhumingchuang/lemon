using System.Collections.Generic;
using UnityEditor;

namespace LemonFramework.Editor
{
    public class BuildGroupModel: IEditorModel
    {
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
            }
        }

        private Dictionary<string, IEditorView> _editorViews;
        public Dictionary<string,IEditorView> EditorViews
        {
            get
            {
                return _editorViews;
            }
            set
            {
                _editorViews = value;
                if (_view is BuildGroupView)
                {
                    (_view as BuildGroupView).EditorViews = _editorViews;
                }
            }
        }

        private Dictionary<string, System.Func<bool, EditorWindow>> _editorViewsEvent;
        public Dictionary<string, System.Func<bool, EditorWindow>> EditorViewsEvent
        {
            get
            {
                return _editorViewsEvent;
            }
            set
            {
                _editorViewsEvent = value;
                if (_view is BuildGroupView)
                {
                    (_view as BuildGroupView).EditorViewsEvent = _editorViewsEvent;
                }
            }
        }

        public void Setup()
        {
            _editorViews = new Dictionary<string, IEditorView>();
            _editorViews.Add("BuildProjectSettings", BuildProjectEditor.BuildProjectSetings());
            //_editorViews.Add("AssemblySettings", AssemblyFileCreator.Create());
            //_editorViews.Add("MVPCreator", MVPFileCreator.Create());
            _editorViews.Add("CodesInfo", CodesInfo.CodesInfoMenu());
            EditorViews = _editorViews;

            _editorViewsEvent = new Dictionary<string, System.Func<bool, EditorWindow>>();
            EditorViewsEvent.Add("BuildProjectSettings", BuildProjectEditor.BuildProjectSetings);
            //EditorViewsEvent.Add("AssemblySettings", AssemblyFileCreator.Create);
            //EditorViewsEvent.Add("MVPCreator", MVPFileCreator.Create);
            EditorViewsEvent.Add("CodesInfo", CodesInfo.CodesInfoMenu);
            EditorViewsEvent = _editorViewsEvent;
        }

        public void UnSetup()
        {
            _editorViews.Clear();
        }
    }
}
