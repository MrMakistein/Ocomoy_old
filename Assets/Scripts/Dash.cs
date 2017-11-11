using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour {
    private float coolDownAmount = 2f;
    private float coolDownTimer;
    public float dashSpeed = 10;
    public float dashDuration = 0.3f;
    private float dashTimer = 0f;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        if (coolDownTimer > 0)
        {
            coolDownTimer -= Time.deltaTime;
        }

        if (dashTimer > 0)
        {
            transform.Translate(dashSpeed* Vector3.forward * Time.deltaTime);
            dashTimer -= Time.deltaTime;
        }



        if (Input.GetKey(KeyCode.LeftShift) && coolDownTimer <= 0)
        {
           
            dashTimer = dashDuration;
            coolDownTimer = coolDownAmount;
        }
		
	}
}
