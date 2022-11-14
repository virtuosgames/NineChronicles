using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
    public enum LocalizationType
    {
        CsvCfg = 0,
        GenesisBlock = 1,
        JsonCfg = 2,
    }

    public class LocalizationData
    {
        public string name;
        public byte[] bytes;
        public string text;
        public LocalizationType type;
    }

    public class Localization : MonoBehaviour
    {
        private static Localization _instance;
        private Dictionary<string, LocalizationData> localizationInfo = new Dictionary<string, LocalizationData>();

        public static Localization instance
        {
            get
            {
                if (_instance)
                    return _instance;

                _instance = new GameObject(typeof(Localization).ToString(), typeof(Localization))
                    .GetComponent<Localization>();

                return _instance;
            }
        }

        public IEnumerator LoadLocalization(Action<bool> callback)
        {
            Application.backgroundLoadingPriority = ThreadPriority.High;

            var configInitialized = false;
            StartCoroutine(LoadCfg(Application.streamingAssetsPath + "/localizationcfg.unity3d",
                LocalizationType.CsvCfg,
                succeed =>
                {
                    Debug.Log($"Localization Config initialized.");
                    configInitialized = true;
                }
            ));
            yield return new WaitUntil(() => configInitialized);
            configInitialized = false;
            StartCoroutine(LoadCfg(Application.streamingAssetsPath + "/genesis-block.unity3d",
                LocalizationType.GenesisBlock,
                succeed =>
                {
                    Debug.Log($"Genesis-Block initialized.");
                    configInitialized = true;
                }
            ));
            yield return new WaitUntil(() => configInitialized);

            configInitialized = false;
            StartCoroutine(LoadCfg(Application.streamingAssetsPath + "/jsoncfg.unity3d", LocalizationType.JsonCfg,
                succeed =>
                {
                    Debug.Log($"Json Config initialized.");
                    configInitialized = true;
                }
            ));
            yield return new WaitUntil(() => configInitialized);

            Application.backgroundLoadingPriority = ThreadPriority.Low;
            callback(true);
        }

        public IEnumerator LoadCfg(string url, LocalizationType type, Action<bool> callback)
        {
            var www = new WWW(url);
            yield return www;
            if (www.error == null)
            {
                AssetBundle bundle = www.assetBundle;
                var assetRequest = bundle.LoadAllAssetsAsync();
                yield return assetRequest;
                if (assetRequest.isDone == true)
                {
                    var loadedAssets = assetRequest.allAssets;

                    for (int i = 0; i < loadedAssets.Length; ++i)
                    {
                        var textAsset = loadedAssets[i] as TextAsset;
                        AddLocalizationInfo(textAsset, type);
                    }
                }
            }
            else
            {
                Debug.Log(www.error);
            }

            callback(true);
        }

        private void AddLocalizationInfo(TextAsset data, LocalizationType type)
        {
            var dataInfo = new LocalizationData();
            dataInfo.name = data.name;
            dataInfo.bytes = data.bytes;
            dataInfo.text = data.text;
            dataInfo.type = type;

            if (localizationInfo.ContainsKey(data.name))
            {
                localizationInfo[dataInfo.name] = dataInfo;
            }
            else
            {
                localizationInfo.Add(dataInfo.name, dataInfo);
            }
        }

        public List<LocalizationData> GetDatasByType(LocalizationType type)
        {
            List<LocalizationData> datas = new List<LocalizationData>();
            foreach (var itr in localizationInfo.Values)
            {
                if (itr.type == type)
                {
                    datas.Add(itr);
                }
            }

            return datas;
        }


        public LocalizationData GetDataInfo(string name)
        {
            if (localizationInfo.ContainsKey(name))
            {
                return localizationInfo[name];
            }

            return null;
        }
    }
}