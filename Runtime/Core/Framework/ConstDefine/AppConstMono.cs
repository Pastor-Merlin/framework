using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using System.IO;

public class AppConstMono:MonoBehaviour
{
    private static AppConstMono instance;
    public static AppConstMono Instance { get { return instance; } }

    public bool useDebug = true;
    public bool useAI = true;
    public bool useNetDebug = false;

#if UNITY_EDITOR
    public bool UseStreamingAssets = false;                     //Editor 是否使用SetramingAssets目录里的资源（lua 和 其他资源）
    public bool UpdateMode = false;                            //更新模式-默认关闭 
    public int  TimerInterval = 1;
    public bool isCommnad = false;
#endif

    public int GameFrameRate = 30;                              //游戏帧频

    public bool isOuterNet = true;
    public int LocalSocketPort = 0;                             //LobbySocket服务器端口
    public string LocalSocketAddress = string.Empty;            //LobbySocket服务器地址
    public int GameSocketPort = 0;                             //GameSocket服务器端口
    public string GameSocketAddress = string.Empty;            //GameSocket服务器地址

    void Awake()
    {
        instance = this;

        Debug.unityLogger.logEnabled = useDebug;
        AppConst.UseDebug = useDebug;
        AppConst.UseNetDebug = useNetDebug;

#if UNITY_EDITOR
        AppConst.UseStreamingAssets = UseStreamingAssets;
        AppConst.TimerInterval = TimerInterval;
#endif

        AppConst.GameFrameRate = GameFrameRate;
        AppConst.IsOuterNet = isOuterNet;
        AppConst.LocalSocketPort = LocalSocketPort;
        AppConst.LocalSocketAddress = LocalSocketAddress;
        AppConst.GameSocketPort = GameSocketPort;
        AppConst.GameSocketAddress = GameSocketAddress;
    }

}