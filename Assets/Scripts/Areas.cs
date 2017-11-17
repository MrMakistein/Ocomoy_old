using System.Collections;
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
                    print("1");
                    arena.GetComponent<SpawnController>().player_pos = new Vector3(-32.2f, 2.0f, -6.0f);
                }
                if (area_id == 2)
                {
                    print("2");
                    arena.GetComponent<SpawnController>().player_pos = new Vector3(-32.2f, 2.0f, -6.0f);
                }
                if (area_id == 3)
                {
                    print("3");
                    arena.GetComponent<SpawnController>().player_pos = new Vector3(-32.2f, 2.0f, -6.0f);
                }
                if (area_id == 4)
                {
                    print("4");
                    arena.GetComponent<SpawnController>().player_pos = new Vector3(-32.2f, 2.0f, -6.0f);
                }
                if (area_id == 5)
                {
                    print("5");
                    arena.GetComponent<SpawnController>().player_pos = new Vector3(-32.2f, 2.0f, -6.0f);
                }
                if (area_id == 6)
                {
                    print("6");
                    arena.GetComponent<SpawnController>().player_pos = new Vector3(-32.2f, 2.0f, -6.0f);
                }
                //arena.GetComponent<SpawnController>().player_pos = transform;



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
