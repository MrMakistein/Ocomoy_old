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

    public bool muteMusic; //mutes all background music
    float stClones; //showtime for Clones
    float stShield; //showtime for Shield
    public Vector3 spawn_position;
    public float home_distance;
    public float health_percent;
    public float collectible_distance;
    public float endRad; //radius of listening at the end

    public static AudioManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start () {

        muteMusic = false;
        clones.volume = 0;
        shield.volume = 0;
        end.volume = 0;

        endRad = 25;
        stClones = Player.instance.clone_time;
        stShield = Player.instance.shield_duration -1;
        
    }
	
	// Update is called once per frame
	void Update () {

        //Calculate Player Health Percent
        health_percent = Player.instance.currentHealth / Player.instance.maxHealth;

        // Get Distance between Player and closest collectible
        // Player.instance.collectible_distance;

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

        if (stClones <= 0f)
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

        if (muteMusic)
        {
            basis.volume = 0;
            drums.volume = 0;
            shield.volume = 0;
            clones.volume = 0;
            end.volume = 0;
        }
    }
}
