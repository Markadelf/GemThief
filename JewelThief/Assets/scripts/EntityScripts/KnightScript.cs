using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightScript : MonoBehaviour {

    private Rigidbody2D rigid;
    private BoxCollider2D collide;
    public bool faceRight = false;

    // Use this for initialization
    void Start () {
        rigid = GetComponent<Rigidbody2D>();
        collide = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {

        if (faceRight)
        {
            rigid.velocity = new Vector2(3, rigid.velocity.y);
        }
        else
        {
            rigid.velocity = new Vector2(-3, rigid.velocity.y);
        }
    }


    void OnCollisionStay2D(Collision2D col)
    {
        ContactPoint2D[] points = col.contacts;
        if (points.Length == 0 || col.gameObject == PlayerContolScript.player)
            return;
        float highest = points[0].point.y;
        for (int i = 1; i < points.Length; i++)
        {
            if (points[i].point.y > highest)
            {
                highest = points[i].point.y;
            }
        }
        float leftmost = points[0].point.x;
            for (int i = 1; i < points.Length; i++)
            {
                if (points[i].point.x < leftmost)
                {
                    leftmost = points[i].point.x;
                }
            }
        float rightmost = points[0].point.x;
        for (int i = 1; i < points.Length; i++)
        {
            if (points[i].point.x > rightmost)
            {
                rightmost = points[i].point.x;
            }
        }
        //If we are going right and it is to our right, turn around
        //If we are going left and it is to our left, turn around
        if (faceRight && (leftmost > collide.bounds.max.x || (rightmost < transform.position.x && !(highest - .4f > collide.bounds.min.y))))
        {
            faceRight = false;
        }
        else if (!faceRight && (rightmost < collide.bounds.min.x || (leftmost > transform.position.x && !(highest - .4f > collide.bounds.min.y))))
        {
            faceRight = true;
        }

    }



}
