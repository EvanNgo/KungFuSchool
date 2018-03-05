﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlelr : MonoBehaviour {
    public float maxSpeed;
    public float jumpHeight;
    bool facingRight;
    bool grounded;
    int jumpCount;
    Rigidbody2D myBody;
    Animator myAni;
    // Use this for initialization
    void Start() {
        myBody = GetComponent<Rigidbody2D>();
        myAni = GetComponent<Animator>();
        facingRight = true;
    }

    // Update is called once per frame
    void Update() {
        print("update");
        float move = Input.GetAxis("Horizontal");
        myBody.velocity = new Vector2(move * maxSpeed, myBody.velocity.y);
        myAni.SetFloat("Speed", Mathf.Abs(move));
        if (move > 0 && !facingRight) {
            faceCotnroll();
        }
        else if (move < 0 && facingRight)
        {
            faceCotnroll();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (grounded)
            {
                myAni.SetFloat("Speed", 0);
                myAni.SetBool("Jumping", true);
                grounded = false;
                myBody.velocity = new Vector2(myBody.velocity.x, jumpHeight);
            }
        }
    }

    void faceCotnroll()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            myAni.SetBool("Jumping",false);
            grounded = true;
        }
    }
}