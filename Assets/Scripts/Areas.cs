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
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//save all interactives within the zone into an array
	private void determineAreaCollectibles() {
		GameObject[] allInteractives = GameObject.FindGameObjectsWithTag("Interactive");
		for (int i = 0; i < allInteractives.Length; i++) {
			if (OnTriggerEnter ((allInteractives [i]).GetComponent<Collider>)) {

			}
		}
	}
		
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
