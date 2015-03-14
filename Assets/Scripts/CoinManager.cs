using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class CoinManager : MonoBehaviour {

    private GameObject player;
    private int coinCount;
    public float spawnTime;
    public float spawnTimeDefault;
    public int generateAmountMin;
    public int generateAmountMax;
    public float minHeight;
    public float maxHeight;
    public float spacingMin;
    public float spacingMax;
    private const string Name = "coin_";

	private void Start () {
        player = GameObject.FindWithTag("MainCamera");
        coinCount = 0;
        spawnTimeDefault = spawnTime;
	}
    
    private void Update () {
        if (spawnTime > 0)
        {
            spawnTime -= Time.deltaTime;
        }
        else
        {
            StartCoroutine(BuildCluster());
            spawnTime = 4.0f;
        }
	}

    private IEnumerator BuildCluster()
    {
        var rand = Random.Range(generateAmountMin, generateAmountMax);
        for (int i = 0; i < rand; i++)
        {
            CreateCoin();
            yield return new WaitForSeconds(Random.Range(spacingMin, spacingMax));
        }
    }

    private void CreateCoin()
    {
        GameObject g = new GameObject(Name + (++coinCount));
        g.tag = "Coin";
        g.AddComponent<Light>();
        g.transform.localPosition = new Vector2(player.transform.localPosition.x + 10, Random.Range(minHeight, maxHeight));
        g.transform.parent = this.transform;
    }
}
