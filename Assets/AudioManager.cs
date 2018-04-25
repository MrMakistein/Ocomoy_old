using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class AudioManager : MonoBehaviour {

    public AudioSource clones;
    public AudioSource shield;
    public AudioSource drums;
    public AudioSource altar;
    public AudioClip altarClip;
    public AudioSource collect;
    public AudioSource end;
    public AudioSource basis;
    public AudioSource lifeLow;
    public AudioSource vignette;

    public bool turnDownForWhat; //mutes all background music according to loudness
    public float loudness; //controls loudness of music when turnDownForWhat = true;
    float stClones; //showtime for Clones
    float stShield; //showtime for Shield
    public Vector3 spawn_position;
    public float home_distance;
    public float health_percent;
    public float collectible_distance;
    public float endRad; //radius of listening at the end
    public float colRad; //radius of listening for collectibles

    public static AudioManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start () {

        turnDownForWhat = false;
        clones.volume = 0;
        shield.volume = 0;
        end.volume = 0;
        lifeLow.volume = 0;
        loudness = 0;
        vignette.volume = 0;

        colRad = 20;
        endRad = 25;
        stClones = Player.instance.clone_time;
        stShield = Player.instance.shield_duration -1;
        
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log("drums: " + drums.volume);

        //Calculate Player Health Percent
        health_percent = Player.instance.currentHealth / Player.instance.maxHealth;
        //Debug.Log("health: " + health_percent);
        if (health_percent <= 0.25)
        {
            lifeLow.volume = 1;
        } else
        {
            lifeLow.volume = 0;
        }

        //hear vignette according to Distance between Player and closest collectible
        if (Player.instance.collectible_distance < endRad)
        {
            vignette.volume = Mathf.Pow(1f - (Player.instance.collectible_distance - 2) / colRad, 2);
        }
        else
        {
            vignette.volume = 0;
        }
        

        //Calculate Distance between Player and Spawnshrine
        home_distance = Vector3.Distance(Player.instance.transform.position, spawn_position);

        if (Player.instance.equipped_ability != 0 &&  Input.GetKeyDown("space") && altar.isPlaying == false) altar.Play();
        if (Player.collectSound)
        {
            collect.Play();
            Player.collectSound = false;
        }
        //Audio events for shield
        if (Player.shieldActive)
        {
            shield.volume = 1;
            drums.volume = 0;
            Player.shieldActive = false;
            stShield = stShield - (Time.deltaTime);
        }
        if (stShield <= 0f)
        {
            shield.volume = 0;
            drums.volume = 1;
            stShield = Player.instance.shield_duration - 1;
        }


        //Audio events for clones
        if (Player.clonesActive)
        {
            clones.volume = 1;
            Player.clonesActive = false;
            stClones = Player.instance.clone_time;
            //Debug.Log("showtime"+showtime);
        }

        if (stClones <= 0f || GameObject.FindGameObjectsWithTag("Clone").Length == 0)
        {
            clones.volume = 0;
        }
        else
        {
            stClones = stClones - (Time.deltaTime);
        }

        //Debug.Log("distance: " + home_distance);



        //melody at end of game
        if (Player.instance.collectibleCount == 3)
        {
            drums.volume = 1 - Mathf.Pow(1.1f - (home_distance - 5) / endRad, 2);
            if (home_distance < endRad)
            {
                end.volume = Mathf.Pow(1.1f - (home_distance - 5) / endRad, 2);
            }
            else
            {
                end.volume =0;
                drums.volume = 1;
            }
            Debug.Log("drums_volume: "+drums.volume);
        }

        if (turnDownForWhat)
        {
            basis.volume = loudness;
            drums.volume = loudness;
            shield.volume = loudness;
            clones.volume = loudness;
            end.volume = loudness;
            lifeLow.volume = loudness;
        } else
        {
               /* basis.volume = 1;
                drums.volume = 1;
                shield.volume = 1;
                clones.volume = 1;
                end.volume = 1;
                lifeLow.volume = 1; */
        }
    }
}
