using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using lus.framework;

public class AppMono : SingletonMono<AppMono>
{
    public bool bUseAB = false;

    protected AssetManager assetManager;
    protected ViewManager viewManager;
    protected void Awake()
    {
        viewManager = this.GetComponent<ViewManager>();
        viewManager.Append<GameController>();
        viewManager.Append<TempController>();
        viewManager.Execute();

        assetManager = this.GetComponent<AssetManager>();
        assetManager.bUseAB = bUseAB;
        assetManager.LoadAssetAction = ()=>
        {
            Debug.Log("load preresource finished!");
        };
    }
    
    protected void OnEnable()
    {
        EventManager.AddListener(NotiConst.LOADFINISHED, OnLoadFinished);
    }

    protected void OnDisable()
    {
        EventManager.RemoveListener(NotiConst.LOADFINISHED, OnLoadFinished);
    }

    public void OnLoadFinished(string msgName, object obj)
    {
        var prefab = AssetManager.Instance.LoadAsset<GameObject>("Shared/SelfResource/Prefabs/Player/Player" + ".prefab");
        if (prefab != null)
            Instantiate(prefab);
        EventManager.Invoke(NotiConst.VIEWSHOW, new CreatViewInfo()
        {
            path = "Shared/SelfResource/Prefabs/UI/",
            prefab = "GameView" + ".prefab",
            layer = "0"
        });
    }
}
