using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    public interface IStartup
    {
        CodeMode CodeModeType { get; }
    }
}
