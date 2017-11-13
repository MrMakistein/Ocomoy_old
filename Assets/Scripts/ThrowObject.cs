using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour {

    public int weight_class = 1;
    public float dmg_cooldown = 10;
    public float dmg_cooldown_max = 10;
    public int object_damage = 4;
    public bool isclone = false;
    Vector3 finalDirection;

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
            dmg_cooldown -= Time.deltaTime*10;
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        GameObject player = GameObject.Find("Player");
        GameObject god = GameObject.Find("TheosGott");
  
        if (player.GetComponent<Player>().hit_cooldown_timer <= 0 && dmg_cooldown >= 1 && 
            (other.name == "CloneHitbox1Trigger" || other.name == "CloneHitbox2Trigger" || other.name == "CloneHitbox3Trigger" || other.name == "CloneHitbox4Trigger"))
        {
            isclone = true;
            player.GetComponent<Player>().hit_cooldown_timer = player.GetComponent<Player>().hit_cooldown;
            god.GetComponent<dnd>().ReleaseObject();
            this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            Invoke("GiveMotion", 0.02f);


            if (other.name == "CloneHitbox1Trigger")
            {
                Invoke("KillClone1", 0.01f);
            }

            if (other.name == "CloneHitbox2Trigger")
            {
                Invoke("KillClone2", 0.01f);
            }

            if (other.name == "CloneHitbox3Trigger")
            {
                Invoke("KillClone3", 0.01f);
            }

            if (other.name == "CloneHitbox4Trigger")
            {
                Invoke("KillClone4", 0.01f);
            }

        }
           

    }
    void GiveMotion()
    {
        this.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-10.0F, 10.0F), Random.Range(8.0F, 12.0F), Random.Range(-10.0F, 10.0F));
    }
   

    void KillClone1()
    {
        clone1_hitbox.SetActive(false);
        clone1.SetActive(false);
        isclone = false;
    }
    void KillClone2()
    {
        clone2_hitbox.SetActive(false);
        clone2.SetActive(false);
        isclone = false;
    }
    void KillClone3()
    {
        clone3_hitbox.SetActive(false);
        clone3.SetActive(false);
        isclone = false;
    }
    void KillClone4()
    {
        clone4_hitbox.SetActive(false);
        clone4.SetActive(false);
        isclone = false;
    }
}
