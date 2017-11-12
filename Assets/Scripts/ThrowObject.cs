using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour {

    public float weight_class = 1;
    public float dmg_cooldown = 10;
    public float dmg_cooldown_max = 10;
    public int object_damage = 4;
    public bool isclone = false;

    public GameObject clone1;
    public GameObject clone2;
    public GameObject clone3;
    public GameObject clone4;

    public GameObject clone1_hitbox;
    public GameObject clone2_hitbox;
    public GameObject clone3_hitbox;
    public GameObject clone4_hitbox;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       
        // If no object is selected the dmg_cooldown for the interactive object is reset to the maximum
        if (dnd.draggingObject != null)
        {
            dnd.draggingObject.GetComponent<ThrowObject>().dmg_cooldown = dmg_cooldown_max;
        }
        
        // Dmg cooldown gradually goes back to 0 
        if (dmg_cooldown > 0)
        {
            dmg_cooldown -= Time.deltaTime*10;
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "CloneHitbox1Trigger")
        {
            Invoke("KillClone1", 0.2f);
            isclone = true;
            Debug.Log("hit 1");
        }
        Debug.Log(other.gameObject.name);
    }

    void KillClone1()
    {
        clone1_hitbox.SetActive(false);
        clone1.SetActive(false);
        isclone = false;
    }
}
