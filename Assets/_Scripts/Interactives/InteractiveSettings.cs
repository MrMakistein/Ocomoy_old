using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class InteractiveSettings : MonoBehaviour {
public class InteractiveSettings : MonoBehaviour
{
    public Material normalMaterial;
    public Material collectibleMaterial;
    public float collectible_weight = 10;
    public bool isCollectible = false;
    public bool collectibleOnFloor = true;
    public int combo_state = 0;
    public GameObject combo_particles1;
    public GameObject combo_particles2;
    public GameObject combo_particles3;
    public float combo_particle_reset_timer = 0;


    // Use this for initialization
    void Start()
    {
        //isCollectible = false; //default: not a collectible; will be changed by SpawnController	
        //isCollectible = false;

        GetComponent<Rigidbody>().isKinematic = false; //disable kinematics -> can be grabbed
        GetComponent<Collider>().isTrigger = false;
    }

    //entweder: ausgehen von boolean isCollectible
    //oder: extra Methode ausführen von SpawnController aus! Der spawnt die Collectibles und ändert die Settings!
    public void SetCollectible()
    {


        isCollectible = true; //mark as collectible
                              ////GetComponent<Collider> ().isTrigger = true; //set as trigger => you can enter it to deactivate!

        //Update the mass of the collectible
        this.gameObject.GetComponent<ThrowObject>().InitialMass = collectible_weight;

        //for testing
        //GetComponent<Renderer>().material = collectibleMaterial;
        //GetComponent<Rigidbody> ().isKinematic = true; //don't fall through floor --> PROBLEM: can't pick up

    }

    void Update()
    {
        
        if (combo_particle_reset_timer > 0)
        {
            if (combo_particle_reset_timer >= 2.3f && ComboManager.instance.combo_level == 3)
            {
                combo_particles1.SetActive(true);
            }
             else if (combo_particle_reset_timer >= 2.3f && ComboManager.instance.combo_level == 4)
            {
                combo_particles2.SetActive(true);
            }
             else if (combo_particle_reset_timer >= 2.3f && ComboManager.instance.combo_level == 5)
            {
                combo_particles3.SetActive(true);
            }

            combo_particle_reset_timer = combo_particle_reset_timer - 0.1f;
            
                if (combo_particle_reset_timer > 2.0 && combo_particle_reset_timer <= 2.2)
                {
                    combo_particles1.GetComponent<ComboParticles>().StopParticles();
                    combo_particles2.GetComponent<ComboParticles>().StopParticles();
                    combo_particles3.GetComponent<ComboParticles>().StopParticles();
                    combo_particle_reset_timer = 2.0f;
                }

            if (combo_particle_reset_timer <= 0)
            {
                combo_particles1.SetActive(false);
                combo_particles2.SetActive(false);
                combo_particles3.SetActive(false);
            }
        }

    }

}
