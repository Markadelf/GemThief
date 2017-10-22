using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableScript : MonoBehaviour {

    public float immuneTimerPeriod = 0;
    private float internalTimer = 0;
    public int health = 1;
    public int teamID = 0;
    public bool endGame = false;
    public bool ignore = false;
    private SpriteRenderer rend;

    // Use this for initialization
    void Start () {
        rend = GetComponent<SpriteRenderer>();		
	}
	
	// Update is called once per frame
	void Update () {
		if(internalTimer > 0)
        {
            internalTimer -= Time.deltaTime;
            rend.color = new Color(1, internalTimer * 6 % 1, internalTimer * 6 % 1);
        }
        else
        {
            rend.color = Color.white;
        }
        if (health <= 0)
        {
            if (endGame)
            {
                LevelManagerScript.Freeze();
            }
            Destroy(gameObject);
        }

    }

    //A funtion that lets things damage things.
    public void Damage()
    {
        if (internalTimer <= 0 && !ignore)
        {
            health--;
            internalTimer = immuneTimerPeriod;
        }
    }


}
