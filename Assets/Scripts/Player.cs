
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour {
    // Health Variables
    public float maxHealth = 100;
    public float currentHealth;
    public Image healthBar;
    public float regenSpeed = 10;

    //Clone Ability Variables
    public float clone_timer = 0;
    public GameObject clone1;
    public GameObject clone2;
    public GameObject clone3;
    public GameObject clone4;
    public GameObject clone1_hitbox;
    public GameObject clone2_hitbox;
    public GameObject clone3_hitbox;
    public GameObject clone4_hitbox;
    public float speed = 1.0F;
    private float startTime;



    // Random Shit
    public int equipped_ability = 0;
    float hit_cooldown_timer;
    public float hit_cooldown = 10;
	private int collectibleCount;
    

    // Use this for initialization
    void Start () {
		collectibleCount = 0;
        healthBar.fillAmount = 1;
        currentHealth = maxHealth;
    }



    // Update is called once per frame
    void Update () {
        //Hit Cooldown Timer
        if (hit_cooldown_timer > 0)
        {
            hit_cooldown_timer -= Time.deltaTime * 10;
        }

        // Health Regeneration
        if (currentHealth < maxHealth)
        {
            currentHealth = currentHealth + (regenSpeed/200);
            healthBar.fillAmount = currentHealth / maxHealth;
        }

        // Ability Activation
        if (Input.GetKeyDown("space") && equipped_ability >= 2 && !gameObject.GetComponent<Movement>().move_block)
        {
            print("BOOM");

            if (equipped_ability >= 2)
            {
                
                clone_timer = 1;
                clone1.SetActive(true);
                clone1_hitbox.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

                //clone2.SetActive(true);
                //clone3.SetActive(true);
                //clone4.SetActive(true);




            }



            equipped_ability = 0;
            
        }

        if (clone_timer >= 1)
        {
            Clone_ability();
        }


    }

    

    private void Clone_ability()
    {
        Vector3 position1 = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);
        if (clone_timer < 1)
        {
            clone1_hitbox.transform.position = Vector3.Lerp(clone1_hitbox.transform.position, position1, Time.deltaTime);
        } else
        {
            clone1_hitbox.transform.position = transform.position;
            clone1_hitbox.transform.position = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);

        }



        //clone1.transform.position = transform.position;
        //clone1.transform.position = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);
        //clone1_hitbox.transform.position = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);
        clone1_hitbox.transform.position = Vector3.Lerp(clone1_hitbox.transform.position, position1, Time.deltaTime);

        //clone2.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2);
        //clone3.transform.position = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z);
        //clone4.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2);

        clone1.transform.rotation = transform.rotation;
        //clone2.transform.rotation = transform.rotation;
        //clone3.transform.rotation = transform.rotation;
        //clone4.transform.rotation = transform.rotation;


        clone_timer += Time.deltaTime*5;

        



        if (clone_timer >= 50)
        {
            clone_timer = 0;
            clone1.SetActive(false);
            clone2.SetActive(false);
            clone3.SetActive(false);
            clone4.SetActive(false);


        }

    }

    private void OnCollisionEnter(Collision col)
    {
        // Test for player/interactive collision and deal the correct amount of damage depening on the weight_class
		if (col.gameObject.tag == "Interactive" && !col.gameObject.GetComponent<InteractiveSettings>().isCollectible && col.gameObject.GetComponent<ThrowObject>().dmg_cooldown >= 1 && hit_cooldown_timer <= 0 && !this.GetComponent<Movement>().move_block)
        {
            
            hit_cooldown_timer = hit_cooldown;
            if (col.gameObject.GetComponent<ThrowObject>().weight_class == 1)
            {
                currentHealth = currentHealth - 10;
                healthBar.fillAmount = currentHealth/maxHealth;
            }

            if (col.gameObject.GetComponent<ThrowObject>().weight_class == 2)
            {
                currentHealth = currentHealth - 20;
                healthBar.fillAmount = currentHealth / maxHealth;
            }

            if (col.gameObject.GetComponent<ThrowObject>().weight_class == 3)
            {
                currentHealth = currentHealth - 30;
                healthBar.fillAmount = currentHealth / maxHealth;
            }

            if (col.gameObject.GetComponent<ThrowObject>().weight_class == 4)
            {
                currentHealth = currentHealth - 40;
                healthBar.fillAmount = currentHealth / maxHealth;
            }

            //Object Damage
            col.gameObject.GetComponent<ThrowObject>().object_damage -= 1;
            if (col.gameObject.GetComponent<ThrowObject>().object_damage <= 0)
            {
                Destroy(col.gameObject);
            }
				
		} else if(col.gameObject.tag == "Interactive" && col.gameObject.GetComponent<InteractiveSettings>().isCollectible){
			col.gameObject.SetActive (false); //collect/destroy uppon hitting
			Color[] colors = {Color.red, Color.magenta, Color.cyan, Color.green, Color.gray}; //for testing!
			GetComponent<Renderer>().material.color = colors[collectibleCount]; //for testing

			collectibleCount++;
		}
    }

    private void OnTriggerStay(Collider col)
    {
        // Tests for players pressing spacebar while standing in shrine
        if (col.gameObject.tag == "Shrine" && Input.GetKeyDown("space") && col.gameObject.GetComponent<Shrine>().shrine_cooldown_timer <= 0 && col.gameObject.GetComponent<Shrine>().blessing_spawn_cooldown_timer <= 0)
        {
            col.gameObject.GetComponent<Shrine>().shrine_cooldown_timer = col.gameObject.GetComponent<Shrine>().shrine_cooldown;
            equipped_ability = col.gameObject.GetComponent<Shrine>().shrine_id;


        }
    }
}