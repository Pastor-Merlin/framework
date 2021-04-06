using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UObject = UnityEngine.Object;

namespace Framework
{
    public class ResourcesLoad
    {
        private static ResourcesLoad inStance = null;
        public static ResourcesLoad Instance
        {
            get
            {
                if (inStance == null)
                {
                    inStance = new ResourcesLoad();
                }

                return inStance;
            }
        }

        private ResourcesLoad()
        {
            //this.Init();
        }

        private Dictionary<string, string> mAssetPathDic = new Dictionary<string, string>();
        /// <summary>
        /// 所有资源字典
        /// </summary>
        private Dictionary<string, ResourceInfo> mDicAaaet = new Dictionary<string, ResourceInfo>();

        /// <summary>
        /// CPU 个数
        /// </summary>
        private int mProcessorCount = 0;

        //初始化数据
        public void Init()
        {
            mProcessorCount = SystemInfo.processorCount > 0 && SystemInfo.processorCount <= 8 ? SystemInfo.processorCount : 1;
        }

        /// <summary>
        /// 正在加载的列表
        /// </summary>
        public List<RequestInfo> mInLoads = new List<RequestInfo>();

        /// <summary>
        /// 等待加载的列表
        /// </summary>
        public Queue<RequestInfo> mWaitting = new Queue<RequestInfo>();

        /// <summary>
        /// 资源加载堆栈
        /// </summary>
        public Stack<List<string>> mAssetStack = new Stack<List<string>>();

        #region 阻塞加载资源
        public void Load(string assetName, Action<UObject> action = null, Type type = null, bool isKeepInMemory = false, bool isAsync = true)
        {

        }
        #endregion

        #region 异步Res加载
        public void LoadAsync(string assetName, Action<UObject> action, Type type, bool isKeepInMemory)
        {
            if (mDicAaaet.ContainsKey(assetName))
            {
                if (action != null)
                {
                    action(mDicAaaet[assetName].resource);
                }

                return;
            }

            for (int i = 0; i < mInLoads.Count; i++)
            {
                if (mInLoads[i].resourceName == assetName)
                {

                    if (action != null)
                    {
                        mInLoads[i].AddAction(action);
                    }

                    return;
                }
            }

            foreach (RequestInfo info in mWaitting)
            {
                if (info.resourceName == assetName)
                {
                    if (action != null)
                    {
                        info.AddAction(action);
                    }

                    return;
                }
            }

            RequestInfo requestInfo = new RequestInfo();
            requestInfo.resourceName = assetName;
            if (action != null)
            {
                requestInfo.AddAction(action);
            }

            requestInfo.isKeepInMemory = isKeepInMemory;
            requestInfo.type = type == null ? typeof(UObject) : type;
            mWaitting.Enqueue(requestInfo);
        }
        #endregion

        #region 资源处理

        /// <summary>
        /// 从资源字典中取得一个资源
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <returns></returns>
        public ResourceInfo GetAsset(string assetName)
        {
            ResourceInfo info = null;
            mDicAaaet.TryGetValue(assetName, out info);
            return info;
        }

        /// <summary>
        /// 释放一个资源
        /// </summary>
        /// <param name="assetName">资源名称</param>
        public void ReleaseAsset(string assetName)
        {
            ResourceInfo info = null;
            mDicAaaet.TryGetValue(assetName, out info);

            if (info != null && !info.isKeepInMemory)
            {
                mDicAaaet.Remove(assetName);
            }
        }

        /// <summary>
        /// 修改资源是否常驻内存
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <param name="IsKeepInMemory">是否常驻内存</param>
        public void IsKeepInMemory(string assetName, bool IsKeepInMemory)
        {
            ResourceInfo info = null;
            mDicAaaet.TryGetValue(assetName, out info);

            if (info != null)
            {
                info.isKeepInMemory = IsKeepInMemory;
            }
        }
        #endregion

        #region 资源释放以及监听
        /// <summary>
        /// 把资源压入顶层栈内
        /// </summary>
        /// <param name="assetName">资源名称</param>
        public void AddAssetToName(string assetName)
        {
            if (mAssetStack.Count == 0)
            {
                mAssetStack.Push(new List<string>() { assetName });
            }

            List<string> list = mAssetStack.Peek();
            list.Add(assetName);
        }

        /// <summary>
        /// 开始让资源入栈
        /// </summary>
        public void PushAssetStack()
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, ResourceInfo> info in mDicAaaet)
            {
                info.Value.stackCount++;
                list.Add(info.Key);
            }

            mAssetStack.Push(list);
        }

        /// <summary>
        /// 释放栈内资源
        /// </summary>
        public void PopAssetStack()
        {
            if (mAssetStack.Count == 0) return;

            List<string> list = mAssetStack.Pop();
            List<string> removeList = new List<string>();
            ResourceInfo info = null;
            for (int i = 0; i < list.Count; i++)
            {
                if (mDicAaaet.TryGetValue(list[i], out info))
                {
                    info.stackCount--;
                    if (info.stackCount < 1 && !info.isKeepInMemory)
                    {
                        removeList.Add(list[i]);
                    }
                }
            }
            for (int i = 0; i < removeList.Count; i++)
            {
                if (mDicAaaet.ContainsKey(removeList[i]))
                    mDicAaaet.Remove(removeList[i]);
            }

            GC();
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void GC()
        {
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }

        #endregion

        public void Update()
        {
            if (mInLoads.Count > 0)
            {
                for (int i = mInLoads.Count - 1; i >= 0; i--)
                {
                    if (mInLoads[i].IsDone)
                    {
                        RequestInfo info = mInLoads[i];

                        ResourceInfo resInfo = new ResourceInfo();
                        resInfo.resource = info.Asset;
                        resInfo.isKeepInMemory = info.isKeepInMemory;
                        resInfo.stackCount = 1;
                        if (!mDicAaaet.ContainsKey(info.resourceName))
                        {
                            mDicAaaet.Add(info.resourceName, resInfo);
                        }

                        if (info.actionList != null && info.actionList.Count > 0)
                        {
                            for (int k = 0; k < info.actionList.Count; k++)
                            {
                                info.actionList[k](info.Asset);
                            }
                        }

                        AddAssetToName(info.resourceName);

                        mInLoads.RemoveAt(i);
                    }
                }
            }

            while (mInLoads.Count < mProcessorCount && mWaitting.Count > 0)
            {
                RequestInfo info = mWaitting.Dequeue();
                mInLoads.Add(info);
                info.LoadAsync();
            }
        }
    }
}