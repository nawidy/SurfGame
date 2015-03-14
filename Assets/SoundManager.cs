using UnityEngine;
using System.Collections;
using Assets.Scripts;

/// <summary>
/// sound manager, preloads and makes it unity set friendly
/// </summary>
public class SoundManager : MonoBehaviour {

    private static SoundManager _instance;
    public static SoundManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SoundManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    // public declarations for unity
    public AudioSource Coin;
    public AudioSource PlayerHit;
    public AudioSource PlayerDeath;
    public AudioSource CoinFive;

    /// <summary>
    /// starts the singleton pattern
    /// </summary>
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != _instance)
                Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// deprecated
    /// helper function that executes a play sound
    /// </summary>
    /// <param name="type"></param>
    public void Play(Sound type)
    {
       switch (type)
       {
           case Sound.Coin:
               Coin.Play();
               break;
           case Sound.PlayerHit:
               PlayerHit.Play();
               break;
           case Sound.PlayerDeath:
               PlayerDeath.Play();
               break;
           default:
               Debug.Log("Sound Play ERROR");
               break;
       }
   }
}
