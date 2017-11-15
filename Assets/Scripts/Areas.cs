using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Areas : MonoBehaviour {
	private List<GameObject> areaInteractives; //all interactives within the area object
	private static int numberOfAreaObjects = 0; //for unique area id
	public bool isWinZone = false;

	// Use this for initialization
	void Start () {
		//FOR UNIQUE NAME	
		numberOfAreaObjects++; //count up id

		//INITIALIZE LIST
		areaInteractives = new List<GameObject>();
	}

	private void OnTriggerStay(Collider other){ //something entered my area trigger...
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
