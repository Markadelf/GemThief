using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingScript : MonoBehaviour {

    private Rigidbody2D rigid;

	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerControlScript.player != null)
        {
            Vector3 vec = (PlayerControlScript.player.transform.position - transform.position);
            rigid.velocity = vec * 3 / vec.magnitude;
        }
    }
}
