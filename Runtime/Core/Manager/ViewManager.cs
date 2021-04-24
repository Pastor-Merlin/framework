using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace lus.framework
{
    public struct CreatViewInfo
    {
        public string path;
        public string prefab;
        public string layer;
    }

    public enum UI_LAYER
    {
        LOW = 0,//基础UI 0级
        MIDDLE = 1,//基础UI 1级
        HIGH = 2//基础UI 2级
    }

    public class ViewManager : SingletonMono<ViewManager>
    {
        private List<Type> UICtlList = new List<Type>();
        private Dictionary<string, GameObject> UIViewList = new Dictionary<string, GameObject>();

        protected override void Awake()
        {
            EventManager.AddListener(NotiConst.VIEWSHOW, OnViewShow);
            EventManager.AddListener(NotiConst.VIEWDEL, OnViewDel);
        }

        protected void OnDestroy()
        {
            EventManager.RemoveListener(NotiConst.VIEWSHOW, OnViewShow);
            EventManager.RemoveListener(NotiConst.VIEWDEL, OnViewDel);
        }

        public void Append<T>()
        {
            UICtlList.Add(typeof(T));
        }
        
        public void Execute()
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
        }

        public void OnViewShow(string msgName, object obj)
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

            view = CreateView(viewInfo.path, viewInfo.prefab, viewInfo.layer);
            UIViewList.Add(viewInfo.prefab, view);
        }

        public void OnViewDel(string msgName, object obj)
        {
            string prefabName = obj as string;

            GameObject view = null;
            if (UIViewList.TryGetValue(prefabName, out view))
            {
                BaseView baseView = view.GetComponent<BaseView>();
                if (baseView != null)
                    baseView.DestroyView();
                else
                    GameObject.Destroy(view);

                UIViewList.Remove(prefabName);
            }
        }

        private GameObject CreateView(string path, string prefab, string layer)
        {
            var prefabObj = AssetManager.Instance.LoadAsset<GameObject>(path + prefab);
            if (prefabObj != null)
                return Instantiate(prefabObj, CanvasManager.Instance.levels[int.Parse(layer)].transform);
            else
                return null;
        }

    }
}