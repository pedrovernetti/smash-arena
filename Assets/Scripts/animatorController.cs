using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animatorController : MonoBehaviour
{
    Animator anim;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.Play("Running");
            anim.SetBool("forward", true);
           
        }
        if (Input.GetKeyDown(KeyCode.K) && anim.GetBool("forward"))
        {
          //  anim.Play("Dash");
            anim.SetBool("dash", true);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            anim.Play("RunningBackward");
            anim.SetBool("backward", true);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.Play("Left");
            anim.SetBool("left", true);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.Play("Right");
            anim.SetBool("right", true);
        }
        
        if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetBool("forward", false);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            anim.SetBool("backward", false);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetBool("left", false);
            anim.Play("Idle");
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("right", false);
            anim.Play("Idle");
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            anim.SetBool("dash", false);
            if(anim.GetBool("forward"))
                anim.Play("Running");
        }
        if (Input.GetKeyDown(KeyCode.J)&& anim.GetBool("forward")==false && anim.GetBool("backward")==false)
        {
            anim.Play("Secret");
            anim.SetBool("secret", true);
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            anim.SetBool("secret", false);
        }

    }
}
