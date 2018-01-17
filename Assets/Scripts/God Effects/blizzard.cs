﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blizzard : MonoBehaviour {


    public float radius = 12;
    public float TimeAlive = 10;

    private GameObject player;    
    Collider[] colliders;
    float startTime;

    // Use this for initialization
    void Start()
    {
        Destroy(this.gameObject, 18);
        player = GameObject.Find("Player");
        startTime = Time.time;
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void Update()
    {
        if (player != null)
        {

            colliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider c in colliders)
            {
                if (c.gameObject != player)
                {
                    continue;
                }
                else if (Time.time-startTime <= 13.5)
                {
                    player.GetComponent<Movement>().SetSlow();
                }
            }
        }
        else
        {
            Debug.LogError("Player not found");
        }
    }
}
