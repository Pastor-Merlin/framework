using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UObject = UnityEngine.Object;

/// <summary>
/// 资源信息
/// </summary>
namespace Framework
{
    public class ResourceInfo
    {
        /// <summary>
        /// 资源
        /// </summary>
        public UObject resource;

        /// <summary>
        /// 是否常驻内存
        /// </summary>
        public bool isKeepInMemory;

        /// <summary>
        /// 资源堆栈数量
        /// </summary>
        public int stackCount = 0;
    }

    /// <summary>
    /// 资源加载信息
    /// </summary>
    public class RequestInfo
    {
        /// <summary>
        /// 资源反馈信息
        /// </summary>
        public ResourceRequest request;

        /// <summary>
        /// 是否常驻内存
        /// </summary>
        public bool isKeepInMemory;

        /// <summary>
        /// 资源名称
        /// </summary>
        public string resourceName;
        public string resourceFullName
        {
            get
            {
                return resourceName;
            }
        }

        /// <summary>
        /// 资源类型
        /// </summary>
        public Type type;

        /// <summary>
        /// 资源是否加载完成
        /// </summary>
        public bool IsDone
        {
            get
            {
                //AssetDatabase.LoadAssetAtPath加载的直接对资源进行检测
                if (asset)
                    return true;

                return (request != null && request.isDone);
            }
        }

        /// <summary>
        /// 加载到的资源
        /// </summary>
        private UObject asset;
        public UObject Asset
        {
            get
            {
                //AssetDatabase.LoadAssetAtPath加载的直接对资源进行检测
                if (asset)
                    return asset;

                return request != null ? request.asset : null;
            }
            set
            {
                asset = value;
            }
        }

        /// <summary>
        /// 添加Action等到加载完毕后回调
        /// </summary>
        public List<Action<UObject>> actionList;
        public void AddAction(Action<UObject> action)
        {
            if (actionList == null)
            {
                actionList = new List<Action<UObject>> { action };
            }
            else
            {
                if (!actionList.Contains(action))
                {
                    actionList.Add(action);
                }
            }
        }

        public void LoadAsync()
        {
            if (resourceFullName.IndexOf("Resources/", 0) == -1)
            {
#if UNITY_EDITOR
                if (type != null)
                {
                    Asset = UnityEditor.AssetDatabase.LoadAssetAtPath(resourceFullName, type);
                }
                else
                {
                    Asset = UnityEditor.AssetDatabase.LoadMainAssetAtPath(resourceFullName);
                }
#else
            Debug.LogError("[ResourceInfo.LoadAsync] 非Editor环境请使用AB加载资源");
#endif
            }
            else
            {
                request = type == null ? Resources.LoadAsync(resourceFullName) : Resources.LoadAsync(resourceFullName, type);
            }
        }
    }
}