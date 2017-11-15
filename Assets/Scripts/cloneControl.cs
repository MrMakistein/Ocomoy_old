using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloneControl : MonoBehaviour {


    public int CloneAmount = 4;
    public GameObject cloneObj;

    public static bool active = true;
	// Use this for initialization
	void Start () {
		if(cloneObj == null)
        {
            Debug.LogError("No clone specified in cloneControl!");
        }
	}


    
    // Update is called once per frame
    void Update () {
		if (active == true)
        {
            for (int i = 0; i < CloneAmount; i++)
            {
                GameObject currentSpawn = Instantiate(cloneObj, transform.position, Quaternion.identity);
                currentSpawn.transform.parent = gameObject.transform;               
            }
            active = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Interactive")
        {
            foreach (ContactPoint c in collision.contacts)
            {
                if(c.thisCollider.gameObject.tag == "Clone")
                {
                    Destroy(c.thisCollider.gameObject);             
                }                
            }
        }       
    }
}
