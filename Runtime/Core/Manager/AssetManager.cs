using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace lus.framework
{
    [DisallowMultipleComponent]
    public class AssetManager : SingletonMono<AssetManager>
    {
        [HideInInspector]
        public bool bUseAB = false;
        public Action LoadAssetAction;
        public Dictionary<string, AssetBundle> allManifestDic = new Dictionary<string, AssetBundle>();
        private const string AssetResRoot = "MyAssetBundles";
        private const string filesName = "files.txt";

        protected override void Awake()
        {
            base.Awake();
        }

        void Start()
        {
            if (bUseAB)
            {
                var ABFilesMd5 = Utility.ReadLocalFilesTxt(filesName, AssetResRoot, true, out string[] fileInfo);
                foreach (var go in ABFilesMd5)
                {
                    var path = Application.streamingAssetsPath + "/" + AssetResRoot + "/" + go.Value;
                    if (!File.Exists(path))
                    {
                        path = Application.persistentDataPath + "/" + AssetResRoot + "/" + go.Value;
                    }
                    if (File.Exists(path))
                    {
                        var myAssetBundle = AssetBundle.LoadFromFile(path);
                        if (myAssetBundle != null)
                            allManifestDic.Add(go.Key, myAssetBundle);
                    }
                }

                if(LoadAssetAction != null)
                {
                    LoadAssetAction();
                }
            }
        }

        void OnDestroy()
        {
            Unload();
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        public T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            if (!bUseAB)
            {
                if (typeof(T) == typeof(SpriteAtlas))
                {
                    var name = path.Substring(path.LastIndexOf("/") + 1, path.Length - path.LastIndexOf("/") - 1);
                    return AssetDatabase.LoadAssetAtPath<T>("Assets/" + path + "/" + name + ".spriteatlas");
                }
                else
                    return AssetDatabase.LoadAssetAtPath<T>("Assets/" + path);
            }
            else
            {
               return DoLoadAsset<T>(path);
            }
#else
            return DoLoadAsset<T>(path);
#endif
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        public void Unload()
        {
            foreach (var go in allManifestDic)
            {
                go.Value.Unload(true);
            }
            allManifestDic.Clear();
        }

        #region  PRIVATE
        protected T DoLoadAsset<T>(string path) where T : UnityEngine.Object
        {
            if (typeof(T) == typeof(SpriteAtlas))
            {
                path = "/" + path.ToLower();
                var fullPath = "Assets" + path;
                fullPath = fullPath.ToLower();
                var name = path.Substring(path.LastIndexOf("/") + 1, path.Length - path.LastIndexOf("/") - 1);
                return DoLoadAsset<T>(path, fullPath + "/" + name + ".spriteatlas");
            }
            else
            {
                path = "/" + path.ToLower();
                var fullPath = "Assets" + path;
                fullPath = fullPath.ToLower();
                path = path.Substring(0, path.LastIndexOf("."));
                return DoLoadAsset<T>(path, fullPath);
            }
        }
        protected T DoLoadAsset<T>(string path, string fullPath) where T : UnityEngine.Object
        {
            if (allManifestDic.ContainsKey(path))
            {
                return allManifestDic[path].LoadAsset<T>(fullPath) as T;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}