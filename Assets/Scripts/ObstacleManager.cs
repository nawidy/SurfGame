using UnityEngine;
using System.Collections;

public class ObstacleManager : MonoBehaviour {

    GameObject player;
    float myTimer = 3.0f;
    int obstacleCount;
    const int OffSetX = 24;
    const int OffSetY = 4;
    const string Name = "obstacle_";

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("MainCamera");
        obstacleCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (myTimer > 0)
        {
            myTimer -= Time.deltaTime;
        }
        else
        {
            GameObject g = new GameObject(Name + (++obstacleCount));
            g.tag = "Obstacle";
            g.AddComponent<Red>();
            g.transform.localPosition = new Vector2(player.transform.localPosition.x + OffSetX, transform.localPosition.y + OffSetY);
            g.transform.parent = this.transform;
            myTimer = 3.0f;
        }
	}
}
