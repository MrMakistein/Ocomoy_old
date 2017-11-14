using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClone : MonoBehaviour {

    private GameObject player;

    public float rotation;
    public float offset = 2;
    public float speed = 10;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        this.transform.position = (player.transform.position + new Vector3(offset * Mathf.Sin(Mathf.Deg2Rad * rotation), player.transform.position.y, offset * Mathf.Cos(Mathf.Deg2Rad * rotation)));

        }

    // Update is called once per frame
    void LateUpdate () {
        //position update
        this.transform.position = (player.transform.position + new Vector3(offset * Mathf.Sin(Mathf.Deg2Rad * rotation), 0, offset * Mathf.Cos(Mathf.Deg2Rad * rotation)));
        Debug.Log("Velocity: " + (player.transform.position + (new Vector3(offset * Mathf.Sin(Mathf.Deg2Rad * rotation), 0, offset * Mathf.Cos(Mathf.Deg2Rad * rotation))) - new Vector3(this.transform.position.x, 0, this.transform.position.z)));
        this.transform.rotation = player.transform.rotation;
    }
}
    