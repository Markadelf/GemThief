using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinByTouch : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject == PlayerContolScript.player)
        {
            LevelManagerScript.Freeze();
        }

    }
}
