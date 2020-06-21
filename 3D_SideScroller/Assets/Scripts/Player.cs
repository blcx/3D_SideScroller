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
    public CapsuleCollider col;
    public bool Crouching;
    float moveH, moveV;
    private bool _isJump;

    public Transform headRay;
    public Transform bodyRay;
    public float RayLength;
    private bool headRoom;


    // ///////////Start is called before the first frame update//////////////////////////////////////////////////
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        facingRight = true;
        col = GetComponent<CapsuleCollider>();
      

    }


    //////////////////////// Update is called once per frame/////////////////////////////////////////////
    void FixedUpdate()
    {

        Movement();
        Jump();
        Crouch();
        RayCheckPlatorm();
              
    }

    /// <summary>
    /// //////////////////////////////////////////////////////////////////////////////////////////////

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
    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////
    void Crouch()
    {
        float moveV = CrossPlatformInputManager.GetAxis("Vertical");


        //checks headroom by raycast and if hit input value are emulated for crouch 
        if (headRoom == false)
        {
            moveV = -1f;
            //Crouching = true;
        }
        else if (headRoom == true && moveV >= 0)
        {
            
           Crouching = false;
            anim.SetBool("Crouching", false);
        }

        //input functions    
        if (moveV < 0)
        {
            Debug.Log("crouching");
            Crouching = true;
            anim.SetBool("Crouching", true);       
            
        }
        else if(moveV >= 0) 
        { 
            Crouching = false;
            anim.SetBool("Crouching", false);

        }

        //triggers different animation state machines 
        if (Crouching)
        {

            anim.SetFloat("CrouchWalk", Mathf.Abs(moveH));
            
            rb.velocity = new Vector3(moveH * (speed / 8), rb.velocity.y, rb.velocity.z);

        }

        if (Crouching)
        {
            col.height = 0.8f;
            col.center = new Vector3(0,0.45f,0);
        }
        else
        {
            col.height = 1.45f;
            col.center = new Vector3(0,0.75f,0);

        }

       
    }

    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);            
    }

    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>


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

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    void RayCheckPlatorm()
    {
        RaycastHit hit1,hit2;


        if (Physics.Raycast(headRay.position, headRay.TransformDirection(Vector3.up), out hit1, RayLength, groundMask))
        {
            Debug.DrawRay(headRay.position, headRay.TransformDirection(Vector3.up) * RayLength, Color.green);
            Debug.Log("Did Hit");
            headRoom = false;
        }
        else
        {
            Debug.DrawRay(headRay.position, headRay.TransformDirection(Vector3.up) * RayLength, Color.white);
            Debug.Log("Did not Hit");
            headRoom = true;
        }



      




    }











}//class end
