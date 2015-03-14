using UnityEngine;
using System.Collections;
using Assets.Scripts;

/// <summary>
/// notes, need to make a jump bheiavor class
/// </summary>
public class PlayerController : MonoBehaviour 
{
    private SoundManager sm;

    public int Health;
    public float MoveSpeed = 1.0f;  //MoveSpeed
    private float ySpeed = 1.0f;
    private float xIntensity = .1f;
    private float yIntensity = .2f;
    public float JumpVelocityCurrent;
    public float JumpVelocityDefault;

    public State state;
    public int jumpCounter;
    public PlayerStats playerStats;
    private Material playerMat;
    private Color[] colors = { Color.gray, Color.red };
    
    void Awake()
    {
        sm = SoundManager.instance;
    }

	// Use this for initialization
	void Start () {
        playerStats = GameObject.Find("PlayerStats").GetComponent<PlayerStats>();
        playerMat = GameObject.Find("Capsule Body").GetComponent<MeshRenderer>().material;
        JumpVelocityCurrent = JumpVelocityDefault;
        state = State.Moving;
        jumpCounter = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (IsPlayerDead())
        {
            PlayerDeath();
        }
		//DebugUpdate();
        MobileUpdate();
	}

    void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.collider.tag == "Terrain")
        {
            state = State.Moving;
            ResetJump();
        } 
        else if (hit.collider.tag == "Coin") {
            playerStats.Coins++;
        }
        else if (hit.collider.tag == "Obstacle")
        {
            sm.Play(Sound.PlayerHit);
            Health--;
            StartCoroutine(Flash (.1f, 0.1f));
        }
    }

    void MobileUpdate()
    {
        bool currentlyPressed = false;

        // hard coded, single vs double jump
        if (Input.touchCount != 0 && Input.GetTouch(0).tapCount == 1)
        {
            currentlyPressed = true;
        }
        else if (Input.touchCount != 0 && Input.GetTouch(0).tapCount == 2)
        {
            currentlyPressed = true;
            jumpCounter = 2;
        } else if (Input.touchCount != 0 && Input.GetTouch(0).tapCount == 3) 
        {
            jumpCounter = 0;
        }
        if (Input.touchCount != 0)
        {
            Debug.Log(Input.GetTouch(0).tapCount);
        }

        if (currentlyPressed == true && State.Falling != state)
        {
            state = State.Airborne;
        }

        if (State.Moving == state)
        {
            // position based movement
            transform.localPosition = new Vector2(xIntensity * MoveSpeed + transform.localPosition.x,
                                                  0 * ySpeed + transform.localPosition.y);
        }
        else if (State.Airborne == state)
        {
            ExecuteJump();
        }
        else if (State.Falling == state)
        {
            // position based movement
            transform.localPosition = new Vector2(xIntensity * MoveSpeed + transform.localPosition.x,
                                                    0 * ySpeed + transform.localPosition.y);
        }
        else
        {
            Debug.Log("ERROR ON STATE");
        }
    }

    void ExecuteJump()
    {
        if (JumpVelocityCurrent > 0)
        {
            if (jumpCounter == 1)
            {
                Jump();
            }
            else if (jumpCounter == 2)   // jumps more than the allowed magic number
            {
                DoubleJump();
            }
            else
            {
                Jump();
            }
        }
        else
        {
            state = State.Falling;
        }
    }

    private void Jump()
    {
        transform.localPosition = new Vector2(xIntensity * MoveSpeed + transform.localPosition.x,
            yIntensity * ySpeed + transform.localPosition.y);

        JumpVelocityCurrent -= Time.deltaTime;
    }

    private void DoubleJump()
    {
        JumpVelocityCurrent = JumpVelocityCurrent + JumpVelocityDefault; // or JumpVelocityCurrent+ JumpVelocityDefault
        Jump();
    }

    void ResetJump()
    {
        JumpVelocityCurrent = JumpVelocityDefault;
        jumpCounter = 1;
    }

    void PlayerDeath()
    {
        // explodes
        sm.PlayerDeath.Play();
        Destroy(this.gameObject);
    }

    private bool IsPlayerDead()
    {
        if (Health <= 0)
        {
            return true;
        }
        return false;
    }

    IEnumerator Flash(float time, float intervalTime)
    {
        float elapsedTime = 0f;
        int index = 0;
        while (elapsedTime < time)
        {
            playerMat.color = colors[index % 2];
            elapsedTime += Time.deltaTime;
            index++;
            yield return new WaitForSeconds(intervalTime);
        }
        playerMat.color = Color.white;
    }

    /// <summary>
    /// temp hack
    /// </summary>
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 18;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;

        if (Health <= 2)
        {
            style.normal.textColor = Color.red;
        }
        GUI.Label(new Rect(0, 0, 180, 120), "Health:  " + Health, style);
    }

    void DebugUpdate()
    {

        // not a true restart
        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevelName);
            return;
        }

        // check if screen is currently pressed
        var currentlyPressed = Input.GetKey("up");

        if (currentlyPressed == true && State.Falling != state)
        {
            state = State.Airborne;
        }

        if (State.Moving == state)
        {
            // position based movement
            transform.localPosition = new Vector2(xIntensity * MoveSpeed + transform.localPosition.x,
                                                  0 * ySpeed + transform.localPosition.y);
        }
        else if (State.Airborne == state)
        {
            ExecuteJump();
        }
        else if (State.Falling == state)
        {
            transform.localPosition = new Vector2(xIntensity * MoveSpeed + transform.localPosition.x,
                                      0 * ySpeed + transform.localPosition.y);

            Debug.Log("Here");
        }
        else
        {
            Debug.Log("ERROR ON STATE");
        }
    }

}
