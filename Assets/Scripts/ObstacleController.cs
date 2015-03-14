using UnityEngine;
using System.Collections;

public class ObstacleController : MonoBehaviour {

    private SoundManager sm;
    /// <summary>
    /// disable the object to be pool'ed
    /// </summary>
    protected void Awake()
    {
        sm = SoundManager.instance;
    }
	// Use this for initialization
	protected void Start () {

	}
	
	// Update is called once per frame
	protected void Update () {
        Destroy();
	}

    public void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.gameObject.tag == "Player")
        {
            sm.PlayerHit.Play();
            DestroyOnContact();
        }
    }

    /// <summary>
    /// destroy object immediately
    /// </summary>
    private void DestroyOnContact()
    {
        Destroy(this.gameObject);
    }

    /// <summary>
    /// destroy object when it is no longer visible on the world screen
    /// </summary>
    private void Destroy()
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.x < 0)
            Destroy(this.gameObject);
    }
}
