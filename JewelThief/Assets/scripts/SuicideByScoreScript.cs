using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideByScoreScript : MonoBehaviour {

    public int scoreRec = 0;

	// Use this for initialization
	void Start () {
        if (LevelManagerScript.score < scoreRec)
            Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
