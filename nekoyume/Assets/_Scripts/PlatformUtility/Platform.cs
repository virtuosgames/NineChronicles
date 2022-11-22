using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Nekoyume
{
    public class Platform
    {
        public static string PersistentDataPath
        {
            get { return Application.persistentDataPath; }
        }

        public static string GetPersistentDataPath(string fileName)
        {
            return Path.Combine(Platform.PersistentDataPath, fileName);
        }

        public static string StreamingAssetsPath
        {
            get { return Application.streamingAssetsPath; }
        }

        public static string GetStreamingAssetsPath(string fileName)
        {
            return Path.Combine(Platform.StreamingAssetsPath, fileName);
        }

        public static string DataPath
        {
            get { return Application.dataPath; }
        }

        public static string GetDataPath(string fileName)
        {
            return Path.Combine(Platform.DataPath, fileName);
        }

        public static RuntimePlatform CurrentPlatform
        {
            get { return Application.platform; }
        }

        public static bool IsTargetPlatform(RuntimePlatform platform)
        {
            return CurrentPlatform == platform;
        }

        public static bool IsMobilePlatform()
        {
            return Platform.IsTargetPlatform(RuntimePlatform.Android) ||
                   Platform.IsTargetPlatform(RuntimePlatform.IPhonePlayer);
        }
    }
}