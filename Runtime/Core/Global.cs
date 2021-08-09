using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PureMVC.Core
{
    public class Global : MonoBehaviour
    {
        public UILayerScript[] uILayers;

        public UILayerScript GetILayer(UILayer type) => uILayers.FirstOrDefault(x => x.UILayer == type);
    }
}