﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class BaseData<T> where T : class, new()
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
    }
}
