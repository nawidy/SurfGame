using UnityEngine;
using System.Collections;

public class BuoyancyEffect : MonoBehaviour {

    public float upwardBuoyancyMin;
    public float upwardBuoyancyMax;


	// Use this for initialization
	public void Start () {
        upwardBuoyancyMin = 6.0f;
        upwardBuoyancyMax = 10.0f;
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        other.attachedRigidbody.AddForce(Vector2.up * upwardBuoyancyMax * 2.0f);
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        other.attachedRigidbody.AddForce(Vector2.up * Random.Range(upwardBuoyancyMin, upwardBuoyancyMax));
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        other.attachedRigidbody.AddForce(Vector2.up * upwardBuoyancyMin * .5f);
    }
}
