using System.Collections;
using System.Collections.Generic;
using UnityEditor;
namespace LemonFramework.Editor
{
    /// <summary>
    /// Build设置
    /// </summary>
    public static class BuildGroup
    {
        private static BuildGroupModel _model;
        [MenuItem(EditorDefine.BUILDGROUP)]
        public static void BuildGroupSettings()
        {
            if (_model != null && _model.View != null)
            {
                EditorViewManager.Close(_model);
            }
            _model = new BuildGroupModel();
            EditorViewManager.Show<BuildGroupView>(_model);
        }

    }
}
