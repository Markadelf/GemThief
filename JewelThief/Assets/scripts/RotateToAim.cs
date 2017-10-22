using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToAim : MonoBehaviour {

    Rigidbody2D rigid;
	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(rigid.velocity.y, rigid.velocity.x) * Mathf.Rad2Deg);
	}
}
