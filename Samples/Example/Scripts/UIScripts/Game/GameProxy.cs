using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using lus.framework;

public class GameProxy : BaseProxy<GameProxy>,IBaseProxy
{
    public void RegisterListen()
    {
        Debug.Log(typeof(GameController) + "---" + "RegisterListen");
    }
}
