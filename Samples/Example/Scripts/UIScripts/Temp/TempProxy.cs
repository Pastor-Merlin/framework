using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lus.framework
{
    public class TempProxy : BaseProxy<TempProxy>,IBaseProxy
    {
        public void RegisterListen()
        {
            Debug.Log(typeof(TempProxy) + "---" + "RegisterListen");
        }
    }
}