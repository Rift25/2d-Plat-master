using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eagle : Enemy
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    [SerializeField] private float flySpeed;
    [SerializeField] private LayerMask Default;
    private Collider2D coll;
    private Rigidbody2D rb;

    private bool facingLeft = true;

    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (facingLeft)
        {
            //Test to see if we are beyond the leftCap
            if (transform.position.x > leftCap)
            {
                //Make sure the sprite is facing the right location, and if it is not, then face the right direction
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                if (coll.IsTouchingLayers(Default))
                {
                    //Jump
                    rb.velocity = new Vector2(-flySpeed, 0);
                }
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            //Test to see if we are beyond the rightCap
            if (transform.position.x < rightCap)
            {
                //Make sure the sprite is facing the right location, and if it is not, then face the right direction
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                if (coll.IsTouchingLayers(Default))
                {
                    //Jump
                    rb.velocity = new Vector2(flySpeed, 0);
                }
            }
            else
            {
                facingLeft = true;
            }
        }
    }
}
