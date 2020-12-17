using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Start() Variables
    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D coll;

    // State Machine
    private enum State {idle, running, jumping, falling, hurt};
    private State state = State.idle;
    

    // Inspector Variables
    [SerializeField]private LayerMask ground;
    [SerializeField] private float speed = 60f;
    [SerializeField] private float jumpforce = 40f;
    [SerializeField] private int cherries = 0;
    [SerializeField] private Text cherryText;
    [SerializeField] private float hurtForce = 10f;

    private void Start()
    {
        // Setting Up Variables 
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }


    private void Update()
    {
        if(state != State.hurt)
        {
            Movement();
        }
        AnimationState();
        animator.SetInteger("state", (int)state); // Sets animation basen on Enumerator state
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            Destroy(collision.gameObject);
            cherries += 1;
            cherryText.text = cherries.ToString();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            if(state == State.falling)
            {
                Destroy(other.gameObject);
                Jump();
            }
            else
            {
                state = State.hurt;
                if(other.gameObject.transform.position.x > transform.position.x)
                {
                    // Enemy is to my right, so I should be damaged and move left
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    // Enemy is to my left, so I should be damaged and move right
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
            }
        }
    }

    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");

        //  Player movement/animations
        // Moving Left
        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        // Moving Right
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        // Jumping
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpforce);
        state = State.jumping;
    }

    private void AnimationState()
    {
        // State Machine Logic
        if(state == State.jumping) // In the air
        {
            if(rb.velocity.y < .1f) // Descending
            {
                state = State.falling;
            }
        }

        else if(state == State.falling)
        {
            if (coll.IsTouchingLayers(ground)) // On the Ground
            {
                state = State.idle;
            }
        }
        else if (state == State.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                state = State.idle;
            }
        }

        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            // Moving
            state = State.running;
        }
        else
        {
            // Not Moving
            state = State.idle;
        }
        
    }

}
