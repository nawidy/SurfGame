using UnityEngine;
using System.Collections;


/// <summary>
/// the score controller that keeps track of inventory gained, and output positioning ot the screen
/// </summary>
public class ScoreController : MonoBehaviour {

    private PlayerStats playerStats;
    private SoundManager sm;

    private void Awake()
    {
        sm = SoundManager.instance;
    }

	private void Start () {
	     playerStats = GameObject.Find("PlayerStats").GetComponent<PlayerStats>();
	}
	
	private void Update () {
        Hit100();
	}

    private void Hit100()
    {
        if (playerStats.Coins != 0 && playerStats.Coins % 50 == 0)
        {
            sm.CoinFive.Play();
        }
    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 40;
        GUI.skin.label.fontStyle = FontStyle.Bold;
        GUI.Label(new Rect((Screen.width/2)-40,0,200,120), "Score:  " + playerStats.Coins);
    }    
}
