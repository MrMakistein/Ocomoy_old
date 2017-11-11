using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    private GameObject player;
    private Camera currentCamera;
    private GameObject dummy;
    private Vector3 playerScreenPos;
    public float thresholdWidth = 50;
    public float thresholdHeight = 50;
    public float smoothTime = 0.2f;
    //These two values are used as there are some gameplay relevant functions that depend on the screen resolution
    private float editorScreenMean = (1053 + 459) / 2;
    private float playScreenMean;
    private Vector3 cameraVelocity = Vector3.zero;
    private Vector3 moveToPosition;
    private bool movementTriggered = false;


    // Use this for initialization
    void Start() {
        player = GameObject.Find("Player");
        dummy = GameObject.Find("CameraDummy");
        currentCamera = Camera.main;
        playScreenMean = (Screen.width + Screen.height) / 2;
        thresholdWidth = (thresholdWidth / editorScreenMean) * playScreenMean;
        thresholdHeight = (thresholdHeight / editorScreenMean) * playScreenMean;

    }

    // Update is called once per frame
    void Update() {
        //check for nullpointer
        if (player != null && dummy != null)
        {
            //transform the position of the player to the camera screen
            playerScreenPos = currentCamera.WorldToScreenPoint(player.transform.position);

            //if the player is on the edge of the screen --> realign camera to center
            if (playerScreenPos.x < thresholdWidth || Screen.width - thresholdWidth < playerScreenPos.x || playerScreenPos.y < thresholdHeight || Screen.height - thresholdHeight < playerScreenPos.y)
            {
                moveToPosition = player.transform.position;
                movementTriggered = true;
            }

            if (movementTriggered)
            {
                dummy.transform.position = Vector3.SmoothDamp(dummy.transform.position, moveToPosition, ref cameraVelocity, smoothTime);
                if(Vector3.Distance(dummy.transform.position, moveToPosition) < 0.1)
                {
                    movementTriggered = false;
                }
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
