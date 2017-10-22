using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour {

    public bool persist = true;
    public bool active = true;
    public int teamID = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (active)
        {
            if (!persist)
            {
                Destroy(gameObject);
            }
        }

    }


    void OnTriggerStay2D(Collider2D col)
    {
        if (active)
        {
            DamagableScript other = col.gameObject.GetComponent<DamagableScript>();
            if (other != null && teamID != other.teamID)
            {
                other.Damage();
                if (!persist)
                {
                    Destroy(gameObject);
                }
            }
        }

    }

}
