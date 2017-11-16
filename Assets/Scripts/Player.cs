
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



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

    //Slippery Hands Ability Variables
    public int slip_uses = 5;
    private int slips_left = 0;

    //Compass Ability Variables
    public float compass_timer = 0;
    public float compass_duration = 50;
    public GameObject compass;

    // Random Shit
    public int equipped_ability = 0;
    public float hit_cooldown_timer;
    public float hit_cooldown = 10;
    private int collectibleCount;
    private GameObject arena;
    private GameObject god;
    public GameObject healthbar;
    public GameObject display_shield;
    public GameObject display_compass;
    public GameObject display_slippy;
    public GameObject display_clone;
    public GameObject display_slip_uses;




    // Use this for initialization
    void Start() {
        arena = GameObject.Find("Arena");
        
        collectibleCount = 0;
        healthBar.fillAmount = 1;
        currentHealth = maxHealth;
        god = GameObject.Find("TheosGott");

        if (cloneObj == null)
        {
            Debug.LogError("No clone specified in player!");
        }
    }



    // Update is called once per frame
    void Update() {

        healthbar.transform.position = transform.position;
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
        if ( currentHealth <= 0)
        {
            print("Arena wins");
            SceneManager.LoadScene("Menu");
        }

        // Ability Activation
        if (Input.GetKeyDown("space") && !gameObject.GetComponent<Movement>().move_block)
        {
            display_shield.SetActive(false);
            display_compass.SetActive(false);
            display_slippy.SetActive(false);
            display_clone.SetActive(false);


            if (equipped_ability == 2)
            {
                Start_clone_ability();
            }


            if (equipped_ability == 3)
            {
                shield_timer = 1;
                shield.SetActive(true);
            }

            if (equipped_ability == 4)
            {
                compass_timer = 1;
                compass.SetActive(true);
            }

            if (equipped_ability == 5)
            {
                slips_left = slip_uses;
            }

            equipped_ability = 0;
        }

        if (shield_timer >= 1)
        {
            Shield_ability();
        }

        if (compass_timer >= 1)
        {
            Compass_ability();
        }

        if (slips_left >= 1)
        {
            Slipperyhands_ability();
        }


    }


    private void Slipperyhands_ability()
    {
            display_slip_uses.GetComponent<Text>().text = "" + slips_left;

        if (Input.GetKeyDown("space"))
        {
            god.GetComponent<dnd>().ReleaseObject();
            slips_left--;
            if (slips_left == 0)
            {
                display_slip_uses.GetComponent<Text>().text = "";
            }
        }

    }



    private void Compass_ability()
    {
        compass.transform.position = transform.position;
        compass.GetComponent<Compass>().UpdateCompassDetection();

        if (arena.GetComponent<SpawnController>().allCollected)
        {
            compass_timer = compass_duration;
        }

        compass_timer += Time.deltaTime * 5;
        if (compass_timer >= compass_duration)
        {
            compass_timer = 0;
            compass.SetActive(false);

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
                float dmg = col.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
                if (col.gameObject.GetComponent<ThrowObject>().weight_class == 1)
                {
                    while (dmg > 20)
                    {
                        dmg = dmg - 1;
                    }
                    currentHealth = currentHealth - dmg;
                    healthBar.fillAmount = currentHealth / maxHealth;
                }

                if (col.gameObject.GetComponent<ThrowObject>().weight_class == 2)
                {
                    while (dmg > 20)
                    {
                        dmg = dmg - 1;
                    }
                    currentHealth = currentHealth - dmg*2;
                    healthBar.fillAmount = currentHealth / maxHealth;
                }

                if (col.gameObject.GetComponent<ThrowObject>().weight_class == 3)
                {
                    while (dmg > 20)
                    {
                        dmg = dmg - 1;
                    }
                    currentHealth = currentHealth - dmg*3;
                    healthBar.fillAmount = currentHealth / maxHealth;
                }

                if (col.gameObject.GetComponent<ThrowObject>().weight_class == 4)
                {
                    while (dmg > 20)
                    {
                        dmg = dmg - 1;
                    }
                    currentHealth = currentHealth - dmg*4;
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
            if (col.gameObject.GetComponent<Shrine>().shrine_id == 2)
            {
                display_clone.SetActive(true);
            }
            if (col.gameObject.GetComponent<Shrine>().shrine_id == 3)
            {
                display_shield.SetActive(true);
            }
            if (col.gameObject.GetComponent<Shrine>().shrine_id == 4)
            {
                display_compass.SetActive(true);
            }
            if (col.gameObject.GetComponent<Shrine>().shrine_id == 5)
            {
                display_slippy.SetActive(true);
            }
            if (col.gameObject.GetComponent<Shrine>().shrine_id == 1 && arena.GetComponent<SpawnController>().allCollected)
            {
                print("Player wins");
                SceneManager.LoadScene("Menu");
            }
            
        }
    }
}