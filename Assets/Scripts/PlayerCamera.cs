using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

  GameObject player;

	// Use this for initialization
	void Start () {
    player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
	  float x = Mathf.Lerp(transform.localPosition.x, player.transform.localPosition.x, Time.time);
    	transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
	}
}
