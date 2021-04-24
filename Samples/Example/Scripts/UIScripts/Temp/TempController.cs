using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lus.framework
{
    public class TempController : BaseController<TempController>,IBaseController
    {
        public void Init() 
        {
            Debug.Log(typeof(TempController) + "---" + "Init");
        }

        public void InitProxy()
        {
            Debug.Log(typeof(TempController) + "---" + "InitProxy");
        }
    }
}