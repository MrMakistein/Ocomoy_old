using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForClones : MonoBehaviour {
    private GameObject[] clones;
    public GameObject ClonedObject;
    public bool cloned = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(cloned == true)
        {

            clones = GameObject.FindGameObjectsWithTag("Clone");
            foreach  (GameObject o in clones)
            {

            }
        }
	}
}
