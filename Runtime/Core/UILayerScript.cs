﻿using UnityEngine;

namespace PureMVC.Core
{
    public enum UILayer
    {
        Hidden = 0,
        Low = 10,
        Mid = 20,
        High = 30,
    }

    public class UILayerScript : MonoBehaviour
    {
        public UILayer UILayer;
    }
}