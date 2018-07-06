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
            anim.SetBool("forward", true);
        if (Input.GetKeyDown(KeyCode.S))
            anim.SetBool("backward", true);
        if (Input.GetKeyDown(KeyCode.A))
            anim.SetBool("left", true);
        if (Input.GetKeyDown(KeyCode.D))
            anim.SetBool("right", true);
        if (Input.GetKeyDown(KeyCode.K))
            anim.SetBool("dash", true);
        if (Input.GetKeyUp(KeyCode.W))
            anim.SetBool("forward", false);
        if (Input.GetKeyUp(KeyCode.S))
            anim.SetBool("backward", false);
        if (Input.GetKeyUp(KeyCode.A))
            anim.SetBool("left", false);
        if (Input.GetKeyUp(KeyCode.D))
            anim.SetBool("right", false);
        if (Input.GetKeyUp(KeyCode.K))
            anim.SetBool("dash", false);
        if (Input.GetKeyDown(KeyCode.J))
            anim.SetBool("capoeira", true);
        if (Input.GetKeyUp(KeyCode.J))
            anim.SetBool("capoeira", false);

    }
}
