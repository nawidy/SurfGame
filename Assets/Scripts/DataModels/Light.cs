using UnityEngine;
using System.Collections;

/// <summary>
/// fake coin class
/// </summary>
public class Light : CoinController {

    public string ResourcePath = "light";

    private void Start()
    {
        SpriteRenderer renderer = this.gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = Resources.Load<Sprite>(ResourcePath);
        //this.gameObject.AddComponent<Rigidbody2D>();
        this.gameObject.AddComponent<BoxCollider2D>();
    }

    private void Update()
    {
        base.Update();
    }
}
