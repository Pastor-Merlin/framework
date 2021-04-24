using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using lus.framework;

public class CanvasManager : SingletonMono<CanvasManager>
{
    [HideInInspector]
    public Level[] levels;

    protected override void Awake()
    {
        base.Awake();

        var levelRoot = this.transform.Find("Level");
        if (levelRoot != null)
        {
            levels = levelRoot.GetComponentsInChildren<Level>(true);
        }
    }
    void Start()
    {
        StartCoroutine("LoadSceneAsync");
    }

    public static AsyncOperation async = null;
    IEnumerator LoadSceneAsync()
    {
        async = SceneManager.LoadSceneAsync("Game");//, LoadSceneMode.Additive);
        async.allowSceneActivation = false;
        while (async != null && !async.isDone)
        {
            if (async.progress >= 0.9f)
            {
                async.allowSceneActivation = true;
            }
            EventManager.Invoke(NotiConst.LOADING, async.progress);
            yield return new WaitForEndOfFrame();
        }
        EventManager.Invoke(NotiConst.LOADFINISHED, "");
    }
}
