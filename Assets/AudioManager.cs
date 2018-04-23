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
    float stClones; //showtime for Clones
    float stShield; //showtime for Shield
                   
    void Start () {
        clones.volume = 0;
        shield.volume = 0;
        stClones = Player.instance.clone_time;
        stShield = Player.instance.shield_duration -1;
    }
	
	// Update is called once per frame
	void Update () {

      
        if (Player.instance.equipped_ability != 0 &&  Input.GetKeyDown("space")) altar.Play();
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
            stShield = stShield - (Time.deltaTime);
        }
        if (stShield <= 0f)
        {
            shield.volume = 0;
            drums.volume = 1;
            Player.shieldActive = false;
        }

        //Audio events for clones
        if (Player.clonesActive)
        {
            clones.volume = 1;
            if (stClones == Player.instance.clone_time) altar.Play();
            stClones = stClones - (Time.deltaTime);
            //Debug.Log("showtime"+showtime);
        }
        if (stClones <= 0f)
        {
            clones.volume = 0;
            Player.clonesActive = false;
        }
    }
}
