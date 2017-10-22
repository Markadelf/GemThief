using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(PlayerControlScript.player != null)
            transform.position = PlayerControlScript.player.transform.position + new Vector3(0,0,-10);
	}
}
