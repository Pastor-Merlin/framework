using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Framework;

public class AppFacade : Facade
{
    private static AppFacade _instance;

    public AppFacade() : base()
    {
    }

    public static AppFacade Instance
    {
        get{
            if (_instance == null) {
                _instance = new AppFacade();
            }
            return _instance;
        }
    }

    override protected void InitFramework()
    {
        base.InitFramework();

        RegisterCommand(NotiConst.UI_INIT, typeof(UIControllerCommand));
        RegisterCommand(NotiConst.START_UP, typeof(StartUpCommand));
    }

    /// <summary>
    /// 启动框架
    /// </summary>
    public void StartUp()
    {

        SendMessageCommand(NotiConst.UI_INIT);
        RemoveMultiCommand(NotiConst.UI_INIT);

        SendMessageCommand(NotiConst.START_UP);
        RemoveMultiCommand(NotiConst.START_UP);
    }
}

