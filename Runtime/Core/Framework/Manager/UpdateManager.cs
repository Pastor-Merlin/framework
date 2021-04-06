using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Framework
{
    public class UpdateManager : Manager
    {
        private List<Action> updateEventList = new List<Action>();

        public void AddEvent(Action action)
        {
            if (!updateEventList.Contains(action))
                updateEventList.Add(action);
        }

        public void RemoveEvent(Action action)
        {
            updateEventList.Remove(action);
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < updateEventList.Count; i++)
            {
                updateEventList[i]();
            }
        }
    }
}