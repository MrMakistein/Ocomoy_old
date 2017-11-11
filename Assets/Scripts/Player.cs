using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour {
    public float maxHealth = 100;
    public float currentHealth;
    public Image healthBar;
    float hit_cooldown_timer;
    public float hit_cooldown = 10;

    // Use this for initialization
    void Start () {
        healthBar.fillAmount = 1;
        currentHealth = maxHealth;
    }
	
	// Update is called once per frame
	void Update () {
        if (hit_cooldown_timer > 0)
        {
            hit_cooldown_timer -= Time.deltaTime * 10;
        }

    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Interactive" && col.gameObject.GetComponent<ThrowObject>().dmg_cooldown >= 1 && hit_cooldown_timer <= 0)
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



        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Shrine" && Input.GetKeyDown("space") && col.gameObject.GetComponent<Shrine>().shrine_cooldown_timer <= 0)
        {
            col.gameObject.GetComponent<Shrine>().shrine_cooldown_timer = col.gameObject.GetComponent<Shrine>().shrine_cooldown;
            print("hi");
        }
    }
}