using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace lus.framework
{
    public class BaseView : MonoBehaviour, IBaseView
    {
        private List<string> _listenerNames = new List<string>();
        private List<string> _listenerObjNames = new List<string>();

        public virtual void OnMessage(IMessage message)
        {
        }

        public void AddListener(string eventName, UnityAction<string, object> listener)
        {
            _listenerNames.Add(eventName);
            EventManager.AddListener(eventName, listener);
        }

        public void AddObjListener(string eventName, UnityAction<string, object> listener)
        {
            _listenerObjNames.Add(eventName);
            EventManager.AddListener(gameObject, eventName, listener);
        }

        public void Invoke(string eventName, object eventParams = null)
        {
            EventManager.Invoke(eventName, eventParams);
        }

        public void Invoke(object obj, string eventName, object eventParams = null)
        {
            EventManager.Invoke(obj, eventName, eventParams);
        }

        public virtual void DestroyView()
        {
            for (int i = 0; i < _listenerNames.Count; i++)
            {
                EventManager.RemoveAllListeners(_listenerNames[i]);
            }

            for (int i = 0; i < _listenerObjNames.Count; i++)
            {
                EventManager.RemoveAllListeners(gameObject, _listenerNames[i]);
            }
            _listenerNames.Clear();
            _listenerObjNames.Clear();

            Destroy(this.gameObject);
        }
    }
}