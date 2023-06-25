namespace LemonFramework.Editor.UIModule
{
    /// <summary>
    /// 编辑器窗口接口
    /// </summary>
    public interface IEditorView
    {
        void CloseView ();
        void ShowView (bool auto);
    }
}