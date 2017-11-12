
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



    // Random Shit
    public float equipped_ability;
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
            
        }
    }
}