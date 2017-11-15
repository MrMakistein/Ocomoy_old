using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClone : MonoBehaviour {

    private GameObject player;

    public float rotation;
    public float rotation_speed = 10;
    public float offset = 2;
    public float speed = 10;
    public float expandTime = 2;
    private float expandTimer = 0;
    private float cloneTime;
    private float cloneTimer = 0;
    private int cloneAmount = 0;

    private GameObject god;
    // Use this for initialization

    private GameObject[] Clones;
    
	void Start ()
    {
        player = GameObject.Find("Player");
        this.transform.position = (player.transform.position + new Vector3(offset * Mathf.Sin(Mathf.Deg2Rad * rotation), player.transform.position.y, offset * Mathf.Cos(Mathf.Deg2Rad * rotation)));
        god = GameObject.Find("TheosGott");
        cloneTime = player.GetComponent<Player>().clone_time;
    }

    // Update is called once per frame
    void LateUpdate () {

        if (cloneTimer > cloneTime)
        {
            Destroy(this.gameObject);
        }
        else
        {
            cloneTimer += Time.deltaTime;
        }

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

        this.transform.rotation = player.transform.rotation;

        Clones = GameObject.FindGameObjectsWithTag("Clone");
        if(cloneAmount != Clones.Length)
        {
            for (int i = 0; i < Clones.Length; i++)
            {
                if (Clones[i] == this.gameObject)
                {
                    rotation = (360.0f / Clones.Length) * i;
                }
            }
            cloneAmount = Clones.Length;
        }
         rotation += rotation_speed * Time.deltaTime;
    }
    
}
    