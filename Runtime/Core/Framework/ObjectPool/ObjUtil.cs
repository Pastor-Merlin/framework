using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using System;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Framework
{
    public class ObjUtil
    {
        public static GameObject GetInstantiateObject(string name, GameObject go)
        {
            ObjectPoolManager poolManager = AppFacade.Instance.GetManager<ObjectPoolManager>(ManagerName.ObjectPool);
            GameObjectPool goPool = poolManager.GetPool(name);
            if (goPool == null)
                goPool = poolManager.CreatePool(name, 0, 999, go);
            GameObject effect = goPool.NextAvailableObject();
            effect.name = name;
            return effect;
        }

        public static IEnumerator RecycleGameObject(float delay, string name, GameObject toRecycle)
        {
            yield return new WaitForSeconds(delay);
            Recycle(name, toRecycle);
        }

        public static bool Recycle(string name, GameObject toRecycle)
        {
            if (toRecycle == null)
                return false;
            ObjectPoolManager poolManager = AppFacade.Instance.GetManager<ObjectPoolManager>(ManagerName.ObjectPool);
            GameObjectPool goPool = poolManager.GetPool(name);
            if (goPool == null)
            {
                //Debug.Log("Pool not exist:"+name);
                GameObject.Destroy(toRecycle);
                return false;
            }
            goPool.ReturnObjectToPool(name, toRecycle);
            return true;
        }

        public static T GetObjFromPool<T>() where T : class, new()
        {
            ObjectPoolManager pool = AppFacade.Instance.GetManager<ObjectPoolManager>(ManagerName.ObjectPool);
            ObjectPool<T> pPool = pool.GetPool<T>();
            if (pPool == null)
                pPool = pool.CreatePool<T>(null, null);
            T p = pPool.Get();
            return p;
        }

        public static void Recycle<T>(T t) where T : class, new()
        {
            ObjectPoolManager pool = AppFacade.Instance.GetManager<ObjectPoolManager>(ManagerName.ObjectPool);
            ObjectPool<T> pPool = pool.GetPool<T>();
            pPool.Release(t);
        }

        public static GameObject GetObjFromPool(string res, string model, int maxSize = 999)
        {
            ObjectPoolManager poolManager = AppFacade.Instance.GetManager<ObjectPoolManager>(ManagerName.ObjectPool);
            GameObjectPool goPool = poolManager.GetPool(model);
            GameObject obj = null;
            if (goPool == null)
            {
                obj = ResourcesManager.Instance.DownAsset(res, model, ".prefab", typeof(GameObject), true, true) as GameObject;
                if (obj != null)
                {
                    GameObject poolObj = GameObject.Instantiate(obj);
                    poolObj.name = model;
                    poolObj.SetActive(false);
                    goPool = poolManager.CreatePool(model, 0, maxSize, poolObj);
                }
            }
            else
                obj = goPool.NextAvailableObject();

            if (obj)
                obj.name = model;

            return obj;
        }

        public static void GetObjFromPoolSync(string res, string model, Action<UnityEngine.Object> action = null, int maxSize = 999)
        {
            ObjectPoolManager poolManager = AppFacade.Instance.GetManager<ObjectPoolManager>(ManagerName.ObjectPool);
            GameObjectPool goPool = poolManager.GetPool(model);
            GameObject obj = null;
            if (goPool == null)
            {
                string modelName = model;
                Action<UnityEngine.Object> callBack = action;
                int sizeMax = maxSize;
                ResourcesManager.Instance.DownAssetSync(res, model, ".prefab", (UnityEngine.Object o) =>
                {
                    obj = GameObject.Instantiate(o) as GameObject;
                    if (obj)
                        obj.name = model;

                    GameObject poolObj = GameObject.Instantiate(o) as GameObject;
                    poolObj.SetActive(false);
                    poolObj.name = model;
                    poolManager.CreatePool(modelName, 0, sizeMax, poolObj);

                    callBack?.Invoke(obj);
                }, typeof(GameObject));
            }
            else
            {
                obj = goPool.NextAvailableObject();

                if (obj)
                    obj.name = model;

                if (action != null)
                    action(obj);
            }

        }

        public static void ReturnObjToPool(string model, GameObject obj)
        {
            ObjectPoolManager poolManager = AppFacade.Instance.GetManager<ObjectPoolManager>(ManagerName.ObjectPool);
            GameObjectPool goPool = poolManager.GetPool(model);
            if (goPool == null)
                return;
            goPool.ReturnObjectToPool(model, obj);
        }

        public static GameObject CreateObject(string res, string model)
        {
            GameObject obj = ResourcesManager.Instance.DownAsset(res, model, ".prefab", typeof(GameObject), true, true) as GameObject;
            return obj;
        }

        public static void SetAtlasImg(string path, string atlasName, GameObject imgObj, string atlasImg)
        {
            if (imgObj == null)
            {
                Debug.LogError("【Helper:SetAtlasImg】：imgObj 为空");
                return;
            }

            SpriteAtlas atlas = ResourcesManager.Instance.LoadAsset(path, atlasName, ".spriteatlas", typeof(SpriteAtlas), false) as SpriteAtlas;
            imgObj.GetComponent<Image>().sprite = atlas.GetSprite(atlasImg);
        }
    }
}