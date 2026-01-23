using System.IO;
using UnityEngine;

namespace PridePerception.util {
    public static class ModPaths {
        public static string AssetPath => Path.Combine(Application.streamingAssetsPath, "Modded", "blitzo.baldiplus.prideperception", "Images");
        public static string GetPath(string fileName) => Path.Combine(AssetPath, fileName);
    }
}