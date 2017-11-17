using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class earthshatter : MonoBehaviour {
    public float radius = 12;
    public int timeReversed = 2;
    
    private GameObject player;

    Collider[] colliders;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");        
        
        if (player != null)
        {
            colliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider c in colliders)
            {
                if (c.gameObject != player)
                {
                    continue;
                }
                else
                {
                    player.GetComponent<Movement>().reversed = true;
                    Destroy(this, destroyTime());
                }
            }
        }
        else
        {
            Debug.LogError("Player not found");
        }
    }

    private float destroyTime()
    {
        //calculates time stunned based on distance to the epicenter
        return timeReversed * (1-(Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(player.transform.position.x, player.transform.position.z)))/radius);
    }
    

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void OnDestroy()
    {
        foreach (Transform child in transform)
        {
                Destroy(child.gameObject);
        }
        player.GetComponent<Movement>().reversed = false;
        Destroy(this.gameObject);
    }  
    
}

