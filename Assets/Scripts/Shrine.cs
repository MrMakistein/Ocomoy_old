using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviour {

    public float shrine_cooldown = 10;
    public float shrine_cooldown_timer = 0;
    public float blessing_type = 1;
    public float shrine_id = 1;
    public float shrine_survive = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (shrine_cooldown_timer > 0)
        {
            GetComponent<Renderer>().material.color = Color.red;
            shrine_cooldown_timer -= Time.deltaTime * 1;
        } else
        {
            GetComponent<Renderer>().material.color = Color.gray;
        }

    }
}
