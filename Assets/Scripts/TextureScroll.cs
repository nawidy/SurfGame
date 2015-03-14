using UnityEngine;
using System.Collections;

public class TextureScroll : MonoBehaviour 
{
    private Renderer objRenderer;
    public  Vector2  scrollSpeed = Vector2.zero;

    public void Awake()
    {
        objRenderer = GetComponent<Renderer>();
    }

	public void Update () 
    {
        objRenderer.material.mainTextureOffset = scrollSpeed * Time.time;
	}
}
