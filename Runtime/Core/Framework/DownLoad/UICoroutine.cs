using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICoroutine : MonoBehaviour
{
    private static UICoroutine mInstance = null;

    public static UICoroutine uiCoroutine
    {
        get
        {
            if (mInstance == null)
            {
                GameObject go = new GameObject();
                if (go != null)
                {
                    go.name = "_UICoroutine";
                    go.AddComponent<UICoroutine>();
                }
                else
                {
                    Debug.LogError("Init UICoroutine faild. GameObjet can not be null.");
                }
            }
            return mInstance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        mInstance = this;
    }

    void OnDestroy()
    {
        mInstance = null;
    }
}
