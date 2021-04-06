using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Single<Factory>
{
    public GameObject CreateView(string path, string viewName, string layer, object par = null)
    {
        GameObject view = ObjUtil.CreateObject("SelfResources/Prefabs/UI/" + path, viewName);
        view.transform.parent = GameObject.Find("UIMainCanvas/" + layer).transform;
        view.transform.localPosition = Vector3.zero;
        view.transform.localScale = Vector3.one;

        RectTransform rect = view.transform.GetComponent<RectTransform>();
        rect.offsetMax = Vector2.zero;
        rect.offsetMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.anchorMin = Vector2.zero;

        return view;
    }
}
