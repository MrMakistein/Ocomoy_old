using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class InteractiveSettings : MonoBehaviour {
public class InteractiveSettings : MonoBehaviour
{
    public Material normalMaterial;
    public Material collectibleMaterial;
    public float collectible_weight = 10;
    public bool isCollectible = false;
    public bool collectibleOnFloor = true;



    // Use this for initialization
    void Start()
    {
        //isCollectible = false; //default: not a collectible; will be changed by SpawnController	
        //isCollectible = false;

        GetComponent<Rigidbody>().isKinematic = false; //disable kinematics -> can be grabbed
        GetComponent<Collider>().isTrigger = false;
    }

    //entweder: ausgehen von boolean isCollectible
    //oder: extra Methode ausführen von SpawnController aus! Der spawnt die Collectibles und ändert die Settings!
    public void SetCollectible()
    {


        isCollectible = true; //mark as collectible
                              ////GetComponent<Collider> ().isTrigger = true; //set as trigger => you can enter it to deactivate!

        //Update the mass of the collectible
        this.gameObject.GetComponent<ThrowObject>().InitialMass = collectible_weight;

        //for testing
        //GetComponent<Renderer>().material = collectibleMaterial;
        //GetComponent<Rigidbody> ().isKinematic = true; //don't fall through floor --> PROBLEM: can't pick up

    }
}
