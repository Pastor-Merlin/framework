using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using lus.framework;

public class GameController : BaseController<GameController>,IBaseController
{
    public void Init() 
    {
         Debug.Log(typeof(GameController) + "---" + "Init");
    }

    public void InitProxy()
    {
        Debug.Log(typeof(GameController) + "---" + "InitProxy");
    }
}
