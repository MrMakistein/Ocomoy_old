using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class AudioManager : MonoBehaviour {

    public AudioSource clones;
    public AudioSource shield;
    public AudioSource drums;
    public AudioSource melody;
    public AudioSource altar;
    public AudioClip altarClip;
    public AudioSource collect;
    public AudioSource end;
    public AudioSource basis;
    public AudioSource lifeLow;
    public AudioSource vignette;

    public float drumsVol; //oscillating drums
    public float melodyVol; //oscillating melody

    public float lifeLowTime; //time for lifeLow Sound
    public float old_health;

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

        turnDownForWhat = true;
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

        basis.volume = 1 * loudness;

        if (turnDownForWhat)
        {
            loudness = 0;
        }
        else
        {
            loudness = 1;
        }

        drumsVol = Mathf.Sin(Time.frameCount / 400f) * loudness;
        drums.volume = drumsVol;
        //Debug.Log(drumsVol);

        melodyVol = Mathf.Abs(Mathf.Cos(Time.frameCount / 500f)) * loudness;
        melody.volume = melodyVol;
        
        //###LOW HEALTH###
        old_health = health_percent;
        //Calculate Player Health Percent
        health_percent = Player.instance.currentHealth / Player.instance.maxHealth;
        if (health_percent < old_health)
        {
            //Debug.Log("Entered If");
            lifeLowTime = 0;
        }
        if (health_percent <= 0.20)
        {
            lifeLowTime += Time.deltaTime / 6f;
            lifeLow.volume = Mathf.Lerp(1 * loudness, 0, lifeLowTime);
            old_health = health_percent;             
        }
        else
        {
            lifeLow.volume = 0;
        }

       

        //hear vignette according to Distance between Player and closest collectible
        if (Player.instance.collectible_distance < endRad)
        {
            vignette.volume = Mathf.Pow(1f - (Player.instance.collectible_distance - 2) / colRad, 2)*loudness;
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
            shield.volume = 1*loudness;
            drums.volume = 0;
            Player.shieldActive = false;
            stShield = stShield - (Time.deltaTime);
        }
        if (stShield <= 0f)
        {
            shield.volume = 0;
            drums.volume = drumsVol;
            melody.volume = melodyVol;
            stShield = Player.instance.shield_duration - 1;
        }


        //Audio events for clones
        if (Player.clonesActive)
        {
            clones.volume = 1 * loudness;
            melody.volume = 0;
            //Debug.Log("Clones Volume: "+ clones.volume);
            Player.clonesActive = false;
            stClones = Player.instance.clone_time;
            //Debug.Log("showtime"+showtime);
        }

        if (stClones <= 0f || GameObject.FindGameObjectsWithTag("Clone").Length == 0)
        {
            clones.volume = 0;
            melody.volume = melodyVol;
        }
        else
        {
            clones.volume = 1 * loudness;
            melody.volume = 0;
            stClones = stClones - (Time.deltaTime);
        }

        //Debug.Log("distance: " + home_distance);



        //melody at end of game
        if (Player.instance.collectibleCount == 3)
        {
            drums.volume = drumsVol - Mathf.Pow(1.1f - (home_distance - 5) / endRad, 2);
            drums.volume = melodyVol - Mathf.Pow(1.1f - (home_distance - 5) / endRad, 2);
            if (home_distance < endRad)
            {
                end.volume = Mathf.Pow(1.1f - (home_distance - 5) / endRad, 2)*loudness;
            }
            else
            {
                end.volume =0;
                drums.volume = drumsVol;
                melody.volume = melodyVol;
            }
        }       
    }
}
