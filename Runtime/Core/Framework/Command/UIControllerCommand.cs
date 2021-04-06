using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
// using Toolkits;

namespace Framework
{
    public struct CreatViewInfo
    {
        public string path;
        public string prefab;
        public string layer;
    }

    public enum UI_LAYER
    {
        LOW = 1,//基础UI 1级
        MIDDLE = 2,//基础UI 2级
        HIGH = 3,//基础UI 3级
        TIP = 4,//对话框 UI
        GUIDE = 5,//更高级1
        SPECIAL = 6,//跟高级特殊
    }

    public class UIControllerCommand : ControllerCommand
    {
        private List<Type> UICtlList = new List<Type>();
        private Dictionary<string, GameObject> UIViewList = new Dictionary<string, GameObject>();

        public UIControllerCommand()
        {
            // //Loading 
            // UICtlList.Add(typeof(LoadingController));

            // //Fight
            // UICtlList.Add(typeof(FightCanvasController));

            // //Score
            // UICtlList.Add(typeof(ScoreCanvasController));

            // //login
            // UICtlList.Add(typeof(UILoginController));

            // //mapConfig
            // UICtlList.Add(typeof(UIMapConfigController));

            // //BattleController
            // UICtlList.Add(typeof(BattleController));

            // //lobby
            // UICtlList.Add(typeof(UILobbyController));

            // //creatTask
            // UICtlList.Add(typeof(UICreatTaskController));

            // //Tips
            // UICtlList.Add(typeof(UITipsController));


            // //task
            // UICtlList.Add(typeof(UITaskController));


            // //taskWaiting
            // UICtlList.Add(typeof(UITaskWaitingController));

            // //equip
            // UICtlList.Add(typeof(UIEquipController));

        }

        public override void Execute(IMessage message)
        {
            for (int i = 0; i < UICtlList.Count; i++)
            {
                object controller = Activator.CreateInstance(UICtlList[i]);
                if (controller is IBaseController)
                {
                    IBaseController baseController = (IBaseController)controller;
                    baseController.InitProxy();
                    baseController.Init();
                }
            }

            // EventManager.AddListener(NotiConst.VIEWSHOW, OnViewShow);
            // EventManager.AddListener(NotiConst.VIEWDEL, OnViewDel);
        }

        private void OnViewShow(string msgName, object obj)
        {
            if (obj == null)
                return;

            CreatViewInfo viewInfo = (CreatViewInfo)obj;

            GameObject view = null;
            if (UIViewList.TryGetValue(viewInfo.prefab, out view))
            {
                Debug.LogError("此UI已经创建了：" + viewInfo.prefab);
                return;
            }

            view = Factory.Instance().CreateView(viewInfo.path, viewInfo.prefab, viewInfo.layer);
            UIViewList.Add(viewInfo.prefab, view);
        }

        private void OnViewDel(string msgName, object obj)
        {
            string prefabName = obj as string;

            GameObject view = null;
            // if (UIViewList.TryGetValue(prefabName, out view))
            // {
            //     BaseView baseView = view.GetComponent<BaseView>();
            //     if (baseView != null)
            //         baseView.DestroyView();
            //     else
            //         GameObject.Destroy(view);
            // }
        }
    }
}