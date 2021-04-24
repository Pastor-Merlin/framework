using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lus.framework
{
    public class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = (T) this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
