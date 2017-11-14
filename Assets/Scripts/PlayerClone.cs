using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClone : MonoBehaviour {

    private GameObject player;

    public float rotation;
    public float offset = 2;
    public float speed = 10;
    public float expandTime = 2;
    private float expandTimer = 0;

    private GameObject god;
    // Use this for initialization

    private GameObject[] Clones;
    
	void Start ()
    {
        player = GameObject.Find("Player");
        this.transform.position = (player.transform.position + new Vector3(offset * Mathf.Sin(Mathf.Deg2Rad * rotation), player.transform.position.y, offset * Mathf.Cos(Mathf.Deg2Rad * rotation)));
        god = GameObject.Find("TheosGott");
    }

    // Update is called once per frame
    void LateUpdate () {

        //position update

        if (expandTimer < expandTime)
        {
            this.transform.position = Vector3.Lerp(transform.position, (player.transform.position + new Vector3(offset * Mathf.Sin(Mathf.Deg2Rad * rotation), 0, offset * Mathf.Cos(Mathf.Deg2Rad * rotation))), expandTimer/expandTime);
            expandTimer += Time.deltaTime;
        }
        else
        {
            this.transform.position = (player.transform.position + new Vector3(offset * Mathf.Sin(Mathf.Deg2Rad * rotation), 0, offset * Mathf.Cos(Mathf.Deg2Rad * rotation)));
            Debug.Log("Velocity: " + (player.transform.position + (new Vector3(offset * Mathf.Sin(Mathf.Deg2Rad * rotation), 0, offset * Mathf.Cos(Mathf.Deg2Rad * rotation))) - new Vector3(this.transform.position.x, 0, this.transform.position.z)));     
        }
        Clones = GameObject.FindGameObjectsWithTag("Clone");
        for (int i = 0; i < Clones.Length; i++)
        {
            if( Clones[i] == this.gameObject)
            {
                rotation = (360.0f / Clones.Length) * i ;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        god.GetComponent<dnd>().ReleaseObject(); // Makes the god drop the object he's holding
        Destroy(this, 0.2f);
    }
}
    