using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultistScript : MonoBehaviour {

    public GameObject proj;
    public float shootPeriod = .3f;
    public int spawnHeight = 3;

    bool faceRight;
    float shootTimer;


	// Use this for initialization
	void Start () {
        shootTimer = shootPeriod;
    }

    // Update is called once per frame
    void Update () {
        if(shootTimer < 0)
        {
            GameObject shot = Instantiate(proj, transform.position + new Vector3(0, spawnHeight, 0), transform.rotation);
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), shot.GetComponent<BoxCollider2D>());
            shootTimer = shootPeriod;
        }
        else
        {
            shootTimer -= Time.deltaTime;
        }
	}
}
