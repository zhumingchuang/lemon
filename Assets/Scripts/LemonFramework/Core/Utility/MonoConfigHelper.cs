using System;
using UnityEngine;

namespace LemonFramework
{
    public static class MonoConfigHelper
    {
        public static string GetCodeConfig ()
        {
            try
            {
                //GameObject config = (GameObject)Resources.Load ("CodeConfig");
                //string configStr = config.GetComponent<ReferenceCollector> ().Get<TextAsset> ("CodeConfig").text;
                string configStr = "";
                return configStr;
            }
            catch (Exception e)
            {
                throw new Exception ($"load global config file fail", e);
            }
        }
    }

    public class DllConfig
    {
        public bool UseStreamingAssets;
        public string PlatformName;
        public string ManifestName;
        public string Uri;
    }
}
