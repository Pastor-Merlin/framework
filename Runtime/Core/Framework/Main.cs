using UnityEngine;
using System.Collections;

namespace Framework
{
    public class Main : MonoBehaviour
    {
        private float deltaTime = 0f;
        private bool isUpdate = true;

        private void Update()
        {
            if (!isUpdate)
                return;

            deltaTime += Time.deltaTime;
            if (deltaTime >= 1)
            {
                isUpdate = false;
                AppFacade.Instance.StartUp();   //启动游戏
            }
        }

        //切后台调用
        private void OnApplicationPause(bool focus)
        {

        }
    }
}