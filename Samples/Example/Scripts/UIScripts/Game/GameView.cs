using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using lus.framework;

public class GameView : BaseView,IBaseView
{
    public Image image;
    public Material mat;
    void Start()
    {
        mat.mainTexture = null;
        mat.mainTexture = AssetManager.Instance.LoadAsset<Texture>("Shared/SelfResource/Textures/000" + ".jpg");

        var spriteAtals = AssetManager.Instance.LoadAsset<SpriteAtlas>("Shared/SelfResource/Atlas/Test");
        if (spriteAtals != null)
        {
            image.sprite = spriteAtals.GetSprite("2");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMessage(IMessage message)
    {

    }
}
