using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Areas : MonoBehaviour {
	//private GameObject[] areaInteractives;
	private List<GameObject> areaInteractives;
	private static int numberOfAreaObjects = 0; //for unique area id
	public string name;
	public bool isWinZone = false;

	// Use this for initialization
	void Start () {
		name = "area" + numberOfAreaObjects.ToString; //unique name
		numberOfAreaObjects++;
		determineAreaCollectibles ();
		SpawnController.DetermineCollectibles (); //TODO: geht wsl nicht
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//save all interactives within the zone into an array
	private void determineAreaCollectibles() {
		GameObject[] allInteractives = GameObject.FindGameObjectsWithTag("Interactive");
		for (int i = 0; i < allInteractives.Length; i++) {
			if (OnTriggerEnter ((allInteractives [i]).GetComponent<Collider>)) { ///TODO: bedingung so stellen dass es funktioniert!
				areaInteractives.Add(allInteractives.Length);

				///oder: in der interactive Settings klasse schauen für jedes: onTriggerEnter
				/// dann sich selber adden zu ei
			}
		}
	}

	//TODO: solution finden: wie schaffe ich ess, dass wenn ein collider in meinem trigger ist, true geliefert wird?
	public bool OnTriggerEnter(Collider other){
		return true;
	}

	public void setWinZone(){ //for Player spawn and end goal
		isWinZone = true;
	}

	//getter for areaCollectibles
	public GameObject[] getAreaInteractives(){ //TODO oder List returnen?
		return areaInteractives.ToArray;
	}
}
