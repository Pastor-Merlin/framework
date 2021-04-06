using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
//using Toolkits;
using UnityEngine.Events;

namespace Framework
{
    public class ScenesManager : Manager
    {
        private AsyncOperation async;
        private string sceneName = string.Empty;
        private Action<string> callback;
        private Action<float> progressFunc;
        private float targetValue;
        private float curValue;
        private float loadingSpeed = 2;
        private AssetBundle curScenesAB = null;

        private void Start()
        {

        }

        void OnEnable()
        {
            //Debug.Log("OnEnable called");
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            //Debug.Log("OnDisable");
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        // called second
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            string name = sceneName;
            sceneName = string.Empty;
            progressFunc = null;

            //EventManager.Invoke(NotiConst.SCENELOADED, name);
        }

        //检查场景是否在包里
        public bool CheckInPkg(string name)
        {
            bool canLoad = Application.CanStreamedLevelBeLoaded(name);
            return canLoad;
        }

        public bool CheckSceneIsHave(string name)
        {
            if (ResourcesManager.USE_AB)
            {
                string scenePath = AppConst.ScenesResRoot + "/" + name;
                int result = Util.CheckUseFileIsExit(scenePath, out string fileMd5Str);
                if (result != 0)
                {
                    return (result == 1 || result == 2);
                }
            }

            //进行包内场景文件存在检查
            if (CheckInPkg(name))
            {
                return true;
            }

            return false;
        }

        public void LoadSceneAsync(string sName, Action<float> progressfunc = null)
        {
            if (sName == sceneName || sceneName != string.Empty)
            {
                return;
            }

            //释放场景AB资源
            if (curScenesAB != null)
            {
                curScenesAB.Unload(true);
                curScenesAB = null;
            }

            //释放AssetBundle资源
            ResourcesManager.Instance.LoadedSceneDestory();

            sceneName = sName;
            progressFunc = progressfunc;

            targetValue = 0.0f;
            curValue = 0.0f;

            LoadScene();
        }

        public void LoadScene()
        {
            StartCoroutine(LoadAsync());

            //释放Asset对象
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }
        private IEnumerator LoadAsync()
        {
            yield return new WaitForEndOfFrame();

            async = SceneManager.LoadSceneAsync(sceneName);
            if (async != null)
                async.allowSceneActivation = false;

            while (async != null && !async.isDone)
            {
                yield return async;
            }
        }

        public void FixedUpdate()
        {
            if (async == null)
                return;

            //Debug.Log(async.progress);

            if (async.progress < 0.9f)
            {
                targetValue = async.progress;
                loadingSpeed = 1f;
            }
            else
            {
                targetValue = 1.0f;
                loadingSpeed = 0.5f;
            }


            if (curValue < targetValue)
                curValue += 0.1f * loadingSpeed;


            if (curValue >= 1.0f)
            {
                async.allowSceneActivation = true;
                async = null;
            }

            if (progressFunc != null)
                progressFunc(curValue);

        }
    }
}

