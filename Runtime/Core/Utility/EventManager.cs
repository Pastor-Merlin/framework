using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace lus.framework
{
    public class CustomEvent : UnityEvent<string, object> { }

    public static class EventManager
    {
        private static Dictionary<string, CustomEvent> GlobalEventDictionary = new Dictionary<string, CustomEvent>();
        private static Dictionary<object, Dictionary<string, CustomEvent>> ObjectEventDictionary = new Dictionary<object, Dictionary<string, CustomEvent>>();

        public static void AddListener(object obj, string eventName, UnityAction<string, object> listener)
        {
            Dictionary<string, CustomEvent> dictionary;
            if (!ObjectEventDictionary.TryGetValue(obj, out dictionary))
            {
                dictionary = new Dictionary<string, CustomEvent>();
                ObjectEventDictionary.Add(obj, dictionary);
            }

            CustomEvent thisEvent = null;
            if (!dictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent = new CustomEvent();
                dictionary.Add(eventName, thisEvent);
            }

            thisEvent.AddListener(listener);
        }

        public static void AddListener(string eventName, UnityAction<string, object> listener)
        {
            CustomEvent thisEvent = null;
            if (!GlobalEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent = new CustomEvent();
                GlobalEventDictionary.Add(eventName, thisEvent);
            }

            thisEvent.AddListener(listener);
        }

        public static void RemoveListener(object obj, string eventName, UnityAction<string, object> listener)
        {
            Dictionary<string, CustomEvent> dictionary;
            if (ObjectEventDictionary.TryGetValue(obj, out dictionary))
            {
                CustomEvent thisEvent = null;
                if (dictionary.TryGetValue(eventName, out thisEvent))
                    thisEvent.RemoveListener(listener);
            }
        }

        public static void RemoveListener(string eventName, UnityAction<string, object> listener)
        {
            CustomEvent thisEvent = null;
            if (GlobalEventDictionary.TryGetValue(eventName, out thisEvent))
                thisEvent.RemoveListener(listener);
        }

        public static void RemoveAllListeners(object obj, string eventName)
        {
            Dictionary<string, CustomEvent> dictionary;
            if (ObjectEventDictionary.TryGetValue(obj, out dictionary))
            {
                CustomEvent thisEvent = null;
                if (dictionary.TryGetValue(eventName, out thisEvent))
                    thisEvent.RemoveAllListeners();
            }
        }

        public static void RemoveAllListeners(string eventName)
        {
            CustomEvent thisEvent = null;
            if (GlobalEventDictionary.TryGetValue(eventName, out thisEvent))
                thisEvent.RemoveAllListeners();
        }

        public static void Invoke(object obj, string eventName, object eventParams)
        {
            Dictionary<string, CustomEvent> dictionary;
            if (ObjectEventDictionary.TryGetValue(obj, out dictionary))
            {
                CustomEvent thisEvent = null;
                if (dictionary.TryGetValue(eventName, out thisEvent))
                    thisEvent.Invoke(eventName, eventParams);
            }
        }

        public static void Invoke(string eventName, object eventParams)
        {
            CustomEvent thisEvent = null;
            if (GlobalEventDictionary.TryGetValue(eventName, out thisEvent))
                thisEvent.Invoke(eventName, eventParams);
        }
    }
}