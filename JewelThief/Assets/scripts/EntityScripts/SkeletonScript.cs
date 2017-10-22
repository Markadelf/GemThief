﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonScript : MonoBehaviour {

    public GameObject proj;
    public float volleyPeriod = 3f;
    public float shootPeriod = .3f;
    public float bulletSpeed = 5;
    public int volleyCount = 3;

    bool faceRight;
    float volleyTimer;
    float shootTimer;
    int loaded = 0;


	// Use this for initialization
	void Start () {
        volleyTimer = volleyPeriod;
        shootTimer = shootPeriod;
        loaded = volleyCount;
    }

    // Update is called once per frame
    void Update () {
		if(volleyTimer < 0)
        {
            if(shootTimer < 0)
            {
                Vector2 aim; 
                if(faceRight)
                    aim = new Vector2(bulletSpeed + loaded / 4f, 9);
                else
                    aim = new Vector2(-bulletSpeed - loaded / 4f, 9);
                GameObject shot = Instantiate(proj, transform.position, transform.rotation);
                shot.GetComponent<Rigidbody2D>().velocity = aim;
                loaded--;
                shootTimer = shootPeriod;
                if(loaded == 0)
                {
                    volleyTimer = volleyPeriod;
                    loaded = volleyCount;
                }
                Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), shot.GetComponent<BoxCollider2D>());
            }
            else
            {
                shootTimer -= Time.deltaTime;
            }
        }
        else
        {
            volleyTimer -= Time.deltaTime;
            if (PlayerContolScript.player != null)
                faceRight = PlayerContolScript.player.transform.position.x > transform.position.x;
        }
	}
}