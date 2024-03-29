﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviour {

    public float shrine_cooldown = 10; // how long the player is locked in the shrine while praying
    [HideInInspector] public float shrine_cooldown_timer = 0;
    public float blessing_spawn_cooldown = 100; // how long the shrine takes to generate a new item after it was picked up
    [HideInInspector] public float blessing_spawn_cooldown_timer = 0;
    public int shrine_id = 1;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {

        // changes the color of the shrine depening on its state
        if (shrine_cooldown_timer > 0)
        {
            GetComponent<Renderer>().material.color = Color.red;
            blessing_spawn_cooldown_timer = blessing_spawn_cooldown;
            shrine_cooldown_timer -= Time.deltaTime * 5;
        }
        else if (blessing_spawn_cooldown_timer > 0)
        {
            GetComponent<Renderer>().material.color = Color.gray;
        }
        else
        {
            GetComponent<Renderer>().material.color = new Color(0.2F, 0.2F, 0.2F, 1);
        }

        // Decreases timer for blessing spawn at shrine
        if (blessing_spawn_cooldown_timer > 0)
        {
            blessing_spawn_cooldown_timer -= Time.deltaTime * 10;
        }
    }
}
