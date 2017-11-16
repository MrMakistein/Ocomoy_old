using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour {
    public bool update_weight = true;
    public int weight_class = 1;
    public static float weight_class_1_limit = 2;
    public static float weight_class_2_limit = 6;
    public static float weight_class_3_limit = 10;
    public float dmg_cooldown = 10;
    public float dmg_cooldown_max = 10;
    public int object_damage = 4;
    public bool isclone = false;
    Vector3 finalDirection;
    private float initialMass;

    // Use this for initialization
    void Start () {
    initialMass = gameObject.GetComponent<Rigidbody>().mass;
		if (update_weight == true)
        {
            if(gameObject.GetComponent<Rigidbody>().mass <= weight_class_1_limit)
            {
                weight_class = 1;
            }
            else if (gameObject.GetComponent<Rigidbody>().mass <= weight_class_2_limit)
            {
                weight_class = 2;
            }
            else if (gameObject.GetComponent<Rigidbody>().mass <= weight_class_3_limit)
            {
                weight_class = 3;
            }
            else
            {
                weight_class = 4;
            }
        }
	}

    // Update is called once per frame
    void Update()
    {
        /* if (clonehit_timer > 0)
         {
             clonehit_timer -= Time.deltaTime * 10;
         }
         */

        // If no object is selected the dmg_cooldown for the interactive object is reset to the maximum
        if (dnd.draggingObject != null)
        {
            dnd.draggingObject.GetComponent<ThrowObject>().dmg_cooldown = dmg_cooldown_max;
        }

        // Dmg cooldown gradually goes back to 0 
        if (dmg_cooldown > 0)
        {
            dmg_cooldown -= Time.deltaTime * 10;

        }

        if(initialMass != GetComponent<Rigidbody>().mass && !Player.slowEffect)
        {
            GetComponent<Rigidbody>().mass = initialMass;
        }
    }
}
