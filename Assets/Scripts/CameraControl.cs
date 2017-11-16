using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    private GameObject player;
    private Camera currentCamera;
    private GameObject dummy;
    private Vector3 playerScreenPos;
    //These two values control at which distance the camera movement will trigger
    public float thresholdWidth = 50;
    public float thresholdHeight = 50;
    public float smoothTime = 0.2f;
    //This value will added to the position of the player, in the direction he is currently looking
    public float additionToPosition = 5f;

    private Vector3 cameraVelocity = Vector3.zero;
    private Vector3 moveToPosition;

    private bool movementTriggered = false;


    // Use this for initialization
    void Start() {
        //Find the Objects.
        player = GameObject.Find("Player");
        dummy = GameObject.Find("CameraDummy");
        currentCamera = Camera.main;
        //Convert screen dependent values, to fitting values for the current game screen.
        thresholdWidth = dnd.ScreenSizeCompensation(thresholdWidth);
        thresholdHeight = dnd.ScreenSizeCompensation(thresholdHeight); 
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
                //Move to the player position, and i bit more in the direction he is facing. This is solved over cos and sin. They need radients of the angel. 
                moveToPosition = new Vector3(player.transform.position.x + (additionToPosition * Mathf.Sin(Mathf.Deg2Rad * player.transform.eulerAngles.y)), player.transform.position.y, player.transform.position.z + (additionToPosition * Mathf.Cos(Mathf.Deg2Rad * player.transform.eulerAngles.y)));
                movementTriggered = true;
            }
            
            if (movementTriggered)
            {
                //SmoothDamp is a function for smooth camera movement.
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
}
