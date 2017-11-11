using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpawnController : MonoBehaviour {
    //PUBLIC CONSTS & VARIABLES
	public const int numberOfCollectibles = 3; //constant for number of objects to be searched
    public int[] zoneLayerNumbers; //array, numbers of the layers with the zones --> without win zone

    //PRIVATE VARIABLES
    private GameObject[] interactives; //all interactive GameObjects in the scene


	// Use this for initialization
	void Start () {
        //SET VARIABLES

        // ((InteractiveSettings) interactives[0]);
        SpawnShrines();
        SpawnInteractives();
        DetermineCollectibles();
        //@TODO: AREALE AUFTEILEN
        //idee: startzone machen, wenn überlappt -> dort nicht spawnen
        //und gleichzeitig -> startzone: wenn Spieler darein läuft und 
        //alle Objekte hat: WIN
        //oder einfach: mindestabstand zwischen den Objekten angeben! (relativ zur Arenagröße)


        //@TODO: IN AREAL
        //alle Objekte mit Tag 'Interactive' suchen
        //von denen jeweils eines ... .iscollectible = true setzen

        ////LAYER MACHEN!! -> Für jede Region einen Layer!! -> und für die winzone! --> dann nur auf dem Layer jeweils eines als Collectible setzen!

        //ODER: Random Objekte droppen? dürfte aber zu offensichtlich sein :/ 
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	void DetermineCollectibles(){
        // @TODO: setze [KONSTANTE ERSTELLEN] InteractiveObjects als Collectibles
        //Je Zone: 1 Object AUSSER in winzone
        interactives = GameObject.FindGameObjectsWithTag("Interactive"); //get all Interactives
        print("Number of Interactives: " + interactives.Length); //for testing
        //(interactives[GetRandomNumber(0, interactives.Length)]).GetComponent<InteractiveSettings>().SetCollectible(); //set as collectible


        ////GET RANDOM GAME OBJECTS FROM THE INTERACTIVES ARRAY   V1
        //var rnd = new System.Random();
        //var leftToPick = numberOfCollectibles;
        //var itemsLeft = numberOfCollectibles;
        //var chosenCollectibles = new List<GameObject>(); //create empty list for the chosen objects
        //var arrayPickIndex = 0;

        //while (leftToPick > 0)
        //{
        //    if (rnd.Next(0, itemsLeft) < leftToPick)
        //    {
        //        chosenCollectibles.Add(interactives[arrayPickIndex]);
        //        leftToPick--;
        //    }
        //    arrayPickIndex++;
        //    itemsLeft--;
        //}


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


        GameObject[] collectibles = chosenCollectibles.ToArray(); //to array cuz what r lists
        print("Number of collectibles: " +collectibles.Length);
        for (int i = 0; i < collectibles.Length; i++)
        {
            collectibles[i].GetComponent<InteractiveSettings>().SetCollectible(); //set every item in the list as collectible
        }



        //for (int zone = 0; zone <= numberOfCollectibles; zone++) //iterate through zones
        //{
        //    //GameObject[]("zone" + (zone + 1)) = new GameObject[10];
        //    List<GameObject> objects = new List<GameObject> (); //create empty list for objects in zone
        //    for (int go = 0; go < interactives.Length; go++) //iterate through all interactives 
        //    {
        //        if ((interactives[go].layer) == zoneLayerNumbers[zone]) //if GO is on the corresponding zone layer...
        //        {
        //            objects.Add(interactives[go]); //add to list!
        //        }
        //    } //end interactives iteration

        //    //objectsInZone.ToArray();
        //    GameObject[] objectsInZone = objects.ToArray(); //convert into array cuz im a noob who cant work with lists
        //    print("Game Objects in Zone " +(zone+1) +": " +objectsInZone.Length); //testing
        //    (objectsInZone[GetRandomNumber(0, objectsInZone.Length)]).GetComponent<InteractiveSettings>().SetCollectible(); //set as collectible

        //    //objectsInZone[GetRandomNumber(0, objectsInZone.)];

        //} //end zone iteration


        //für jede Zone

    }

    //private int GetRandomNumber(int minimum, int maximum)
    //{
    //    int random = 0;
    //    Random r = new Random();
    //    //random = r.Next(0, maximum);

    //    return random;
    //}

    private void SpawnInteractives()
    {
        //Verteile Interactives auf den jeweiligen Layern
        //evtl erst hier die Layer erstellen? Anzahl Layer == Anzahl Collectibles! + 1 Winzone in der Mitte?
        //Keep in mind the Zones! (Zone 1-4, Winzone)
    }

    private void SpawnShrines()
    {
        
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



