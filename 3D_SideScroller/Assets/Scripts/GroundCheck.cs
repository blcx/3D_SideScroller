using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    Player player;
    Animator anim;
    private void Awake()
    {
        player = GetComponentInParent<Player>();
        anim = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            player.OnGround = true;

            anim.SetBool("Grounded", true);
        }



        
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Ground")
        {
            player.OnGround = false;
            anim.SetBool("Grounded", false);

        }


    }


}
