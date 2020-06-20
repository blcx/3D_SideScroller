using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{

    public Animator anim;
    public Rigidbody rb;
    public float speed;
    bool facingRight;
    public float jumpVelocity;
   
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool isGrounded;
    private float jumpTimeCounter = 1f;
    public float jumpTime;
    public Collider col;
    public bool Crouching;
    float moveH, moveV;
    private bool _isJump;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        facingRight = true;
      

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Movement();
        Jump();

        Crouch();


       
       
        
    }



    void Movement()
    {


        moveH = CrossPlatformInputManager.GetAxis("Horizontal");
       
        if (isGrounded)
        {
           

             if (!Crouching)
            {
                
                anim.SetFloat("Speed", Mathf.Abs(moveH));
                rb.velocity = new Vector3(moveH * speed, rb.velocity.y, rb.velocity.z);
            }

            


             //turn face
            if (moveH > 0 && !facingRight)
            {
                Flip();

            }
            else if (moveH < 0 && facingRight == true)
            {

                Flip();

            }

            


        }


    }

    void Crouch()
    {
        float moveV = CrossPlatformInputManager.GetAxis("Vertical");
        
           
        if (moveV < 0)
        {
            Crouching = true;
            anim.SetBool("Crouching", true);

        }
        else if(moveV >= 0) 
        { 
            Crouching = false;
            anim.SetBool("Crouching", false);
        }

        if (Crouching)
        {

            anim.SetFloat("CrouchWalk", Mathf.Abs(moveH));
            
            rb.velocity = new Vector3(moveH * (speed / 8), rb.velocity.y, rb.velocity.z);
        }
       
    }



    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

    
    }




    void Jump()
    {

        //as long as physics sphere detect ground is ground true, else false
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);


        anim.SetBool("Grounded", isGrounded);
        anim.SetBool("Jump", !isGrounded);

        if (CrossPlatformInputManager.GetButton("Jump") && isGrounded == true )
        {
           
            rb.AddForce( Vector3.up * jumpVelocity);
            


        }  



    }













}
