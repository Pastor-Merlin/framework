using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
// using Toolkits;

namespace Framework
{
    public class GameManager : Manager
    {
        protected static bool initialize = false;
        private List<string> downloadFiles = new List<string>();

        /// <summary>
        /// 初始化游戏管理器
        /// </summary>
        void Awake()
        {
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        void Init()
        {
            StartCoroutine(GetPkgFilesTxt());

            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            //关闭帧率设置
            //Application.targetFrameRate = AppConst.GameFrameRate;
        }

        IEnumerator GetPkgFilesTxt()
        {
            string filePath = Util.GetStreamingAssetsPath() + AppConst.AssetResRoot + "/files.txt";
            if (Application.platform == RuntimePlatform.Android)
            {
                UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filePath);
                yield return www.SendWebRequest();
                Util.pkgFilesData = www.downloadHandler.text;
            }
            else
            {
                if (File.Exists(filePath))
                    Util.pkgFilesData = File.ReadAllText(filePath);
            }

            yield return new WaitForEndOfFrame();

            OnResourceInited();
        }

        /// <summary>
        /// 资源初始化结束
        /// </summary>
        public void OnResourceInited()
        {
            ResManager.Initialize(AppConst.AssetResRoot, delegate ()
            {
                Debug.Log("Initialize OK!!!");
                StartCoroutine(OnInitialize());
            });

            this.OnInitialize();
        }

        IEnumerator OnInitialize()
        {
            yield return new WaitForEndOfFrame();

            initialize = true;

            //开始连接网络
            NetWorkManager.ConnectedToServer("connect");
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        void OnDestroy()
        {
            Debug.Log("~GameManager was destroyed");
            if (NetWorkManager != null)
                NetWorkManager.Close();
        }
    }
}