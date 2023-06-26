using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    /// <summary>
    /// 1 mono模式 2 ILRuntime模式 3 mono热重载模式
    /// </summary>
    public enum CodeMode
    {
        Mono = 1,
        ILRuntime = 2,
        Reload = 3,
    }

    public class Startup : MonoBehaviour
    {
        public void Awake ()
        {

        }

        public void Start ()
        {

        }

        public void Update ()
        {
            LemonFrameworkUpdate.Update ();
        }

        public void LateUpdate ()
        {
            LemonFrameworkLateUpdate.LateUpdate ();
        }

        public void FixedUpdate ()
        {
            LemonFrameworkFixedUpdate.FixedUpdate ();
        }

        public void OnApplicationFocus (bool focus)
        {
            LemonFrameworkFocus.OnApplicationFocus (focus);
        }

        public void OnApplicationPause (bool pause)
        {
            LemonFrameworkPause.OnApplicationPause (pause);
        }

        public void OnApplicationQuit ()
        {
            LemonFrameworkQuit.OnApplicationQuit ();
        }
    }
}
