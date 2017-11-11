using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    private GameObject player;
    private Camera currentCamera;
    private GameObject dummy;
    private Vector3 playerScreenPos;
    public int threshholdWidth = 50;
    public int threshholdHeight = 50;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player");
        dummy = GameObject.Find("CameraDummy");
        currentCamera = Camera.main;
        }
	
	// Update is called once per frame
	void Update () {
        //check for nullpointer
        if(player != null && dummy != null)
        {
            //transform the position of the player to the camera screen
            playerScreenPos = currentCamera.WorldToScreenPoint(player.transform.position);
            Debug.Log("PlayerScreenPos: " + playerScreenPos);

            //if the player is on the edge of the screen --> realign camera to center
            if(playerScreenPos.x < threshholdWidth || Screen.width - threshholdWidth < playerScreenPos.x || playerScreenPos.y < threshholdHeight || Screen.height - threshholdHeight < playerScreenPos.y)
            {

            } 
            
        }
        else
        {
            Debug.LogError("Reference not found in CameraControl");
        }
	}

    // Switching behaviour
    //private void OnTriggerEnter(Collider Player)
    //{
    //    if (Player.tag == "Player")
    //    {
    //        Component temp = GetComponentInParent(typeof(Camera));
    //        if (temp.GetType() == typeof(Camera))
    //        {
    //            Debug.Log("Entered IF");
    //            Camera temp_cam = temp.GetComponent<Camera>();
    //            Debug.Log(temp);
    //            Debug.Log(dnd.currentCamera);
    //            if (dnd.currentCamera != null)
    //            {
    //                dnd.currentCamera.enabled = false;
    //                temp_cam.enabled = true;
    //                dnd.currentCamera = null;
    //            }

    //        }
    //    }
       
    //}
}
