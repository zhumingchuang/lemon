namespace LemonFramework.ResModule
{
    public static class GlobalProto
    {
        public const string AssetBundleServerUrl = "http://192.168.2.161:8089/Bundles/";
        public const string Address = "http://192.168.2.161:8089/Bundles/";
    }

    public class AssetAllManifest
    {
        public ManifestInfo[] manifestInfos = new ManifestInfo[]
        {
            new ManifestInfo(){ name="MinManifest",autoUpdate=false},
            //new ManifestInfo(){ name="CNFManifest",autoUpdate=false},
        };
    }
}
