using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    private GameObject player;
    private Camera currentCamera;
    private GameObject dummy;
    private Vector3 playerScreenPos;
<<<<<<< HEAD
    //These two values control at which distance the camera movement will trigger
    public float thresholdWidth = 50;
    public float thresholdHeight = 50;
    public float smoothTime = 0.2f;
    //This value will added to the position of the player, in the direction he is currently looking
    public float additionToPosition = 5f;

    private Vector3 cameraVelocity = Vector3.zero;
    private Vector3 moveToPosition;

=======
    public float thresholdWidth = 50;
    public float thresholdHeight = 50;
    public float smoothTime = 0.2f;
    //These two values are used as there are some gameplay relevant functions that depend on the screen resolution
    private float editorScreenMean = (1053 + 459) / 2;
    private float playScreenMean;
    private Vector3 cameraVelocity = Vector3.zero;
    private Vector3 moveToPosition;
>>>>>>> Maki
    private bool movementTriggered = false;


    // Use this for initialization
    void Start() {
<<<<<<< HEAD
        //Find the Objects.
        player = GameObject.Find("Player");
        dummy = GameObject.Find("CameraDummy");
        currentCamera = Camera.main;
        //Convert screen dependent values, to fitting values for the current game screen.
        thresholdWidth = PersonalMath.ScreenSizeCompensation(thresholdWidth);
        thresholdHeight = PersonalMath.ScreenSizeCompensation(thresholdHeight); 
=======
        player = GameObject.Find("Player");
        dummy = GameObject.Find("CameraDummy");
        currentCamera = Camera.main;
        playScreenMean = (Screen.width + Screen.height) / 2;
        thresholdWidth = (thresholdWidth / editorScreenMean) * playScreenMean;
        thresholdHeight = (thresholdHeight / editorScreenMean) * playScreenMean;

>>>>>>> Maki
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
<<<<<<< HEAD
                //Move to the player position, and i bit more in the direction he is facing. This is solved over cos and sin. They need radients of the angel. 
                moveToPosition = new Vector3(player.transform.position.x + (additionToPosition * Mathf.Sin(Mathf.Deg2Rad * player.transform.eulerAngles.y)), player.transform.position.y, player.transform.position.z + (additionToPosition * Mathf.Cos(Mathf.Deg2Rad * player.transform.eulerAngles.y)));
                movementTriggered = true;
            }
            
            if (movementTriggered)
            {
                //SmoothDamp is a function for smooth camera movement.
=======
                moveToPosition = player.transform.position;
                movementTriggered = true;
            }

            if (movementTriggered)
            {
>>>>>>> Maki
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
<<<<<<< HEAD
=======

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
>>>>>>> Maki
}
