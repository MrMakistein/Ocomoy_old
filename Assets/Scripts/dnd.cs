﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class dnd : MonoBehaviour
{
    //mask used for raycast that the raycast only hits the ground
    int mask;
    //prevents picking up another object after a object was dropped without releasing the mouse button
    bool buttonReleased = true;


    //the maximum distance at which a object can be picked up (needed by the raycast function)
    public float catchingDistance = 100;

    //at which height the object is beeing held (is later influneced by weight class (to compensate for the height of the object))
    public float pickUpHeight = 2;
    public static Camera currentCamera;

    //custom weight multiplier for the four weight classes
    public float InfluenceWeightClass1 = 1f;
    public float InfluenceWeightClass2 = 1f;
    public float InfluenceWeightClass3 = 1f;
    public float InfluenceWeightClass4 = 1f;
    private float currentWeightInfluence;

    //the force at which the pbject is beeing dragged to the cursor
    public float forceStrenght = 400f;


    //dragging a object?
    bool isDragging = false;


    public static float DropDistance;
    public float initialDropDistance = 1200f;
    public float DorpDistanceMultiplierFor1 = 2f;
    public float DorpDistanceMultiplierFor2 = 2f;
    public float DorpDistanceMultiplierFor3 = 2f;
    public float DorpDistanceMultiplierFor4 = 2f;

    //modifier for the pick up height, based on weight classes (expected that heavier objects are larger...)
    float heightOffset;
    public float HeightOffsetFor1 = -1f;
    public float HeightOffsetFor2 = -0.5f;
    public float HeightOffsetFor3 = 0.2f;
    public float HeightOffsetFor4 = 1f;

    //this is used to compensate a calculation mistake, where i could not find out, where it happens, sould be added to pickupheight, when not calculating the mouse vector
    public float heightCompensation = 1.1f;

    public float heightThreshhold = 0.1f;

    //should the height be locked on pickup
    public bool heightLock = false;

    //The point of the ray hit
    Vector3 RayHitPoint;

    //needed to update the draggingobject when moving the camera
    private Vector3 oldCameraPosition;
    private Vector3 cameraDifference;

    //__________________________ DIFFERENT MECHANICS

    //activate the click and fling mechanic
    public bool clickNFling = true;
    //is a object currently being helt in place to fling
    bool charging = false;
    public float shotStrength = 10000f;




    //lock the picked up object, so one can only fling it
    public bool lockObject = true;



    //if the object woobles(uses forces) or uses lerp
    public bool wobbleDrag = false;

    //How fast the object is being picked up (if wobble drag is not active
    public float pickUpSpeed = 10;



    //position of the object in screen cordinates
    private Vector3 pickUpScreenPos;
    //the picked up object
    public static GameObject draggingObject;
    // the rigidbody of the picked up object
    Rigidbody DrObj;
    //position of the mouse in world space
    Vector3 MouseVector;

    //true if you want to stop the dragging process
    bool stopDragging = false;

    //for math
    //these values were the window res the first time the game was tested
    private static float editorScreenMean = (1053 + 459) / 2;
    private static float playScreenMean;

    //value from 0 to 1 which indicates the distance from the mouse cursor to the object relativ to the drop distance
    float screenDistanceRel;

    //used with values that use something screen related;
    private float screenSizeMultiplier = 1;

    long frames = 0;
    float time = 0;
    //singelton
    public static dnd instance;

    void Awake()
    {
        instance = this;

        //uses the mean of the screen for scaling
        playScreenMean = (Screen.width + Screen.height) / 2;
        currentCamera = Camera.main;
        oldCameraPosition = currentCamera.transform.position;
        //the way to tell the mask that the raycast should only hit the ground
        mask = 1 << LayerMask.NameToLayer("is Ground");

        //Every screen dependent variable has to be scaled to fit any resolution
        initialDropDistance = ScreenSizeCompensation(initialDropDistance);
        screenSizeMultiplier = ScreenSizeCompensation(screenSizeMultiplier);
        

    }

    // Update is called once per frame
    void Update()
    {
        //FPS
        if(time > 1)
        {
            time = 0;

            Debug.Log("FPS: " + frames);
            frames = 0;
        } else
        {
            frames++;
            time += Time.deltaTime;
        }

        //main if, if true --> object is beeing picked up.
        //
        //handles everything while picking up, while holding ánd after release 
        if (buttonReleased && Input.GetMouseButton(0) && (!isDragging || Vector3.Distance(pickUpScreenPos, Input.mousePosition) <= DropDistance) && !stopDragging)
        {
            //Check if the player is using a slow effect.
            //TODO: more performent methode for this effect.
            slowEffectCheckAndApply();

            //start dragging
            //this is called as soon as the player clicks
            if (!isDragging)
            {
                //get the object the player has clicked on (if it is a valid object, otherwise return will be null)
                draggingObject = GetObjectFromMouseRaycast();
                //if it is a valid object, initiate dragging
                if (draggingObject)
                {
                    isDragging = true;
                    pickUpScreenPos = currentCamera.WorldToScreenPoint(draggingObject.GetComponent<Rigidbody>().position);

                    //used for lockObject = true    
                    //calculate the "mouse position" in the world, with the height pickUpHeight + heightOffset
                    MouseVector = CalculateMouse3DVector(currentCamera, mask, (pickUpHeight + heightOffset));
                }
            }

            

            //while dragging
            else if (draggingObject != null)
            {
                //to shorten code
                DrObj = draggingObject.GetComponent<Rigidbody>();



                //with clickNFling
                if (clickNFling && (DrObj.tag != "GodObject" || !wobbleDrag))
                {

                    if (Input.GetMouseButtonDown(1))
                    {
                        DrObj.velocity = Vector3.zero;
                        DrObj.constraints = RigidbodyConstraints.FreezeAll;
                        charging = true;

                    }

                    if (Input.GetMouseButtonUp(1))
                    {
                        stopDragging = true;

                    }
                }
                

                pickUpScreenPos = currentCamera.WorldToScreenPoint(DrObj.position);


                //if the object isnt locked, update the position
                if (!lockObject)
                {
                    //calculate the "mouse position" in the world, with the height pickUpHeight + heightOffset
                    MouseVector = CalculateMouse3DVector(currentCamera, mask, pickUpHeight + heightOffset);
                }

                //calculate the relative distance using screen coordinates
                screenDistanceRel = (Vector3.Distance(pickUpScreenPos, Input.mousePosition) / DropDistance);

                //Apply force and change drag
                if (wobbleDrag)
                {
                    if (heightLock)
                    {

                        DrObj.constraints = RigidbodyConstraints.FreezePositionY;
                        //uses lerp to get the object fast to the pick up height, but then uses the standard wobble drag at pick up height
                        if (pickUpHeight + heightOffset + heightCompensation > DrObj.transform.position.y + heightThreshhold || pickUpHeight + heightOffset + heightCompensation > DrObj.transform.position.y + heightThreshhold)
                        {
                            DrObj.transform.position = new Vector3(DrObj.transform.position.x, Vector3.Lerp(DrObj.transform.position, MouseVector, Time.deltaTime * pickUpSpeed).y, DrObj.transform.position.z);
                        }
                        DrObj.AddForce((Time.deltaTime * 140) * (MouseVector - DrObj.transform.position).normalized * forceStrenght / (screenSizeMultiplier), ForceMode.Force);
                        DrObj.drag = (currentWeightInfluence * 1) / (Vector3.Distance(DrObj.transform.position, MouseVector));
                    }
                    else
                    {
                        //screenSizeMultipier, because mouseVector uses pixel coordinates 
                        //delta time to apply the same amount of force for each frame rate
                        DrObj.AddForce((Time.deltaTime * 140 )* (MouseVector - DrObj.transform.position).normalized * forceStrenght / (screenSizeMultiplier), ForceMode.Force);
                        DrObj.drag = (currentWeightInfluence * 1) / (Vector3.Distance(DrObj.transform.position, MouseVector));
                    }

                }
                else
                {
                    DrObj.constraints = RigidbodyConstraints.FreezeAll;
                    DrObj.transform.position = Vector3.Lerp(DrObj.transform.position, MouseVector, Time.deltaTime * pickUpSpeed);
                }

                // if the object isnt locked, move it with the camera
                if (!lockObject)
                {
                    //Update Position for camera movement
                    cameraDifference = currentCamera.transform.position - oldCameraPosition;
                    DrObj.transform.position += cameraDifference;
                }


            }
        }

        //stop dragging
        else
        {
            if (draggingObject != null)
            {
                //reset everything
                DrObj = draggingObject.GetComponent<Rigidbody>();
                DrObj.constraints = RigidbodyConstraints.None;

                //if charged, add force
                if (charging && stopDragging)
                {

                    //used for lockObject = true
                    //calculate the "mouse position" in the world, with the height pickUpHeight + heightOffset
                    MouseVector = CalculateMouse3DVector(currentCamera, mask, pickUpHeight + heightOffset);

                    //Clip Value for Distance Multiplier from 0 - 1

                    float distanceMultiplier = screenDistanceRel < 1 ? screenDistanceRel : 1;
                    float strength = distanceMultiplier * shotStrength;
                    //new vector prefents that the object flies into space...
                    DrObj.AddForce((new Vector3(MouseVector.x, DrObj.transform.position.y, MouseVector.z) - DrObj.transform.position).normalized * strength, ForceMode.Force);

                }
                DrObj.drag = 0;
                draggingObject = null;

            }
            isDragging = false;
            stopDragging = false;
        }

        //Check if the mouse button was released
        if (Input.GetMouseButton(0) && !isDragging)
        {
            buttonReleased = false;
        }
        else
        {
            buttonReleased = true;
        }

        //Camera Update
        oldCameraPosition = currentCamera.transform.position;

        //Update Cursor Position on canvas

    }

    public void ReleaseObject()
    {
        buttonReleased = false;
    }

    private GameObject GetObjectFromMouseRaycast()
    {
        GameObject gmObj = null;
        RaycastHit hitInfo = new RaycastHit();
        //send a ray from the mouse "into" the game
        bool hit = Physics.Raycast(currentCamera.ScreenPointToRay(Input.mousePosition), out hitInfo);
        //if something was hit
        if (hit)
        {
            //now check if the object that was hit is an object that we want to pick up
            if (hitInfo.collider.gameObject.GetComponent<Rigidbody>() &&
                Vector3.Distance(hitInfo.collider.gameObject.transform.position, currentCamera.transform.position) <= catchingDistance &&
                (hitInfo.collider.gameObject.tag == "Interactive" || hitInfo.collider.gameObject.tag == "GodObject"))
            {
                gmObj = hitInfo.collider.gameObject;
            }
        }

        //if it was a correct object, assign the correections for the different weight classes
        if (gmObj != null)
        {
            switch (gmObj.GetComponent<ThrowObject>().weight_class)
            {
                case 1:
                    currentWeightInfluence = InfluenceWeightClass1;
                    DropDistance = initialDropDistance * DorpDistanceMultiplierFor1;
                    heightOffset = HeightOffsetFor1;
                    break;
                case 2:
                    currentWeightInfluence = InfluenceWeightClass2;
                    DropDistance = initialDropDistance * DorpDistanceMultiplierFor2;
                    heightOffset = HeightOffsetFor2;
                    break;
                case 3:
                    currentWeightInfluence = InfluenceWeightClass3;
                    DropDistance = initialDropDistance * DorpDistanceMultiplierFor3;
                    heightOffset = HeightOffsetFor3;
                    break;
                case 4:
                    currentWeightInfluence = InfluenceWeightClass4;
                    DropDistance = initialDropDistance * DorpDistanceMultiplierFor4;
                    heightOffset = HeightOffsetFor4;
                    break;
                default:

                    break;
            }
            //and check again if the player is currently using a slow effect
            slowEffectCheckAndApply();
        }
        return gmObj;
    }

    //Heavy/Slow Effect
    private void slowEffectCheckAndApply()
    {
        if (Player.slowEffect == true)
        {
            currentWeightInfluence = InfluenceWeightClass4;
            DropDistance = initialDropDistance * DorpDistanceMultiplierFor4;
            if (draggingObject != null)
            {
                draggingObject.GetComponent<Rigidbody>().mass = Player.slow_mass;
            }
        }
    }



    //Math
    //as some variables are screen size dependent, compensate it with this function
    public static float ScreenSizeCompensation(float toCompensate)
    {
        return (toCompensate / editorScreenMean) * playScreenMean;
    }

    //Calculates the point according to the mouse courser in a specified height(using pickUpHeight)
    public Vector3 CalculateMouse3DVector(Camera currentCamera, LayerMask mask, float temppickUpHeight)
    {
        Vector3 v3 = Input.mousePosition;
        v3 = currentCamera.ScreenToWorldPoint(v3);
        //raycast to determine the distance from the camera to an object and the angle of the hit
        RaycastHit hitInfo = new RaycastHit();
        Ray r = currentCamera.ScreenPointToRay(Input.mousePosition);

        //!!! A distance (3rd Position) is needed, because otherwise Unity behaves buggy !!!
        bool hit = Physics.Raycast(r, out hitInfo, 2000, mask);
        if (hit)
        {
            //point where ray hits the surface
            RayHitPoint = hitInfo.point;
            Vector3 CamPos = currentCamera.gameObject.transform.position;
            float originalDistance = Vector3.Distance(RayHitPoint, CamPos);

            //Use trigonometry to calculate the point on the ray, where the height is temppickUpHeight
            float cosine = Vector3.Dot(r.direction, hitInfo.normal);
            float cosineDegrees = Mathf.Acos(cosine);
            float resutlingDistance = (temppickUpHeight) / Mathf.Cos(Mathf.PI - cosineDegrees);

            v3 = Vector3.Lerp(RayHitPoint, CamPos, resutlingDistance / originalDistance);
        }
        return v3;
    }


}
