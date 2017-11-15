
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
    public float clone_time = 0;
    public int CloneAmount = 4;
    public GameObject cloneObj;

    //Shield Ability Variables
    public float shield_rotation_speed = 200;
    public float shield_timer = 0;
    public float shield_duration = 50;

    public GameObject shield;



    // Random Shit
    public int equipped_ability = 0;
    public float hit_cooldown_timer;
    public float hit_cooldown = 10;
    private int collectibleCount;
    private GameObject arena;



    // Use this for initialization
    void Start() {
        arena = GameObject.Find("Arena");
        collectibleCount = 0;
        healthBar.fillAmount = 1;
        currentHealth = maxHealth;
        if (cloneObj == null)
        {
            Debug.LogError("No clone specified in player!");
        }
    }



    // Update is called once per frame
    void Update() {

        // Closest Signpost





        //Hit Cooldown Timer
        if (hit_cooldown_timer > 0)
        {
            hit_cooldown_timer -= Time.deltaTime * 10;
        }

        // Health Regeneration
        if (currentHealth < maxHealth)
        {
            currentHealth = currentHealth + (regenSpeed / 200);
            healthBar.fillAmount = currentHealth / maxHealth;
        }

        // Ability Activation
        if (Input.GetKeyDown("space") && !gameObject.GetComponent<Movement>().move_block)
        {


            if (equipped_ability == 2)
            {
                Start_clone_ability();
            }


            if (equipped_ability == 3)
            {
                shield_timer = 1;
                shield.SetActive(true);


            }
            equipped_ability = 0;
        }

        if (shield_timer >= 1)
        {
            Shield_ability();
        }

    }

    private void Start_clone_ability()
    {
        for (int i = 0; i < CloneAmount; i++)
        {
            GameObject currentSpawn = Instantiate(cloneObj, transform.position, Quaternion.identity);
            currentSpawn.transform.parent = gameObject.transform;
        }
    }


    private void Shield_ability()
    {
        shield.transform.position = transform.position;
        shield.transform.Rotate(Vector3.up * Time.deltaTime * shield_rotation_speed, Space.World);

        shield_timer += Time.deltaTime * 5;
        if (shield_timer >= shield_duration)
        {
            shield_timer = 0;
            shield.SetActive(false);

        }
    }

    private void OnCollisionEnter(Collision col)
    {
        GameObject god = GameObject.Find("TheosGott");
        // Test for player/interactive collision and deal the correct amount of damage depening on the weight_class

        GameObject selectedClone = null;
        foreach (ContactPoint c in col.contacts)
        {
            if (c.thisCollider.gameObject.tag == "Clone")
            {
                selectedClone = c.thisCollider.gameObject;
            }
        }

        if (col.gameObject.tag == "Interactive" &&
        !col.gameObject.GetComponent<InteractiveSettings>().isCollectible &&
        col.gameObject.GetComponent<ThrowObject>().dmg_cooldown >= 1 &&
        hit_cooldown_timer <= 0 &&
        !this.GetComponent<Movement>().move_block)
        {

            god.GetComponent<dnd>().ReleaseObject(); // Makes the god drop the object he's holding
            hit_cooldown_timer = hit_cooldown;
            if (selectedClone == null)
            {
                if (col.gameObject.GetComponent<ThrowObject>().weight_class == 1)
                {
                    currentHealth = currentHealth - 10;
                    healthBar.fillAmount = currentHealth / maxHealth;
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
            else
            {
                //trow the object in a random direction
                col.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-10.0F, 10.0F), Random.Range(8.0F, 12.0F), Random.Range(-10.0F, 10.0F));

                //if a clone was hit, destroy him
                Destroy(selectedClone, 0.1f);
            }

            //Object Damage
            col.gameObject.GetComponent<ThrowObject>().object_damage -= 1;
            if (col.gameObject.GetComponent<ThrowObject>().object_damage <= 0)
            {
                Destroy(col.gameObject);
            }

        }

        else if (col.gameObject.tag == "Interactive" && col.gameObject.GetComponent<InteractiveSettings>().isCollectible && col.gameObject.activeSelf)
        {
            col.gameObject.SetActive(false);

            Color[] colors = { Color.red, Color.magenta, Color.cyan, Color.green, Color.gray }; //for testing!
            GetComponent<Renderer>().material.color = colors[collectibleCount]; //for testing

            GameObject[] signposts = GameObject.FindGameObjectsWithTag("Signpost");

            collectibleCount++;
            if (collectibleCount >= arena.GetComponent<SpawnController>().numberOfCollectibles)
            {
                arena.GetComponent<SpawnController>().allCollected = true;
            }

            foreach (GameObject go in signposts)
            {
                go.GetComponent<Signpost>().UpdateSignpostDetection();
            }

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