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
            anim.SetBool("horizontal+", true);
        if (Input.GetKeyDown(KeyCode.K))
            anim.SetBool("dash", true);
        if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetBool("horizontal+", false);
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            anim.SetBool("dash", false);
        }
    }
}
