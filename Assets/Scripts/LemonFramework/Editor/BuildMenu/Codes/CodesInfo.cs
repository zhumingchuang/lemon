using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Text;

namespace LemonFramework.Editor
{
    public static class CodesInfo
    {
        public static CodesInfoModel _model { get; private set; }
        public static CodesInfoView CodesInfoMenu(bool show = false)
        {
            if (_model != null && _model.View != null)
            {
                EditorViewManager.Close(_model);
            }
            _model = new CodesInfoModel();
            EditorViewManager.Show<CodesInfoView>(_model, show);
            _model.OnCodesChangerEvent += OnCodesChanger;
            return _model.View as CodesInfoView;
        }

        public static void OnCodesChanger(List<string> codesPath)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < codesPath.Count; i++)
            {
                sb.Append(codesPath[i] + ",");
            }
            EditorPrefs.SetString(EditorDefine.CNFCodesPaht, sb.ToString());
        }
    }
}
