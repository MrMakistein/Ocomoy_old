using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpawnController : MonoBehaviour {
    //PUBLIC CONSTS & VARIABLES
	public const int numberOfCollectibles = 3; //constant for number of objects to be searched
    public int[] zoneLayerNumbers; //array, numbers of the layers with the zones --> without win zone
	public GameObject[] collectibles;
	public GameObject[] areas; //areas in which collectibles spawn (1 per area)

    //PRIVATE VARIABLES
    private GameObject[] interactives; //all interactive GameObjects in the scene

	// Use this for initialization
	void Start () {
        SpawnShrines();
        //DetermineCollectibles();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DetermineCollectibles(){
        //interactives = GameObject.FindGameObjectsWithTag("Interactive"); //get all Interactives
		List<GameObject> areaObjects = GameObject.FindGameObjectsWithTag("Area");

		//choose a random area as the winzone and exclude it from collectible spawn
		var rnd1 = new System.Random();
		int winZoneIndex = rnd1.Next (0, (areaObjects.Count) - 1); 
		GameObject winZone = areaObjects[winZoneIndex];
		winZone.GetComponent<Areas> ().setWinZone ();
		areaObjects.Remove (winZone);

		//GameObject[] areas = GameObject.FindGameObjectsWithTag("Area"); //get areas
		List<GameObject> chosenCollectibles; //List for all collectibles
		//for (int i = 0; i < interactives.Length; i++) {
		//foreach(int a in areaObjects) {
		for (int a = 0; a < areaObjects.Count; a++){
			
			//TODO: get interactives in zone array from Zone itself!
			GameObject zone = areaObjects[a];
			List<GameObject> interactivesInZone = zone.GetComponent<Areas> ().getAreaInteractives;
		
			//choose one of the interactives in the zone randomly
			var rnd2 = new System.Random ();
			int n = numberOfCollectibles;
			for (int i = 0; n > 0; ++i) {
				int r = rnd2.Next (0, interactivesInZone.Count - i);
				if (r < n) {
					chosenCollectibles.Add (interactivesInZone [i]);
					n--;
				}
			} //end set random collectible 
				
			//then destroy the zone! (isn't needed anymore)
			zone.Destroy(); //TODO: check if it works
		}


        ///GET RANDOM GAME OBJECTS FROM THE INTERACTIVES ARRAY
        /*var rnd2 = new System.Random();
        var chosenCollectibles = new List<GameObject>(); //create empty list for the chosen objects
        int k = numberOfCollectibles;
        for (int i = 0; k > 0; ++i)
        {
            int r = rnd2.Next(0, (interactives.Length)-i);
            if(r < k)
            {
                chosenCollectibles.Add(interactives[i]);
                k--;
            }
        }*/
        collectibles = chosenCollectibles.ToArray(); //to array cuz what r lists
        //print("Number of collectibles: " +collectibles.Length);
        for (int i = 0; i < collectibles.Length; i++)
        {
            collectibles[i].GetComponent<InteractiveSettings>().SetCollectible(); //set every item in the list as collectible
        }

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



