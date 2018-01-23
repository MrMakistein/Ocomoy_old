using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimControl : MonoBehaviour {

    public Animator anim;
    //Time it takes to transition between running and idle
    public float transitionTime = 0.1f;
    //used to blend between running and idle
    //0 = idle, 1 = running
    float blendVal = 0;

    private GameObject Player;
    // Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        Player = GameObject.Find("Player");
    }
	
	// Update is called once per frame
	void Update () {
        if (!Player.GetComponent<Movement>().move_block && (Input.GetButton("Horizontal")  || Input.GetButton("Vertical")))
        {
            blendVal = blendVal >= 1 ? 1 : blendVal + (Time.deltaTime / transitionTime);
        } else
        {
            blendVal = blendVal <= 0 ? 0 : blendVal - (Time.deltaTime / transitionTime);
        }
        anim.SetFloat("Blend", blendVal);
    }
}
