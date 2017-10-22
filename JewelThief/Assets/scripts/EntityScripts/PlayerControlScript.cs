using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState
{
    //Ground
    Idle,
    Running,
    Melee,

    //Air
    AirSlide,
    Airborne,

    WallSlide
    
    
}

public class PlayerControlScript : MonoBehaviour {

    //static ref
    public static GameObject player;
    public GameObject melee;
    public Animation[] sprites;

    //Orientation
    private Rigidbody2D rigid;
    private BoxCollider2D collide;
    private DamagableScript dmg;
    private Animator anim;
    public PlayerState state;
    private float lastY;
    private float currentManueverTime;
    public bool faceRight;
    public bool canSlide = true;
    PlayerState old;

    //Stats
    public float momentum;

    // Use this for initialization
	void Start () {
        currentManueverTime = 0;
        dmg = GetComponent<DamagableScript>();
        rigid = GetComponent<Rigidbody2D>();
        collide = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        faceRight = true;
        state = PlayerState.Idle;
        momentum = 0;
        player = gameObject;
        Time.maximumDeltaTime = 0.03f;
    }
	
	// Update is called once per frame
	void Update () {

        if(state == PlayerState.WallSlide && Mathf.Abs(rigid.velocity.x) > .3f)
        {
            state = PlayerState.Airborne;
        }

        if(state == PlayerState.Melee)
        {
            currentManueverTime += Time.deltaTime;
            if(currentManueverTime > .3f)
            {
                currentManueverTime = 0;
                state = PlayerState.Idle;
            }
        }
        if (state == PlayerState.AirSlide)
        {
            currentManueverTime += Time.deltaTime;
            if (currentManueverTime > .3f)
            {
                currentManueverTime = 0;
                state = PlayerState.Airborne;
                canSlide = false;
                dmg.ignore = false;
            }
            else
            {
                dmg.ignore = true;
                rigid.velocity = new Vector2(-30, 0);
                if (faceRight)
                {
                    rigid.velocity *= -1;
                }
            }
        }

        #region Controls
        if (state == PlayerState.Idle || state == PlayerState.Running)
        {
            
            if (Input.GetKey(KeyCode.X))
            {
                state = PlayerState.Melee;
                GameObject obj = Instantiate(melee, transform.position, transform.rotation);
                if(faceRight)
                    obj.transform.localPosition += new Vector3(1f, 0, 0);
                else
                    obj.transform.localPosition += new Vector3(-1f, 0, 0);
            }
        }

        
        //Jump logic
        if (state == PlayerState.Idle || state == PlayerState.Running || state == PlayerState.WallSlide)
        {
            //Just to save on repeated checks, Im throwing this fix here.
            canSlide = true;
            if (Input.GetKey(KeyCode.Space))
            {
                rigid.velocity = (new Vector2(rigid.velocity.x, 10));
                if (state == PlayerState.WallSlide)
                {
                    Vector2 push = new Vector2(1000, 0);
                    if (faceRight)
                        push = -push;
                    rigid.AddForce(push);
                }
                state = PlayerState.Airborne;
            }
        }



        //Move left or right if relevant
        if (state == PlayerState.Running || state == PlayerState.Idle || state == PlayerState.Airborne || state == PlayerState.WallSlide)
        {

            if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                
                    
                rigid.AddForce(new Vector2((40), 0));
                if (state != PlayerState.WallSlide || !faceRight)
                {
                    if (state == PlayerState.WallSlide)
                        state = PlayerState.Airborne;
                    if (state == PlayerState.Idle)
                    {
                        state = PlayerState.Running;
                    }
                    if (rigid.velocity.x < 0 && state != PlayerState.Airborne)
                        rigid.velocity = new Vector2(0, rigid.velocity.y);
                    if (state == PlayerState.Airborne && canSlide)
                    {
                        if (Input.GetKeyDown(KeyCode.LeftShift))
                        {
                            state = PlayerState.AirSlide;
                            faceRight = true;
                        }
                    }
                }
            }
            else if (!Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
            {
                    rigid.AddForce(new Vector2(-(40), 0));
                if (state != PlayerState.WallSlide || faceRight)
                {
                    if (state == PlayerState.WallSlide)
                        state = PlayerState.Airborne;
                    if (state == PlayerState.Idle)
                    {
                        state = PlayerState.Running;
                    }
                    if (rigid.velocity.x > 0 && state != PlayerState.Airborne)
                        rigid.velocity = new Vector2(0, rigid.velocity.y);
                    if (state == PlayerState.Airborne && canSlide)
                    {
                        if (Input.GetKeyDown(KeyCode.LeftShift))
                        {
                            state = PlayerState.AirSlide;
                            faceRight = false;
                        }
                    }
                }
            }
            //If no input
            else
            {
                if (state == PlayerState.Running)
                {
                    state = PlayerState.Idle;
                }
                if(state == PlayerState.WallSlide)
                {
                    state = PlayerState.Airborne;
                }
                if (state != PlayerState.Airborne)
                {
                    rigid.velocity = new Vector2(rigid.velocity.x * .01f, rigid.velocity.y);
                }
                else
                {
                    rigid.velocity = new Vector2(rigid.velocity.x * .9f, rigid.velocity.y);
                }
            }
        }

        

        if (Mathf.Abs(rigid.velocity.y) > .10f && state != PlayerState.WallSlide && state != PlayerState.Melee && state != PlayerState.AirSlide)
        {
            state = PlayerState.Airborne;
        }


        #endregion


        //Reface the player
        if (rigid.velocity.x != 0 && state != PlayerState.AirSlide)
        {
            faceRight = rigid.velocity.x > 0;
        }
        #region Handle Momentum
        rigid.drag = 0;
        switch (state)
        {
            case PlayerState.Idle:
                if(momentum > 0)
                {
                    momentum -= 2.5f * Time.deltaTime;
                }
                else
                {
                    momentum = 0;
                }
                break;
            case PlayerState.Running:
                if (momentum < 10)
                {
                    momentum += 2.5f * Time.deltaTime;
                }
                break;
            case PlayerState.Melee:
                if (momentum > 0)
                {
                    momentum -= 7f * Time.deltaTime;
                }
                else
                {
                    momentum = 0;
                }
                rigid.drag = 3;

                break;
            case PlayerState.AirSlide:
                break;
            case PlayerState.WallSlide:
                if (momentum > 0)
                {
                    momentum -= 3f * Time.deltaTime;
                }
                rigid.drag = 3;
                break;
            default:
                break;
        }
        #endregion

        if (state != PlayerState.AirSlide)
        {

            float bound = momentum + 9;
            //Double movement allowance in air
            if (state == PlayerState.Airborne)
            bound *= 1.1f;
            if (faceRight && rigid.velocity.x > bound)
            {
                rigid.velocity = new Vector2(bound, rigid.velocity.y);
            }
            else if (rigid.velocity.x < -(bound))
            {
                rigid.velocity = new Vector2(-(bound), rigid.velocity.y);
            }
            if (rigid.velocity.y < -20)
                rigid.velocity = new Vector2(rigid.velocity.x, -20);
        }
        if (state != old || rigid.velocity.y * lastY < 0)
        {
            UpdateAnimator();
        }
        old = state;
        if (!(faceRight && state == PlayerState.WallSlide) && (faceRight || state == PlayerState.WallSlide))
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        lastY = rigid.velocity.y;
    }

    /// <summary>
    /// Hit the ground or a wall
    /// </summary>
    void OnCollisionStay2D(Collision2D col)
    {
        if (state == PlayerState.Airborne || state == PlayerState.WallSlide)
        {
            ContactPoint2D[] points = col.contacts;
            if (points.Length == 0)
                return;
            ContactPoint2D highest = points[0];
            for (int i = 1; i < points.Length; i++)
            {
                if (points[i].point.y > highest.point.y)
                {
                    highest = points[i];
                }
            }
            if (highest.point.y <= collide.bounds.min.y)
            {
                if (rigid.velocity.y < -15)
                {
                    rigid.velocity = new Vector2(0, -15);
                    momentum = momentum / 2;
                }
                if (rigid.velocity.x != 0)
                    state = PlayerState.Running;
                else
                    state = PlayerState.Idle;
            }
            else
            {
                ContactPoint2D lowest = points[0];
                for (int i = 1; i < points.Length; i++)
                {
                    if (points[i].point.y < lowest.point.y)
                    {
                        lowest = points[i];
                    }
                }
                if (lowest.point.y < collide.bounds.max.y)
                {
                    state = PlayerState.WallSlide;
                    faceRight = highest.point.x > transform.position.x;
                }
            }
        }
 
    }

    private void UpdateAnimator()
    {
        switch (state)
        {
            case PlayerState.Idle:
                anim.Play("Idle", 0, .5f);
                break;
            case PlayerState.Running:
                anim.Play("Running", 0, .5f);
                break;
            case PlayerState.Melee:
                anim.Play("Melee", 0, .5f);
                break;
            case PlayerState.AirSlide:
                anim.Play("Dash", 0, .5f);
                break;
            case PlayerState.Airborne:
                if (rigid.velocity.y > 0)
                    anim.Play("Rising", 0, .5f);
                else
                    anim.Play("Fall", 0, .5f);
                break;
            case PlayerState.WallSlide:
                anim.Play("Slide", 0, .5f);
                break;
            default:
                break;
        }
    }


}
