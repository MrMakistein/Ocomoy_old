﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Holds methods for changing settings  
 * Accessed by UI (for invoking function calls) and MenuManager (for logging).
 **/
public class Settings : MonoBehaviour {
	//all private variables go here
	private GameObject _player;
	private GameObject _god;
	public AudioClip[] clips;
	public GameObject audioManager;
	private GameObject _input_notes;


	//SETTINGS VARIABLES (public; appear in JSON file) //TODO @Theo @Maki: bitte alle anderen variablen soweit es geht private machen, sonst sind die im logfile drin ;D
	#region settings variables
	[Header("Player")]
	public bool dashEnabled;
	public int characterSpeed; //0-slow, 1-medium, 2-fast
	public int healthSystem; //0-regen, 1-health packs

	[Header("God")]
	public int objectSpeed; //0-slow, 1-medium, 2-fast
	public bool heightLockEnabled;

	[Header("General")]
	public string notes;
	public int screenShakeMode;
    public int rating;
	public bool musicEnabled;
	public int musicMode; //0-with lead, 1-no lead
	#endregion


	// Use this for initialization
	void Start () {
		//Autointialize
		_player = GameObject.Find ("Player");
		_god = GameObject.Find ("TheosGott");

        _input_notes = GameObject.Find ("AdditionalNotes");
		notes = _input_notes.GetComponentInChildren<Text> ().text;
	}

	//------------------- METHODS FOR ACTUALLY CHANGING SETTINGS -------------------------
	//############## 	character 	##############
	#region character
	public void SetDashEnabled(bool isEnabled){
		dashEnabled = isEnabled;
		_player.GetComponent<Dash> ().enabled = isEnabled; //TODO: test!
	}

	public void ChangeCharacterSpeed(int mode){ //TODO @Maki
		characterSpeed = mode; //update variables
		switch (mode) {
		case 0: //slow
			_player.GetComponent<Movement>().mSpeed = 6;
			_player.GetComponent<Movement>().initialmSpeed = 6;

			break;
		case 1: //medium
			_player.GetComponent<Movement>().mSpeed = 7;
			_player.GetComponent<Movement>().initialmSpeed = 7;
			break;
		case 2: //fast
			_player.GetComponent<Movement>().mSpeed = 8;
			_player.GetComponent<Movement>().initialmSpeed = 8;
			break;
		default:
			characterSpeed = 1; //set medium speed as default in case of error
			break;
		}
	}

	public void ChangeHealthSystem(int mode){ //TODO @Maki
		healthSystem = mode;
		switch (mode) {
		case 0: //Regeneration
			_player.GetComponent<Player>().regenSpeed = 2;
			break;
		case 1: //Health Packs (no regen + collectible as 'medikit')
			_player.GetComponent<Player>().regenSpeed = 0;
			break;
		default:
			healthSystem = 0; //set no regen as default in case of error
			Debug.Log ("Yer an idiot");
			break;
		}
	}
	#endregion

	//############## 	  god 		##############
	#region god
	public void ChangeObjectSpeed(int mode){ //TODO @Theo
		objectSpeed = mode;
		switch (mode) {
		case 0: //slow
			dnd.instance.forceStrenght = 150;

			dnd.instance.InfluenceWeightClass1 = 1;
			dnd.instance.InfluenceWeightClass2 = 1;
			dnd.instance.InfluenceWeightClass3 = 1;
			dnd.instance.InfluenceWeightClass4 = 1;
			break;
		case 1: //medium
			dnd.instance.forceStrenght = 200;

			dnd.instance.InfluenceWeightClass1 = 2;
			dnd.instance.InfluenceWeightClass2 = 2;
			dnd.instance.InfluenceWeightClass3 = 2;
			dnd.instance.InfluenceWeightClass4 = 2;
			break;
		case 2: //fast
			dnd.instance.forceStrenght = 300;
			dnd.instance.InfluenceWeightClass1 = 5;
			dnd.instance.InfluenceWeightClass2 = 5;
			dnd.instance.InfluenceWeightClass3 = 5;
			dnd.instance.InfluenceWeightClass4 = 5;

			break;
		default:
			objectSpeed = 2; //set to medium in case of error
			break;
		}
	}

	public void ToggleHeightLock(bool isEnabled){
		heightLockEnabled = isEnabled;

		_god.GetComponent<dnd> ().heightLock = !(_god.GetComponent<dnd> ().heightLock);
	}
	#endregion

	//############## 	general		##############
	#region general
	public void SetScreenshake(float amount){//TODO: @Theo
		screenShakeMode = (int) Mathf.Ceil(amount);//conversion to int as slider can only pass float values
		switch (screenShakeMode) {
            case 0: //0

                CameraControl.instance.cameraShakeStrength = 0;
                CameraControl.instance.defaultShakeTime = 0;
                break;
            case 1: //medium
                CameraControl.instance.cameraShakeStrength = 0.05f;
                CameraControl.instance.defaultShakeTime = 0.05f;

                break;
            case 2: //9000

                CameraControl.instance.cameraShakeStrength = 0.2f;
                CameraControl.instance.defaultShakeTime = 0.1f;
                break;
            default:
                screenShakeMode = 0; //set to off per default
			Debug.Log ("My screenshake brings all the boys to the yard");
			break;
		}
	}
		
    public void SetRating(float ratingAmount)
    {
        this.rating = (int)(ratingAmount);
        GameObject.Find("SLIDER_Xp").GetComponent<Slider>().value = rating;
    }

	public void SetNotes(string theNotes){ //set notes to any text 
		Debug.Log ("Notes: "+theNotes);
		notes = theNotes;		
	}
	/*
	public void SetNotes(){
		SetNotes (_input_notes.GetComponentInChildren<Text> ().text); //set notes with current text in menu
	}
	*/
		
	public void EnableMusic(bool isEnabled){ //TODO: @Maki
		musicEnabled = isEnabled;
		if (musicEnabled)
        {
            audioManager.GetComponent<AudioSource>().Play();
        } else
        {
            audioManager.GetComponent<AudioSource>().Stop();
        }
	}

	public void ChangeMusicMode(int mode){ //TODO: @Maki
		musicMode = mode;
		switch (mode) {
		case 0: //music variation 1
                audioManager.GetComponent<AudioSource>().Stop();
			audioManager.GetComponent<AudioSource>().clip = clips[1];
			audioManager.GetComponent<AudioSource>().Play();

			Debug.Log("m1");


			break;
		case 1: //music variation 2
			audioManager.GetComponent<AudioSource>().Stop();
            audioManager.GetComponent<AudioSource>().clip = clips[0];
            audioManager.GetComponent<AudioSource>().Play();

			Debug.Log("m2");

			break;

		default:
			musicMode = 1; //set to variation 1 per default
			Debug.Log ("'Something went wrong :('");
			break;
		}
	}
	#endregion
}