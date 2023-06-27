using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework.ResModule
{
    public interface ILemonAsset 
    {
        T Get<T> () where T : UnityEngine.Object;
    }
}
