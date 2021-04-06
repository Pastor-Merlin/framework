
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Framework
{

    [Serializable]
    public class PoolInfo
    {
        public string poolName;
        public GameObject prefab;
        public int poolSize;
        public bool fixedSize;
    }

    public class GameObjectPool
    {
        private int maxSize;
        private int curCreateCount;
        private int poolSize;
        private string poolName;
        private Transform poolRoot;
        private GameObject poolObjectPrefab;
        private Stack<GameObject> availableObjStack = new Stack<GameObject>();

        public GameObjectPool(string poolName, GameObject poolObjectPrefab, int initCount, int maxSize, Transform pool)
        {
            this.poolName = poolName;
            this.poolSize = initCount;
            this.maxSize = maxSize;
            this.curCreateCount = 0;
            this.poolRoot = pool;
            this.poolObjectPrefab = poolObjectPrefab;

            //populate the pool
            for (int index = 0; index < initCount; index++)
            {
                AddObjectToPool(NewObjectInstance());
            }
        }

        //o(1)
        private void AddObjectToPool(GameObject go)
        {
            //add to pool
            go.transform.position = new Vector3(99999, 99999, 99999);
            go.SetActive(false);
            availableObjStack.Push(go);
            go.transform.SetParent(null);
        }

        private GameObject NewObjectInstance()
        {
            if (curCreateCount >= (maxSize -1)) //首个不在这里创建需要减掉
                return null;

            if (poolObjectPrefab != null)
            {
                GameObject go = GameObject.Instantiate(poolObjectPrefab) as GameObject;
                go.transform.position = new Vector3(99999, 99999, 99999);
                //go.SetActive(true);
                curCreateCount++;
                return go;
            }
            else
                Debug.LogError(poolName + "资源不存在！！！！！");

            return null;
        }

        public GameObject NextAvailableObject()
        {
            GameObject go = null;
            if (availableObjStack.Count > 0)
            {
                go = availableObjStack.Pop();
            }
            else
            {
                //Debug.LogWarning("No object available & cannot grow pool: " + poolName);
                go = NewObjectInstance();
            }
            //go.transform.SetParent(null);
            if(go)
                go.SetActive(true);
            return go;
        }

        //o(1)
        public void ReturnObjectToPool(string pool, GameObject po)
        {
            if (poolName.Equals(pool))
            {
                AddObjectToPool(po);
            }
            else
            {
                Debug.LogError(string.Format("Trying to add object to incorrect pool {0} ", poolName));
            }
        }

        public void Clear()
        {
            poolObjectPrefab = null;
            while (availableObjStack.Count > 0)
            {
                GameObject obj = availableObjStack.Pop();
                if (obj != null)
                    GameObject.Destroy(obj);
            }
            availableObjStack.Clear();
            curCreateCount = 0;
        }
    }
}
