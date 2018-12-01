using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    private SpriteRenderer sprite;
    public Sprite bg1;
    public Sprite bg2;
    public Sprite bg3;

    // Use this for initialization
    void Start () {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        InvokeRepeating("animationSea1", 0.0f, 6.0f);
        InvokeRepeating("animationSea2", 2.0f, 6.0f);
        InvokeRepeating("animationSea3", 4.0f, 6.0f);
    }


    void animationSea1()
    {
        sprite.sprite = bg1;
    }

    void animationSea2()
    {
        sprite.sprite = bg2;
    }

    void animationSea3()
    {
        sprite.sprite = bg3;
    }
}
