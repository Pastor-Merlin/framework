using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Framework
{
    public class AppConst
    {
        //Editor 是否使用SetramingAssets目录里的资源
        private static bool useStreamingAssets;
        public static bool UseStreamingAssets
        {
            set { useStreamingAssets = value; }
            get
            {
                if (Application.isMobilePlatform && !Application.isEditor)
                    return true;

                return useStreamingAssets;
            }
        }

        public static int TimerInterval = 1;
        public static int GameFrameRate = 30;                               //游戏帧频

        public const string AssetDir        = "StreamingAssets";            //素材目录 
        public const string AssetResRoot    = "AssetsResources";
        public const string ScenesResRoot   = "Scenes";
        public const string SelfResources   = "SelfResources";             //程序维护资源目录
        public const string AppName         = "Framework";
        public static string DataCfgPath = Application.dataPath + "/" + SelfResources + "/DataCfg/";           //数据配置文件

        public static bool UseDebug = false;                             //Debug模式
        public static bool UseNetDebug = false;                          //网络调试
        public static bool IsOuterNet = true;                            //是否是外网
        public static int LocalSocketPort = 0;                           //LobbySocket服务器端口
        public static string LocalSocketAddress = string.Empty;          //LobbySocket服务器地址
        public static int GameSocketPort = 0;                            //GameSocket服务器端口
        public static string GameSocketAddress = string.Empty;           //GameSocket服务器地址

        public static string StreamingAssetsPath
        {
            get
            {
#if UNITY_EDITOR || UNITY_STANDALONE
                return Application.dataPath + "/StreamingAssets/";
#elif UNITY_ANDROID
            return Application.dataPath + "!/assets/";
#elif UNITY_IOS || UNITY_IPHONE
            return Application.dataPath + "/Raw/";
            //return Application.streamingAssetsPath + "/";
#else
            return Application.dataPath + "/StreamingAssets/";
            //return Application.streamingAssetsPath + "/";
#endif
            }
        }

        //设备类型
        public static string DeviceType
        {
            get
            {
#if UNITY_ANDROID
                return "Android";
#elif UNITY_IOS || UNITY_IPHONE
            return "IOS";
#else
            return "PC";
#endif
            }
        }

        //设备型号
        public static string GetDeviceModel()
        {
            return SystemInfo.deviceModel;
        }
    }
}