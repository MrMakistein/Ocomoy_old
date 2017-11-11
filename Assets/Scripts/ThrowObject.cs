using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour {

    public float weight_class = 1;
    public float dmg_cooldown = 10;
    public float dmg_cooldown_max = 10;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (dnd.draggingObject != null)
        {
            dnd.draggingObject.GetComponent<ThrowObject>().dmg_cooldown = dmg_cooldown_max;
           
        }
        
        if (dmg_cooldown > 0)
        {
            dmg_cooldown -= Time.deltaTime*10;
        }
        
    }
}
