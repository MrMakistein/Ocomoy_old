﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

public class GodEffects : MonoBehaviour
{
    public enum GodEffectType
    {
        tornado, rain, earthshatter, thunder, avalanche, blizzard
    };

    //how the effect should be triggered
    //true = throw, false = click
    public bool ThrowOrClick = true; 

    public Material chargeMaterial;
	[Space(10)]
    public GameObject tornadoEffect;
    public GameObject rainEffect;
   	public GameObject earthshatterEffect;
    public GameObject thunderEffect;
    public GameObject avalancheEffect;
    public GameObject blizzardEffect;
	[Space(10)]
    public int uid;
    public GameObject display;

	[Header("Audio Sources")]
	//public AudioSource tornadoSound;
	//public AudioSource rainSound;
	public AudioSource blizzardSound;
	public AudioSource thunderSound;
	//public AudioSource earthshatterSound;
	//public AudioSource avalancheSound;


    private bool charged = false;

    public GodEffectType CurrentType = 0;
    // Use this for initialization
    void Start()
    {
        CurrentType = (GodEffectType) Random.Range(0, 6);
        display = this.gameObject.transform.GetChild(0).GetChild(0).gameObject;
    }


    // Update is called once per frame
    void Update()
    {
        //to display the correct item
        if (CurrentType == GodEffectType.tornado)
        {
            display.gameObject.GetComponent<Text>().text = "tornado";
        }
        if (CurrentType == GodEffectType.rain)
        {
            display.gameObject.GetComponent<Text>().text = "rain";
        }
        if (CurrentType == GodEffectType.earthshatter)
        {
            display.gameObject.GetComponent<Text>().text = "earthshatter";
        }
        if (CurrentType == GodEffectType.thunder)
        {
            display.gameObject.GetComponent<Text>().text = "thunder";

        }
        if (CurrentType == GodEffectType.avalanche)
        {
            display.gameObject.GetComponent<Text>().text = "avalanche";
        }
        if (CurrentType == GodEffectType.blizzard)
        {
            display.gameObject.GetComponent<Text>().text = "blizzard";
        }

        if (!charged && dnd.draggingObject == this.gameObject && Input.GetMouseButton(1))
        {
            charged = true;
            GetComponent<Renderer>().material = chargeMaterial;
        }


        if(charged && Input.GetMouseButtonUp(1) && !ThrowOrClick)
        {
			this.gameObject.SetActive (false); //deactivate god object (destroy after Sound has played)
			//new calculated Position, to spawn the effect on the ground(with a slight offset)
            SpawnEffect(CurrentType, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z));
			Destroy(this.gameObject);
        }
    }

	private void PlayEffectSound(){
		//Plays the audio clip for the spawned effect Type
		switch (CurrentType) {
		/*case GodEffectType.tornado:
			tornadoSound.Play ();
			break;
		*//*case GodEffectType.rain:
			rainSound.Play ();
			break;
		*/case GodEffectType.thunder:
			thunderSound.Play ();
			break;
		/*case GodEffectType.earthshatter:
			earthshatterSound.Play();
			break;
		*//*case GodEffectType.avalanche:
			avalancheSound.Play ();
			break;
		*/case GodEffectType.blizzard:
			blizzardSound.Play ();
			break;
		}
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (charged && ThrowOrClick)
        {
            SpawnEffect(CurrentType, this.gameObject.transform.position);
			Destroy(this.gameObject);
        }
    }

    private void SpawnEffect(GodEffectType effectType, Vector3 pos)
    {
        //used for respawn
        GameObject[] godObjectSpawnPositions = GameObject.FindGameObjectsWithTag("GodObjectSpawnPosition");

        foreach (GameObject godObjectSpawnPosition in godObjectSpawnPositions)
        {
            if (uid == godObjectSpawnPosition.GetComponent<GodEffectSpawnPosition>().uid)
            {
                godObjectSpawnPosition.GetComponent<GodEffectSpawnPosition>().new_spawn_timer = godObjectSpawnPosition.GetComponent<GodEffectSpawnPosition>().new_spawn_duration;
            }

        }



        //spawnes the correct item
        switch (effectType)
        {
            case GodEffectType.tornado:
                Instantiate(tornadoEffect, pos, Quaternion.identity);
                break;
            case GodEffectType.rain:
                Instantiate(rainEffect, pos, Quaternion.identity);
                break;
            case GodEffectType.avalanche:
                Instantiate(avalancheEffect, pos, Quaternion.identity);
                break;
			case GodEffectType.blizzard:
                Instantiate(blizzardEffect, pos, Quaternion.identity);
                break;
            case GodEffectType.earthshatter:
                Instantiate(earthshatterEffect, pos, Quaternion.identity);
                break;
		case GodEffectType.thunder:
                Instantiate(thunderEffect, pos, Quaternion.identity);
                break;
            default:
                break;
        }
    }
}
