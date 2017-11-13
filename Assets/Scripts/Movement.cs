using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float mSpeed = 7f;
    public bool move_block = false;



    void Start () {
        
	}
	
	void Update () {

        move_block = false;

        GameObject[] shrines = GameObject.FindGameObjectsWithTag("Shrine");
        foreach (GameObject shrine in shrines)
        {
            if (shrine.GetComponent<Shrine>().shrine_cooldown_timer > 0)
            {
                move_block = true;
            }
        }

        if (!move_block && this.GetComponent<Dash>().dashTimer <= 0)
        {
            // Moves the player forward if WAS or D is pressed
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
     
                gameObject.GetComponent<Rigidbody>().velocity = transform.forward * mSpeed;
            }

            // Rotates the player into the correct direction
            Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
            if (movement != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15F);
            }
        }
        

    }
}
