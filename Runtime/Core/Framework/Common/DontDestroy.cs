using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    [DisallowMultipleComponent]
    public class DontDestroy : MonoBehaviour
    {
        private void Awake()
        {

        }

        // Start is called before the first frame update
        void Start()
        {
            DontDestroyOnLoad(this);
        }
    }
}
