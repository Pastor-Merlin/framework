﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;


public class Loom : MonoBehaviour
{
    //最大线程数
    public static int maxThreads = 8;
    private static int numThreads;

    //字段
    private static Loom _current;
    private static bool initialized;
    //属性
    public static Loom Current
    {
        get
        {
            Initialize();
            return _current;
        }
    }
    void Awake()
    {
        _current = this;
        initialized = true;
    }

    public static void Initialize()
    {
        if (!initialized)
        {

            if (!Application.isPlaying)
                return;

            initialized = true;
            var g = new GameObject("Loom");
            _current = g.AddComponent<Loom>();
#if !ARTIST_BUILD
            UnityEngine.Object.DontDestroyOnLoad(g);
#endif
        }
    }

    //NoDelayedQueueItem
    public struct NoDelayedQueueItem
    {
        public Action<object> action;
        public object param;
    }
    private List<NoDelayedQueueItem> _actions = new List<NoDelayedQueueItem>();
    private List<NoDelayedQueueItem> _currentActions = new List<NoDelayedQueueItem>();
    //DelayedQueueItem
    public struct DelayedQueueItem
    {
        public float time;
        public Action<object> action;
        public object param;
    }
    private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();
    private List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();


    //Unity主线程
    public static void QueueOnMainThread(Action<object> taction)
    {
        QueueOnMainThread(taction, null, 0f);
    }
    public static void QueueOnMainThread(Action<object> taction, object tparam)
    {
        QueueOnMainThread(taction, tparam, 0f);
    }
    public static void QueueOnMainThread(Action<object> taction, object tparam, float time)
    {
        if (time != 0)
        {
            lock (Current._delayed)
            {
                Current._delayed.Add(new DelayedQueueItem { time = Time.time + time, action = taction, param = tparam });
            }
        }
        else
        {
            lock (Current._actions)
            {
                Current._actions.Add(new NoDelayedQueueItem { action = taction, param = tparam });
            }
        }
    }

    //异步多线程
    public static Thread RunAsync(Action a)
    {
        Initialize();
        while (numThreads >= maxThreads)
        {
            Thread.Sleep(100);
        }
        Interlocked.Increment(ref numThreads);
        ThreadPool.QueueUserWorkItem(RunAction, a);
        return null;
    }

    private static void RunAction(object action)
    {
        try
        {
            ((Action)action)();
        }
        catch
        {
        }
        finally
        {
            Interlocked.Decrement(ref numThreads);
        }

    }

    void OnDisable()
    {
        if (_current == this)
        {
            _current = null;
        }
    }

    void Update()
    {
        if (_actions.Count > 0)
        {
            lock (_actions)
            {
                _currentActions.Clear();
                _currentActions.AddRange(_actions);
                _actions.Clear();
            }
            for (int i = 0; i < _currentActions.Count; i++)
            {
                _currentActions[i].action(_currentActions[i].param);
            }
        }

        if (_delayed.Count > 0)
        {
            lock (_delayed)
            {
                _currentDelayed.Clear();
                _currentDelayed.AddRange(_delayed.Where(d => d.time <= Time.time));
                for (int i = 0; i < _currentDelayed.Count; i++)
                {
                    _delayed.Remove(_currentDelayed[i]);
                }
            }

            for (int i = 0; i < _currentDelayed.Count; i++)
            {
                _currentDelayed[i].action(_currentDelayed[i].param);
            }
        }
    }

    public void DispathcMsg()
    {


    }
}