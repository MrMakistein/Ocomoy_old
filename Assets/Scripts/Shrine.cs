using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviour {

    public float shrine_cooldown = 10;
    public float shrine_cooldowntimer = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (shrine_cooldowntimer > 0)
        {
            shrine_cooldowntimer -= Time.deltaTime * 1;
        }

        
    }
}
