using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    /// <summary>
    ///     版本信息
    /// </summary>
    [Serializable]
    public class ManifestInfo
    {
        /// <summary>
        ///     清单文件的名字
        /// </summary>
        public string name;

        /// <summary>
        ///     是否自动更新
        /// </summary>
        public bool autoUpdate;
    }
}
