﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{

    public Animator anim;
    public Rigidbody rb;
    public float speed = 3f;
    bool facingRight;
    public float jumpVelocity;
   
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        facingRight = true;
      

    }

    // Update is called once per frame
    void Update()
    {

        Movement();
        Jump();



       
       
        
    }



    void Movement()
    {


        float move = CrossPlatformInputManager.GetAxis("Horizontal");

        anim.SetFloat("Speed", Mathf.Abs(move));
        rb.velocity = new Vector3(move * speed * Time.deltaTime, rb.velocity.y,rb.velocity.z);

        if (move > 0 && !facingRight)
        {
            Flip();

        }
        else if (move < 0 && facingRight == true)
        {

            Flip();
        
        }


    }





    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

    
    }







    void Jump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        anim.SetBool("Grounded", isGrounded);

        if (CrossPlatformInputManager.GetButtonDown("Jump") && isGrounded == true )
        {
           

            rb.velocity = Vector3.up * jumpVelocity;
            
        }
    
    }



    



}
