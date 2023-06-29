using System;

namespace LemonFramework.Editor
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