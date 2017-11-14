using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpawnController : MonoBehaviour {
    //PUBLIC CONSTS & VARIABLES
	public int numberOfCollectibles = 3; //constant for number of objects to be searched
    public int[] zoneLayerNumbers; //array, numbers of the layers with the zones --> without win zone

    //PRIVATE VARIABLES
    private GameObject[] interactives; //all interactive GameObjects in the scene
    public GameObject[] collectibles;
    public bool allCollected = false;

	// Use this for initialization
	void Start () {
        SpawnShrines();
        DetermineCollectibles();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	void DetermineCollectibles(){
        interactives = GameObject.FindGameObjectsWithTag("Interactive"); //get all Interactives
      

        ///GET RANDOM GAME OBJECTS FROM THE INTERACTIVES ARRAY    V2
        var rnd = new System.Random();
        var chosenCollectibles = new List<GameObject>(); //create empty list for the chosen objects
        int k = numberOfCollectibles;
        for (int i = 0; k > 0; ++i)
        {
            int r = rnd.Next(0, (interactives.Length)-i);
            if(r < k)
            {
                chosenCollectibles.Add(interactives[i]);
                k--;
            }
        }


        collectibles = chosenCollectibles.ToArray(); //to array cuz what r lists
        //print("Number of collectibles: " +collectibles.Length);
        for (int i = 0; i < collectibles.Length; i++)
        {
            collectibles[i].GetComponent<InteractiveSettings>().SetCollectible(); //set every item in the list as collectible
        }

        GameObject[] signposts = GameObject.FindGameObjectsWithTag("Signpost");
        foreach (GameObject go in signposts)
        {
            go.GetComponent<Signpost>().FindWinShrine();
        }


    }

    private void SpawnShrines()
    {
        // Assigns random id from 1 to 6 to all of the shrines (no repeating values)
        List<int> list = new List<int>(new int[] { 1, 2, 3, 4, 5, 6 });
        int rand = 0;
        GameObject[] shrines = GameObject.FindGameObjectsWithTag("Shrine");
        foreach (GameObject shrine in shrines)
        {
            rand = UnityEngine.Random.Range(0, list.Count);
            shrine.gameObject.GetComponent<Shrine>().shrine_id = list[rand];
            list.RemoveAt(rand);
        }





    }
}



