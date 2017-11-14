
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
    public GameObject CloneSpawnCollider;

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
    void Start () {
        arena = GameObject.Find("Arena");
        collectibleCount = 0;
        healthBar.fillAmount = 1;
        currentHealth = maxHealth;
    }



    // Update is called once per frame
    void Update () {

        // Closest Signpost





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
        if (Input.GetKeyDown("space") && !gameObject.GetComponent<Movement>().move_block)
        {

  
            if (equipped_ability == 1)
            {
                //Collider box will grow to push the player away from walls
                CloneSpawnCollider.SetActive(true);
                CloneSpawnCollider.GetComponent<BoxCollider>().size = Vector3.Lerp(new Vector3(0.8f, 1.5f, 0.8f), new Vector3(4f, 1.5f, 4f), Time.deltaTime * 1000);
                // After a short delay the clone ability is started
                Invoke("Start_clone_ability", 0.2f);
            }


            if (equipped_ability == 2)
            {
                shield_timer = 1;
                shield.SetActive(true);


            }
            equipped_ability = 0;
        }

        if (clone_timer >= 1)
        {
            Clone_ability();
        }

        if (shield_timer >= 1)
        {
            Shield_ability();
        }

    }

    private void Start_clone_ability()
    {
        clone_timer = 1;
        clone1.SetActive(true);
        clone2.SetActive(true);
        clone3.SetActive(true);
        clone4.SetActive(true);
        clone1_hitbox.SetActive(true);
        clone2_hitbox.SetActive(true);
        clone3_hitbox.SetActive(true);
        clone4_hitbox.SetActive(true);
    }

    private void Clone_ability()
    {
        //Position to interpolate from
        Vector3 original_position1 = new Vector3(transform.position.x - 0.35f, transform.position.y, transform.position.z);
        Vector3 original_position2 = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.35f);
        Vector3 original_position3 = new Vector3(transform.position.x + 0.35f, transform.position.y, transform.position.z);
        Vector3 original_position4 = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.35f);

        //Position to interpolate to
        Vector3 position1 = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);
        Vector3 position2 = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2);
        Vector3 position3 = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z);
        Vector3 position4 = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2);

        // Interpolate hitboxes from center to the sides at the start of the ability cast
        if (clone_timer < 1.1f)
        {
            clone1_hitbox.transform.position = Vector3.Lerp(original_position1, position1, Time.deltaTime * 50);
            clone2_hitbox.transform.position = Vector3.Lerp(original_position2, position2, Time.deltaTime * 50);
            clone3_hitbox.transform.position = Vector3.Lerp(original_position3, position3, Time.deltaTime * 50);
            clone4_hitbox.transform.position = Vector3.Lerp(original_position4, position4, Time.deltaTime * 50);

        }
        else
        {
         // After the cast their position is set manually   
            clone1_hitbox.transform.position = position1;
            clone2_hitbox.transform.position = position2;
            clone3_hitbox.transform.position = position3;
            clone4_hitbox.transform.position = position4;
            CloneSpawnCollider.SetActive(false);

        }

        // Visual clones are set to the correct position
        clone1.transform.position = position1;
        clone2.transform.position = position2;
        clone3.transform.position = position3;
        clone4.transform.position = position4;

        // Visual clone rotation is matched with the player
        clone1.transform.rotation = transform.rotation;
        clone2.transform.rotation = transform.rotation;
        clone3.transform.rotation = transform.rotation;
        clone4.transform.rotation = transform.rotation;

        clone_timer += Time.deltaTime*5;

        // If the clone timer reaches 50+ the ability cast is over and everything is reset
        if (clone_timer >= 50)
        {
            clone_timer = 0;
            clone1.SetActive(false);
            clone2.SetActive(false);
            clone3.SetActive(false);
            clone4.SetActive(false);
            clone1_hitbox.SetActive(false);
            clone2_hitbox.SetActive(false);
            clone3_hitbox.SetActive(false);
            clone4_hitbox.SetActive(false);
        }
    }
    
    private void Shield_ability()
    {
        shield.transform.position = transform.position;
        shield.transform.Rotate(Vector3.up * Time.deltaTime* shield_rotation_speed, Space.World);

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
        if (col.gameObject.tag == "Interactive" && 
            !col.gameObject.GetComponent<InteractiveSettings>().isCollectible && 
            col.gameObject.GetComponent<ThrowObject>().dmg_cooldown >= 1 && 
            hit_cooldown_timer <= 0 
            && !this.GetComponent<Movement>().move_block &&
            !col.gameObject.GetComponent<ThrowObject>().isclone)
        {

            god.GetComponent<dnd>().ReleaseObject(); // Makes the god drop the object he's holding
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
				
		} else if(col.gameObject.tag == "Interactive" && col.gameObject.GetComponent<InteractiveSettings>().isCollectible && col.gameObject.activeSelf){
			col.gameObject.SetActive (false);

            Color[] colors = {Color.red, Color.magenta, Color.cyan, Color.green, Color.gray}; //for testing!
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