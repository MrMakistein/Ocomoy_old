﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Areas : MonoBehaviour {
	private List<GameObject> areaInteractives; //all interactives within the area object
	private static int numberOfAreaObjects = 0; //for unique area id
	public bool isWinZone = false;
    public GameObject arena;
    public int area_id;

	// Use this for initialization
	void Start () {
		numberOfAreaObjects++; //count up id
        arena = GameObject.Find("Arena");
		//INITIALIZE LIST
		areaInteractives = new List<GameObject>();
	}

	private void OnTriggerEnter(Collider other){ //something entered my area trigger...
		//FILL ARRAY/LIST FOR INTERACTIVES ON STARTUP
		if(other.gameObject.CompareTag("Interactive")){ //is it an interactive?
			if (!this.areaInteractives.Contains(other.gameObject)) { //if not already inside list:
				this.areaInteractives.Add (other.gameObject); //add!
			}
		}

        if (other.gameObject.tag == "Shrine")
        {
            if (other.GetComponent<Shrine>().shrine_id == 1)
            {
                isWinZone = true;
                if (area_id == 1)
                {
                    arena.GetComponent<SpawnController>().player_pos = new Vector3(-57.8f, 2.0f, 58.0f); // Top Right
                }
                if (area_id == 2)
                {
                    arena.GetComponent<SpawnController>().player_pos = new Vector3(73.4f, 2.0f, -43.1f); // Bottom Right
                }
                if (area_id == 3)
                {
                    arena.GetComponent<SpawnController>().player_pos = new Vector3(16.4f, 2.0f, -62.6f); // Bottom
                }
                if (area_id == 4)
                {
                    arena.GetComponent<SpawnController>().player_pos = new Vector3(8.6f, 2.0f, 21.2f); //Top
                }
                if (area_id == 5)
                {
                    arena.GetComponent<SpawnController>().player_pos = new Vector3(-53.7f, 2.0f, 39.2f); //Top Left
                }
                if (area_id == 6)
                {
                    arena.GetComponent<SpawnController>().player_pos = new Vector3(-32.2f, 2.0f, -6.0f);// Bottom Left
                }
            }
        }
    } //end onTriggerEnter

	public void setWinZone(){ //for Player spawn and end goal
		isWinZone = true;
		this.GetComponent<Renderer> ().material.color = Color.green;
		this.GetComponent<Renderer> ().material.color = new Color(0, 1, 0, 0.5f);
	}

	//getter for areaCollectibles (LIST)
	public List<GameObject> getAreaInteractives(){ 
		return areaInteractives;
	}

	//getter for areaCollectables (ARRAY)
	public GameObject[] getAreaInteractivesArray(){
		return areaInteractives.ToArray ();
	}
}
