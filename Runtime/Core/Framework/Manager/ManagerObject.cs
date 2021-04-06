using UnityEngine;
using System.Collections;

namespace Framework
{
    public class ManagerObject : MonoBehaviour
    {
        private ResourcesManager resMgrInstance = null;
        // Use this for initialization
        void Start()
        {
            resMgrInstance = ResourcesManager.Instance;
        }

        // Update is called once per frame
        void Update()
        {
            resMgrInstance.OnUpdate();
        }
    }
}
