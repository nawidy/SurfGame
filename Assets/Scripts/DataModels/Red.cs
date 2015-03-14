using UnityEngine;
using System.Collections;

/// <summary>
/// obstacle, should be renamed
/// </summary>
public class Red : ObstacleController {

    const string ResourcePath = "red";

    void Start()
    {
        SpriteRenderer renderer = this.gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = Resources.Load<Sprite>(ResourcePath);
        this.gameObject.AddComponent<Rigidbody2D>();
        this.gameObject.AddComponent<BoxCollider2D>();
    }

    void Update()
    {
        base.Update();
    }
}
