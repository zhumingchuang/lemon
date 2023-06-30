using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LemonFramework
{
    public class Startup : MonoBehaviour, IStartup
    {
        public CodeMode CodeModeType
        {
            get;
            set;
        }

        public void Awake ()
        {
            DontDestroyOnLoad (gameObject);
        }

        public void Start ()
        {
            this.LoadLemonFramework ();
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
