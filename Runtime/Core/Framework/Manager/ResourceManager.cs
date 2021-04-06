using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UObject = UnityEngine.Object;

namespace Framework
{
    public class AssetBundleInfo
    {
        public AssetBundle m_AssetBundle;
        public int m_ReferencedCount;

        public AssetBundleInfo(AssetBundle assetBundle)
        {
            m_AssetBundle = assetBundle;
            m_ReferencedCount = 0;
        }
    }

    public class ResourceManager : Manager
    {
        class LoadAssetRequest
        {
            public Type assetType;
            public string[] assetNames;
            public Action<UObject[]> sharpFunc;
            public string ext;

            //zhf 添加只返回一个对象
            public Action<UObject> sharpOneFunc;
        }

        string m_BaseStreamingAssetsURL = "";
        string m_BasePersistentDataPathURL = "";
        string AssetResRootLow = AppConst.AssetResRoot.ToLower() + "/";

        string[] m_AllManifest = null;
        AssetBundleManifest m_AssetBundleManifest = null;
        Dictionary<string, AssetBundleInfo> m_LoadedAssetBundles = new Dictionary<string, AssetBundleInfo>();
        Dictionary<string, List<LoadAssetRequest>> m_LoadRequests = new Dictionary<string, List<LoadAssetRequest>>();
        Dictionary<string, int> m_loadedSceneDestory = new Dictionary<string, int>();
        Dictionary<string, string> ABFilesMd5 = null;

        // Load AssetBundleManifest.
        public void Initialize(string manifestName, Action initOK)
        {
            if (AppConst.UseStreamingAssets || (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor))
            {
                //初始化AB文件和MD5码的映射
                ABFilesMd5 = Util.ReadLocalFilesTxt(true, out string[] fileInfo);

                m_BaseStreamingAssetsURL = Util.GetStreamingAssetsPath() + AppConst.AssetResRoot + "/";
                m_BasePersistentDataPathURL = Util.DataPath + AppConst.AssetResRoot + "/";
                LoadAsset<AssetBundleManifest>(manifestName.ToLower(), new string[] { "AssetBundleManifest" }, delegate (UObject[] objs)
                {
                    if (objs.Length > 0)
                    {
                        m_AssetBundleManifest = objs[0] as AssetBundleManifest;
                        m_AllManifest = m_AssetBundleManifest.GetAllAssetBundles();
                    }
                    if (initOK != null) initOK();
                });
            }
            else
            {
                if (initOK != null) initOK();
            }
        }

        public void LoadPrefab(string abName, string assetName, Action<UObject[]> func)
        {
            LoadAsset<GameObject>(abName, new string[] { assetName }, func);
        }

        public void LoadPrefab(string abName, string[] assetNames, Action<UObject[]> func)
        {
            LoadAsset<GameObject>(abName, assetNames, func);
        }

        public void LoadPrefab(string abName, string[] assetNames)
        {
            LoadAsset<GameObject>(abName, assetNames, null);
        }

        //zhf 提供给ResourcesManager 的接口
        public void LoadAssetBundle(string abName, string assetName, string ext, Action<UObject> action = null, Type type = null, bool isSceneLoadedDestory = false)
        {
            if (type == null || type == typeof(GameObject))
                LoadAsset<GameObject>(abName, assetName, action, isSceneLoadedDestory, ext);
            else if (type == typeof(Texture2D))
                LoadAsset<Texture2D>(abName, assetName, action, isSceneLoadedDestory, ext);
        }

        string GetRealAssetPath(string abName)
        {
            if (abName.Equals(AppConst.AssetResRoot.ToLower()))
            {
                return abName;
            }
            abName = abName.ToLower();
            /*if (!abName.EndsWith(AppConst.ExtName))
            {
                abName += AppConst.ExtName;
            }*/
            if (abName.Contains("/"))
            {
                return abName;
            }
            //string[] paths = m_AssetBundleManifest.GetAllAssetBundles();  产生GC，需要缓存结果
            for (int i = 0; i < m_AllManifest.Length; i++)
            {
                int index = m_AllManifest[i].LastIndexOf('/');
                string path = m_AllManifest[i].Remove(0, index + 1);    //字符串操作函数都会产生GC
                if (path.Equals(abName))
                {
                    return m_AllManifest[i];
                }
            }
            Debug.LogError("GetRealAssetPath Error:>>" + abName);
            return null;
        }

        /// <summary>
        /// 载入素材
        /// </summary>
        void LoadAsset<T>(string abName, string[] assetNames, Action<UObject[]> action = null) where T : UObject
        {
            abName = GetRealAssetPath(abName);

            LoadAssetRequest request = new LoadAssetRequest();
            request.assetType = typeof(T);
            request.assetNames = assetNames;
            request.sharpFunc = action;

            List<LoadAssetRequest> requests = null;
            if (!m_LoadRequests.TryGetValue(abName, out requests))
            {
                requests = new List<LoadAssetRequest>();
                requests.Add(request);
                m_LoadRequests.Add(abName, requests);
                StartCoroutine(OnLoadAsset<T>(abName));
            }
            else
            {
                requests.Add(request);
            }
        }

        //zhf 添加修改过的接口
        void LoadAsset<T>(string abName, string assetName, Action<UObject> action = null, bool isLoadedSceneDestory = false, string ext = "") where T : UObject
        {
            abName = GetRealAssetPath(abName + "/" + assetName);

            //加入切换场景后自动删除的队列
            if (isLoadedSceneDestory)
                SetSceneLoadedDestroy(abName);

            LoadAssetRequest request = new LoadAssetRequest();
            request.assetType = typeof(T);
            request.assetNames = new string[] { assetName };
            request.sharpOneFunc = action;
            request.ext = ext;

            List<LoadAssetRequest> requests = null;
            if (!m_LoadRequests.TryGetValue(abName, out requests))
            {
                requests = new List<LoadAssetRequest>();
                requests.Add(request);
                m_LoadRequests.Add(abName, requests);
                StartCoroutine(OnLoadAsset<T>(abName));
            }
            else
            {
                requests.Add(request);
            }
        }

        IEnumerator OnLoadAsset<T>(string abName) where T : UObject
        {
            AssetBundleInfo bundleInfo = GetLoadedAssetBundle(abName);
            if (bundleInfo == null)
            {
                OnLoadAssetBundle(abName, typeof(T));

                bundleInfo = GetLoadedAssetBundle(abName);
                if (bundleInfo == null)
                {
                    m_LoadRequests.Remove(abName);
                    Debug.LogError("OnLoadAsset--->>>" + abName);
                    yield break;
                }
            }
            List<LoadAssetRequest> list = null;
            if (!m_LoadRequests.TryGetValue(abName, out list))
            {
                m_LoadRequests.Remove(abName);
                yield break;
            }
            for (int i = 0; i < list.Count; i++)
            {
                string[] assetNames = list[i].assetNames;
                List<UObject> result = new List<UObject>();

                AssetBundle ab = bundleInfo.m_AssetBundle;
                for (int j = 0; j < assetNames.Length; j++)
                {
                    string assetPath = assetNames[j];
                    if (list[i].ext != "")
                        assetPath += list[i].ext;

                    AssetBundleRequest request = ab.LoadAssetAsync(assetPath, list[i].assetType);
                    yield return request;
                    result.Add(request.asset);

                    //T assetObj = ab.LoadAsset<T>(assetPath);
                    //result.Add(assetObj);
                }
                if (list[i].sharpFunc != null)
                {
                    list[i].sharpFunc(result.ToArray());
                    list[i].sharpFunc = null;
                }

                //zhf 加载后的回调
                if (list[i].sharpOneFunc != null)
                {
                    list[i].sharpOneFunc(result[0]);
                    list[i].sharpOneFunc = null;
                }

                bundleInfo.m_ReferencedCount++;
            }
            m_LoadRequests.Remove(abName);
        }

        void OnLoadAssetBundle(string abName, Type type)
        {
            if (type == typeof(AssetBundleManifest))
            {
                DownLoadAssetBundle(abName);
            }
            else
            {
                DownLoadDependencies(abName);
                DownLoadAssetBundle(abName);
            }
        }

        AssetBundleInfo GetLoadedAssetBundle(string abName)
        {
            AssetBundleInfo bundle = null;
            m_LoadedAssetBundles.TryGetValue(abName, out bundle);
            if (bundle == null) return null;

            // No dependencies are recorded, only the bundle itself is required.
            string[] dependencies = GetDependence(abName);
            if (dependencies == null || dependencies.Length == 0)
                return bundle;

            // Make sure all dependencies are loaded
            foreach (var dependency in dependencies)
            {
                if (string.IsNullOrEmpty(dependency))
                    continue;

                AssetBundleInfo dependentBundle;
                m_LoadedAssetBundles.TryGetValue(dependency, out dependentBundle);
                if (dependentBundle == null)
                    return null;
            }
            return bundle;
        }

        //阻塞加载资源
        public UObject OnLoadAssetBundle(string abName, string assetName, string ext, Type resType, bool isLoadedSceneDestory = false)
        {
            AssetBundleInfo bundleInfo = GetLoadedAssetBundle(abName);
            if (bundleInfo == null)
            {
                DownLoadDependencies(abName);
                DownLoadAssetBundle(abName);

                bundleInfo = GetLoadedAssetBundle(abName);
            }
            if (bundleInfo != null)
            {
                AssetBundle ab = bundleInfo.m_AssetBundle;

                UObject obj = null;
                if (resType == typeof(Sprite))
                    obj = ab.LoadAsset<Sprite>(assetName + ext);
                else
                    obj =  ab.LoadAsset(assetName + ext);

                if (obj == null)
                {
                    Debug.LogError("未能从AssetBundle里找到文件:" + assetName);
                }

                bundleInfo.m_ReferencedCount++;

                if (isLoadedSceneDestory)
                    SetSceneLoadedDestroy(abName);

                return obj;
            }
            else
            {
                Debug.LogError("未找到资源文件" + abName);
            }

            return null;
        }

        //设置场景加载完毕后卸载
        public void SetSceneLoadedDestroy(string abName)
        {
            if (m_loadedSceneDestory.ContainsKey(abName))
            {
                int times = m_loadedSceneDestory[abName];
                m_loadedSceneDestory[abName] = ++times;
            }
            else
            {
                m_loadedSceneDestory.Add(abName, 1);
            }
        }

        //获取依赖文件
        public string[] GetDependence(string abName)
        {
            AssetBundleManifest manifest = this.GetAssetBundleManifest();
            if (manifest == null)
                return null;

            string[] strDependences = manifest.GetAllDependencies(abName);

            return strDependences;
        }

        public string GetExitFileUrl(string abName, bool isCheckStreamingAsset = true)
        {
            //文件名映射到MD5码
            if (!ABFilesMd5.TryGetValue(AssetResRootLow + abName, out string Md5aBName))
            {
                Debug.LogError(abName + "对应的MD5 码未在files.txt里找到");
                return string.Empty;
            }

            string url = m_BasePersistentDataPathURL + Md5aBName;
            if (!File.Exists(url))
            {
                url = Util.DataPath + Md5aBName;

                if(isCheckStreamingAsset)
                {
                    if (!File.Exists(url))
                        url = m_BaseStreamingAssetsURL + Md5aBName;
                }
                else
                {
                    //可读写目录不存在 且 不检查安装目录，则直接返回空
                    if (!File.Exists(url))
                        return string.Empty;
                }

                //Android 不能 如下检查资源是否存在
                //if (!File.Exists(url))
                //    return string.Empty;
            }

            return url;
        }

        //加载AssetBundle
        public void DownLoadAssetBundle(string abName)
        {
            if (m_LoadedAssetBundles.ContainsKey(abName))
                return;

            string url = GetExitFileUrl(abName);
            if (url == string.Empty)
                return;

            try
            {
                AssetBundle assetObj = AssetBundle.LoadFromFile(url);
                if (assetObj != null)
                {
                    if (!m_LoadedAssetBundles.ContainsKey(abName))
                    {
                        m_LoadedAssetBundles.Add(abName, new AssetBundleInfo(assetObj));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }

        //加载依赖文件
        public void DownLoadDependencies(string abName)
        {
            if (string.IsNullOrEmpty(abName))
                return;

            string[] dependencies = GetDependence(abName);

            AssetBundleInfo bundleInfo = null;
            if (dependencies.Length > 0)
            {
                for (int i = 0; i < dependencies.Length; i++)
                {
                    string depName = dependencies[i];

                    if (string.IsNullOrEmpty(depName))
                        continue;

                    if (m_LoadedAssetBundles.TryGetValue(depName, out bundleInfo))
                    {
                        bundleInfo.m_ReferencedCount++;
                    }
                    else
                    {
                        //先加载此AB，防止互相引用
                        DownLoadAssetBundle(depName);
                        bundleInfo = GetLoadedAssetBundle(depName);
                        if (bundleInfo != null)
                            bundleInfo.m_ReferencedCount++;

                        //迭代加载AB文件
                        DownLoadDependencies(depName);
                    }
                }
            }
        }

        public AssetBundleManifest GetAssetBundleManifest()
        {
            return m_AssetBundleManifest;
        }

        /// <summary>
        /// 加载完场景后卸载AB
        /// </summary>
        public void LoadedSceneDestory(bool isClearAll = false)
        {
            foreach (KeyValuePair<string,int>kv in m_loadedSceneDestory)
            {
                for(int i = 0;i < kv.Value;i++)
                {
                    UnloadAssetBundle(kv.Key);
                }
            }

            m_loadedSceneDestory.Clear();

            if(isClearAll)
            {
                m_LoadedAssetBundles.Clear();
                AssetBundle.UnloadAllAssetBundles(true);
            }
        }

        /// <summary>
        /// 此函数交给外部卸载专用，自己调整是否需要彻底清除AB
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="isThorough"></param>
        public void UnloadAssetBundle(string abName, bool isThorough = true)
        {
            abName = GetRealAssetPath(abName);
            //Debug.Log(m_LoadedAssetBundles.Count + " assetbundle(s) in memory before unloading " + abName);
            UnloadAssetBundleInternal(abName, isThorough);
            //Debug.Log(m_LoadedAssetBundles.Count + " assetbundle(s) in memory after unloading " + abName);
        }

        void UnloadDependencies(string abName, bool isThorough)
        {
            string[] dependencies = GetDependence(abName);
            if (dependencies.Length == 0)
            {
                return;
            }

            // Loop dependencies.
            foreach (var dependency in dependencies)
            {
                if (string.IsNullOrEmpty(dependency))
                    continue;
                UnloadAssetBundleInternal(dependency, isThorough);
                //递归删除
                //UnloadDependencies(dependency, isThorough);
            }
        }

        void UnloadAssetBundleInternal(string abName, bool isThorough)
        {
            AssetBundleInfo bundle = GetLoadedAssetBundle(abName);
            if (bundle == null)
                return;

            if (--bundle.m_ReferencedCount <= 0)
            {
                if (m_LoadRequests.ContainsKey(abName))
                {
                    return;     //如果当前AB处于Async Loading过程中，卸载会崩溃，只减去引用计数即可
                }

                if (abName.Contains("loading") || abName.Contains("common"))
                    return;

                bundle.m_AssetBundle.Unload(isThorough);
                m_LoadedAssetBundles.Remove(abName);
                Debug.Log(DateTime.Now.TimeOfDay.ToString() + abName + " has been unloaded successfully" );
                //在删除此AB后,此AB引用删除
                UnloadDependencies(abName, isThorough);
            }
        }
    }
}