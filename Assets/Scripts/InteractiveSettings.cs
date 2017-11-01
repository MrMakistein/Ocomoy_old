using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class InteractiveSettings : MonoBehaviour {
public class InteractiveSettings : MonoBehaviour 
{
    public Material normalMaterial;
	public Material collectibleMaterial;

	private bool isCollectible = false;


	// Use this for initialization
	void Start () {
        //isCollectible = false; //default: not a collectible; will be changed by SpawnController	
        //isCollectible = false;

        if (!isCollectible)
        {
            GetComponent<Renderer>().material = normalMaterial;
            GetComponent<Collider>().isTrigger = false;
        }





            //	if (isCollectible) {
            //			//renderer.material.mainTexture = collectibleMaterial;
            //			GetComponent<Renderer> ().material = collectibleMaterial;
            //            GetComponent<Collider>().isTrigger = true;
            //        } else {
            ////			renderer.material.mainTexture = normalMaterial;
            //			GetComponent<Renderer> ().material = normalMaterial;
            //            GetComponent<Collider> ().isTrigger = false;
            //		}		//SpawnController.determineCollectibles (); //geht nicht, warum? --> wann und in welcher reihenfolge wird das ausgeführt? wie kann ich garantieren, dass das nicht nach materialzuweisung passiert?

        }

    // Update is called once per frame
    void Update () {
			
	}

    //entweder: ausgehen von boolean isCollectible
    //oder: extra Methode ausführen von SpawnController aus! Der spawnt die Collectibles und ändert die Settings!
    public void SetCollectible()
    {
        isCollectible = true; //mark as collectible
        GetComponent<Collider>().isTrigger = true; //set as trigger => you can enter it to deactivate!
        //for testing
        GetComponent<Renderer>().material = collectibleMaterial;
        
    }
}
