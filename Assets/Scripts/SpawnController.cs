using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpawnController : MonoBehaviour {
    //PUBLIC CONSTS & VARIABLES
	public const int numberOfCollectibles = 3; //constant for number of objects to be searched
    public int[] zoneLayerNumbers; //array, numbers of the layers with the zones --> without win zone
	public static GameObject[] collectibles;
	public static List<GameObject> spawnAreaObjects;
	public GameObject[] areas; //areas in which collectibles spawn (1 per area)

    //PRIVATE VARIABLES
    private GameObject[] interactives; //all interactive GameObjects in the scene

	// Use this for initialization
	void Start () {
		DetermineAreas ();
		SpawnShrines(); //TODO: make Shrine Spawn dependant on WinZone? --> Player Spawn Point is within WinZone
		Invoke("DetermineCollectibles", 2); //ADJUST: je mehr Zonen/Collectibles desto mehr Zeit lassen bis Execution: Arrays in Areas müssen mit onTriggerEnter erst befüllt werden
		//DetermineCollectibles();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DetermineAreas(){
		spawnAreaObjects = new List<GameObject> (GameObject.FindGameObjectsWithTag("Area"));
		print ("Number of Areas: " + spawnAreaObjects.Count.ToString());

		//choose a random area as the winzone and exclude it from collectible spawn
		var rnd1 = new System.Random();
		int winZoneIndex = rnd1.Next (0, (spawnAreaObjects.Count) - 1);
		print ("random MAX for spawn areas: " + (spawnAreaObjects.Count - 1).ToString()); //TESTING
		GameObject winZone = spawnAreaObjects[winZoneIndex];
		winZone.GetComponent<Areas> ().setWinZone (); //set as winzone
		spawnAreaObjects.Remove (winZone); //exclude from SpawnaAreas

		print ("Number of Areas without Winzone: " + spawnAreaObjects.Count.ToString()); //TESTING
	}

	public void DetermineCollectibles() {
		List<GameObject> chosenCollectibles = new List<GameObject>(); //List for all collectibles

		//ITERATE OVER REMAINING ZONES
		for (int a = 0; a < spawnAreaObjects.Count; a++){
			
			GameObject zone = spawnAreaObjects[a];
			List<GameObject> interactivesInZone = zone.GetComponent<Areas>().getAreaInteractives();

			Debug.Log ("Number of Interactives in # "+zone.GetComponent<Areas>().name +" # : " +interactivesInZone.Count); //TESTING
		
			//choose one of the interactives in the zone randomly
			var rnd2 = new System.Random ();
		

			//CHOOSE A RANDOM INTERACTIVE IN THE ZONE TO SET AS COLLECTIBLE
			int r = rnd2.Next (0, interactivesInZone.Count - 1); 
			interactivesInZone[r].GetComponent<InteractiveSettings> ().SetCollectible ();

			//ADD IT TO THE COLLECTIBLE LIST
			chosenCollectibles.Add(interactivesInZone [r].GetComponent<GameObject>()); //add the game object to the collectibles List

			Destroy(zone); //then destroy the zone! (isn't needed anymore)
		}
			
        collectibles = chosenCollectibles.ToArray(); //to array cuz what r lists


    }

    private void SpawnShrines()
    {
        // Assigns random id from 1 to 6 to all of the shrines (no repeating values)
        List<int> list = new List<int>(new int[] { 1, 2, 3, 4, 5, 6 });
        int rand = 0;
        GameObject[] shrines = GameObject.FindGameObjectsWithTag("Shrine");
		foreach (GameObject shrine in shrines) {
			rand = UnityEngine.Random.Range (0, list.Count);
			shrine.gameObject.GetComponent<Shrine> ().shrine_id = list [rand];
			list.RemoveAt (rand);
		}
    }


} //END SpawnController



