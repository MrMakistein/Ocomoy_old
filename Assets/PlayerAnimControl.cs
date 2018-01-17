using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimControl : MonoBehaviour {

    public Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Horizontal")  || Input.GetButton("Vertical"))
        {
            anim.SetBool("run", true);
        } else
        {

            anim.SetBool("run", false);
        }
	}
}
