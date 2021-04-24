using UnityEngine;
using UnityEngine.Events;

namespace lus.framework
{
    public class BaseProxy<T> where T : class, new()
    {
        private static T _instance;
        private static readonly Object _lock = new Object();

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

        public void Invoke(string eventName, object eventParams = null)
        {
            EventManager.Invoke(eventName, eventParams);
        }

        public void Invoke(object obj, string eventName, object eventParams = null)
        {
            EventManager.Invoke(obj, eventName, eventParams);
        }
    }
}