using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour 
{
    RaycastHit hit1, hit2, edgeHitUp, edgeHitMid;
    public bool UpRayHit, MidRayHit;
    public bool hangingOnLedge;
    public Animator anim;
    public Rigidbody rb;
    public float speed;
    bool facingRight;
    public float jumpVelocity;
    public float edgeRayLength;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public LayerMask edgeMask;
    public bool isGrounded;
    private float jumpTimeCounter = 1f;
    public float jumpTime;
    public CapsuleCollider col;
    public bool Crouching;
    float moveH, moveV;
    public Vector3 edgeGrabOffset;
    //private bool _isJump;
    public Transform EdgePos;

    public bool edgeDetected;
    public Transform headRayB;
    public Transform headRayF;
    public Transform bodyRay;
    public Transform feetRay;
    public float RayLength;
    private bool headRoom;
    public Transform edgeRayUp;
    public Transform edgeRayMid;
    private bool justLanded;
    public AnimationClip climbclip;

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
        //Grounding Check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);


        if (isGrounded)
        {
            hangingOnLedge = false;
            anim.SetBool("Hang", false);
            

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
        else if (moveV >= 0)
        {
            Crouching = false;
            anim.SetBool("Crouching", false);

        }

        //triggers different animation state machines 
        if (Crouching)
        {
            hangingOnLedge = false;

            anim.SetFloat("CrouchWalk", Mathf.Abs(moveH));

            rb.velocity = new Vector3(moveH * (speed / 8), rb.velocity.y, rb.velocity.z);
            col.height = 0.8f;
            col.center = new Vector3(0, 0.45f, 0);
        }
        else
        {
            col.height = 1.45f;
            col.center = new Vector3(0, 0.75f, 0);

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

        

       
            anim.SetBool("Grounded", isGrounded);


            
        
           


        if (CrossPlatformInputManager.GetButton("Jump") && isGrounded == true)
        {

            rb.AddForce(Vector3.up * jumpVelocity);
            anim.SetBool("Jump", true);

        }
        else
        {
            anim.SetBool("Jump", false);

        }

       // if(justLanded ==true )




    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    void RayCheckPlatorm()
    {
        

        ///////////////////////////////////////check headroom 
        if (Physics.Raycast(headRayB.position, headRayB.TransformDirection(Vector3.up), out hit1, RayLength, groundMask) || Physics.Raycast(headRayF.position, headRayF.TransformDirection(Vector3.up), out hit2, RayLength, groundMask))
        {
            Debug.DrawRay(headRayB.position, headRayB.TransformDirection(Vector3.up) * RayLength, Color.green);
            Debug.DrawRay(headRayF.position, headRayF.TransformDirection(Vector3.up) * RayLength, Color.green);
           // Debug.Log("Did HitB");
            headRoom = false;
        }
        else
        {
            Debug.DrawRay(headRayB.position, headRayB.TransformDirection(Vector3.up) * RayLength, Color.white);
            Debug.DrawRay(headRayF.position, headRayF.TransformDirection(Vector3.up) * RayLength, Color.white);
           // Debug.Log("Did not HitF");
            headRoom = true;
        }


        /////////////////////////////////////check ground contact
        if (Physics.Raycast(feetRay.position, feetRay.TransformDirection(Vector3.down), out hit2, 0.2f, groundMask))
        {
            Debug.DrawRay(feetRay.position, feetRay.TransformDirection(Vector3.down) * 0.2f, Color.green);
            //Debug.Log("Did Hit ground");

            if (rb.velocity.y < 0)
            {
                justLanded = true;
                anim.SetBool("Landed", true);
                StartCoroutine(LandingFinish());
            }
        }
        else 
        {
            Debug.DrawRay(feetRay.position, feetRay.TransformDirection(Vector3.down) * 0.2f, Color.white);
           // Debug.Log("Did not Hit ground");
            anim.SetBool("Landed", false);
            justLanded = false;
        }


        ////////////////////////////Edge Raycast Check UP
        if (Physics.Raycast(edgeRayUp.position, edgeRayUp.TransformDirection(Vector3.forward), out edgeHitUp, edgeRayLength, edgeMask))
        {
            Debug.DrawRay(edgeRayUp.position, edgeRayUp.TransformDirection(Vector3.forward) * edgeRayLength, Color.green);

            UpRayHit = true;
                      
        }
        else
        {
            Debug.DrawRay(edgeRayUp.position, edgeRayUp.TransformDirection(Vector3.forward) * edgeRayLength, Color.red);
            UpRayHit = false;
            
        }

        ///////////////////////////////////edge ray mid
        if (Physics.Raycast(edgeRayMid.position, edgeRayMid.TransformDirection(Vector3.forward), out edgeHitMid, edgeRayLength, edgeMask))
        {
            Debug.DrawRay(edgeRayMid.position, edgeRayMid.TransformDirection(Vector3.forward) * edgeRayLength, Color.green);
            MidRayHit = true;
            

        }
        else
        {
            Debug.DrawRay(edgeRayMid.position, edgeRayMid.TransformDirection(Vector3.forward) * edgeRayLength, Color.red);
            MidRayHit = false;

        }





        if (UpRayHit == false && MidRayHit == true && CrossPlatformInputManager.GetButton("Jump"))
        {

            //grab the edge
            Debug.Log("edge grabed");
           
            anim.SetBool("Hang", true);
            rb.isKinematic = true;
            
            hangingOnLedge = true;

            EdgePos = edgeHitMid.collider.gameObject.transform;
           transform.parent = edgeHitMid.transform ;

            
           
           
            



        }
       
      
      

        if (hangingOnLedge)
        {
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            if (v > 0)
            {
                
                anim.SetBool("Climb", true);
               
                

                // StartCoroutine(correctPositionClimb());                     


            }
          
        
        
        }




    }


    IEnumerator correctPositionClimb()
    {
        bool bol = false;
        yield return new WaitForSeconds(3.6f);

        rb.isKinematic = false;
            transform.position = new Vector3(edgeHitMid.collider.gameObject.transform.position.x, edgeHitMid.collider.gameObject.transform.position.y + 0.2f, 0);
        
    }


    
    



    IEnumerator LandingFinish()
    {

        yield return new WaitForSeconds(0.5f);
        justLanded = false;
        anim.SetBool("Landed", false);
    }




   



}//class end
