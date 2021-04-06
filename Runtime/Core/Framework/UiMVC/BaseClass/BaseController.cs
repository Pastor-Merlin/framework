using System;
using System.Collections;
using System.Collections.Generic;
// using Toolkits;
using UnityEngine;
using UnityEngine.Events;

namespace Framework
{
    public class BaseController<T> where T : class, new()
    {
        private static T _instance;
        private static readonly System.Object _lock = new System.Object();

        private List<string> _listenerNames = new List<string>();

        public static T Instance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new T();
                }
            }

            return _instance;
        }

        public void AddListener(string eventName, UnityAction<string, object> listener)
        {
            // _listenerNames.Add(eventName);
            // EventManager.AddListener(eventName, listener);
        }

        public virtual void RemoveAllListener()
        {
            // for (int i = 0; i < _listenerNames.Count; i++)
            // {
            //     EventManager.RemoveAllListeners(_listenerNames[i]);
            // }

            //_listenerNames.Clear();
        }


        public GameObject CreateView(string path, string viewName, UI_LAYER layer)
        {
            return Factory.Instance().CreateView(path, viewName, Convert.ToInt32(layer).ToString());
        }

        // public void Invoke(string eventName, object eventParams = null)
        // {
        //     EventManager.Invoke(eventName, eventParams);
        // }

        // public void Invoke(object obj, string eventName, object eventParams = null)
        // {
        //     EventManager.Invoke(obj, eventName, eventParams);
        // }
    }
}
