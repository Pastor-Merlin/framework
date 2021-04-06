using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UObject = UnityEngine.Object;

#if UNITY_EDITOR 
using UnityEditor;
#endif

namespace Framework
{
    public class ResourcesManager
    {
        public static bool USE_AB = false;

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        private static ResourcesManager instance;
        public static ResourcesManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ResourcesManager();
                }
                return instance;
            }
        }

        //AB资源加载类
        private ResourceManager m_resMgr;
        ResourceManager resMgr
        {
            get
            {
                if (m_resMgr == null)
                    m_resMgr = AppFacade.Instance.GetManager<ResourceManager>(ManagerName.Resource);
                return m_resMgr;
            }
        }
        //Resources 资源加载类
        private ResourcesLoad resLoad = ResourcesLoad.Instance;

        /// <summary>
        /// 初始化数据
        /// </summary>
        private ResourcesManager()
        {
            resLoad.Init();
            this.AppContentPath();
        }

        /// <summary>
        /// 获取资源全路径
        /// </summary>
        /// <param name="resPath"></param>
        /// <returns></returns>
        public string GetFileFullName(string resPath)
        {
            return resPath;
        }

        /// <summary>
        /// 外部帧调用
        /// </summary>
        public void OnUpdate()
        {
            resLoad.Update();
        }

        private void AppContentPath()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    USE_AB = true;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    USE_AB = true;
                    break;
                default:
#if UNITY_EDITOR
                    USE_AB = false;  // 编辑器不使用AB

                    if (AppConst.UseStreamingAssets)
                        USE_AB = true;
#else
                USE_AB = true;   // PC 版本使用AB
#endif
                    break;
            }
        }

        public void Initialize(string manifestName, Action initOK)
        {
            resMgr.Initialize(manifestName, initOK);
        }

        //场景加载完卸载AB资源
        public void LoadedSceneDestory(bool isClearAll = false)
        {
            resMgr.LoadedSceneDestory(isClearAll);
        }

        public string GetExitFileUrl(string fileName, bool isCheckStreamingAsset = true)
        {
            return resMgr.GetExitFileUrl(fileName, isCheckStreamingAsset);
        }

        //同步阻塞加载,用于prefab打成单个资源包的情况
        /// <summary>
        /// UObject DownAsset("SelfResources/Prefabs/Textures/UI/xxx","zzz",".png",typeof(Sprite),false) as Sprite;
        /// </summary>
        /// <param name="assetbundle"></param>
        /// <param name="assetName"></param>
        /// <param name="ext"></param>
        /// <param name="resType"></param>
        /// <param name="instantiate"></param>
        /// <param name="isSceneLoadedDestory"></param>
        /// <returns></returns>
        public UObject DownAsset(string assetbundle, string assetName, string ext, System.Type resType, bool instantiate, bool isSceneLoadedDestory = false)
        {
            UObject prefab;
            //使用一般的资源，开发时候使用
            if (!USE_AB)
                prefab = DownAssetRes(assetbundle, assetName, ext, resType, instantiate);
            else//使用AssetBundle，打包成PC，Android IOS
                prefab = DownAssetAB(assetbundle, assetName, ext, resType, instantiate, isSceneLoadedDestory);
            return prefab;
        }

        //同步阻塞加载assetbundle内资源 ,用于许多资源打成单个资源包的情况
        /// <summary>
        /// TextAsset text = ResourcesManager.Instance.LoadAsset("XXX/YYY/ZZZ", "bag", ".txt", typeof(TextAsset), false) as TextAsset;
        /// SpriteAtlas atlas = ResourcesManager.Instance.LoadAsset("XXX/YYY/ZZZ", "items", ".spriteatlas", typeof(SpriteAtlas), false) SpriteAtlas;
        /// </summary>
        /// <param name="assetbundle"></param>
        /// <param name="assetName"></param>
        /// <param name="ext"></param>
        /// <param name="resType"></param>
        /// <param name="instantiate"></param>
        /// <param name="isSceneLoadedDestory"></param>
        /// <returns></returns>
        public UObject LoadAsset(string assetbundle, string assetName, string ext, System.Type resType, bool instantiate, bool isSceneLoadedDestory = false)
        {
            UObject asset;
            if (!USE_AB)
                asset = DownAssetRes(assetbundle, assetName, ext, resType, instantiate);
            else
                asset = LoadAssetAB(assetbundle, assetName, ext, resType, instantiate, isSceneLoadedDestory);
            return asset;
        }

        //阻塞加载一般资源
        public UObject DownAssetRes(string assetbundle, string assetName, string ext, System.Type resType, bool instantiate)
        {
            if (assetName != string.Empty && assetName != "")
                assetbundle = assetbundle + "/" + assetName;

            assetbundle += ext;
            UObject prefab = null;
#if UNITY_EDITOR
            if (resType == null)
                resType = typeof(UObject);

            UObject asset = AssetDatabase.LoadAssetAtPath("Assets/" + assetbundle, resType);
            if (asset == null)
                Debug.LogError(assetbundle + "不存在");

            if (instantiate == true && asset != null)
                prefab = UObject.Instantiate(asset);
            else
                prefab = asset;
#else
        Debug.LogError("[ResourcesManager.DownAssetLoad] 非Editor 环境 请使用AB接口调用");   
#endif

            return prefab;
        }

        //阻塞加载AB
        public UObject DownAssetAB(string assetbundle, string assetName, string ext, System.Type resType, bool instantiate, bool isSceneLoadedDestory)
        {
            //如果没有资源名，默认截取assetbundle最后一个
            if (string.IsNullOrEmpty(assetName))
            {
                assetName = assetbundle.Substring(assetbundle.LastIndexOf("/") + 1);
            }
            else
            {
                assetbundle = assetbundle + "/" + assetName;
            }
            UObject asset = resMgr.OnLoadAssetBundle(assetbundle.ToLower(), assetName.ToLower(), ext, resType, isSceneLoadedDestory);
            if (instantiate && asset != null)
            {
                asset = UObject.Instantiate(asset);
            }
            return asset;
        }

        //阻塞加载AB
        public UObject LoadAssetAB(string assetbundle, string assetName, string ext, System.Type resType, bool instantiate, bool isSceneLoadedDestory)
        {
            UObject asset = resMgr.OnLoadAssetBundle(assetbundle.ToLower(), assetName.ToLower(), ext, resType, isSceneLoadedDestory);
            if (instantiate && asset != null)
            {
                asset = UObject.Instantiate(asset);
            }
            return asset;
        }

        //异步加载资源
        public void DownAssetSync(string assetbundle, string assetName, string ext = null, Action<UObject> action = null, Type type = null, bool isSceneLoadedDestory = false)
        {
            if (!USE_AB)
                DownAssetResSync(assetbundle, assetName, ext, action, type);
            else//使用AssetBundle
                DownAssetABSync(assetbundle, assetName, ext,  action, type, isSceneLoadedDestory);
        }

        //异步加载一般的资源
        public void DownAssetResSync(string assetbundle, string assetName, string ext, Action<UObject> action = null, Type type = null)
        {
            assetbundle = "Assets/" + assetbundle;
            if (!string.IsNullOrEmpty(assetName))
                assetbundle = assetbundle + "/" + assetName;
            //资源加载
            assetbundle += ext;
            resLoad.LoadAsync(assetbundle, action, type, false);
        }

        //异步加载AB下的资源
        public void DownAssetABSync(string assetbundle, string assetName, string ext, Action<UObject> action = null, Type type = null, bool isSceneLoadedDestory = false)
        {
            //如果没有资源名，默认截取assetbundle最后一个
            if (string.IsNullOrEmpty(assetName))
            {
                assetName = assetbundle.Substring(assetbundle.LastIndexOf("/") + 1);
                assetbundle = assetbundle.Substring(0, assetbundle.LastIndexOf("/"));
            }
            //assetbundle = assetbundle.Replace('/', '-');

            resMgr.LoadAssetBundle(assetbundle, assetName, ext, action, type, isSceneLoadedDestory);
        }

        public void Unload(string fullPath)
        {
            if (!USE_AB)
                return;
            else
                resMgr.UnloadAssetBundle(fullPath);
        }
    }
}