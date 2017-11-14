using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class avalanche : MonoBehaviour {
    public float radius = 12;
    public float height = 10;
    public int count = 10;

    private GameObject spawnObject;
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            spawnObject = GameObject.FindGameObjectWithTag("Interactive");
            spawnObject.GetComponent<InteractiveSettings>().isCollectible = false;
            Instantiate(spawnObject, (transform.position + (Vector3.up * height)) + new Vector3(Random.Range(0, radius), Random.Range(0, radius), Random.Range(0, radius)), Quaternion.identity);
        }

    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }


}

