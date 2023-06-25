using System;

namespace LemonFramework.Editor.UIModule
{
    public interface IEditorModel
    {
        IEditorView View
        {
            get;
            set;
        }

        void Setup ();

        void UnSetup ();
    }
}