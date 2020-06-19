using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{

    public Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Movement();

        
    }





    void Movement()
    {

        anim.SetFloat("Horizontal", CrossPlatformInputManager.GetAxis("Horizontal"));

        if (CrossPlatformInputManager.GetAxis("Horizontal") < 0f)
        {
            transform.RotateAround(transform.position, transform.up, 180f);


        }
        else if(CrossPlatformInputManager.GetAxis("Horizontal") > 0f)
        {
            transform.RotateAround(transform.position, transform.up, 180f);


        }



    }













}
