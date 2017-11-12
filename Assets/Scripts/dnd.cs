using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class dnd : MonoBehaviour
{
    int mask;
    bool buttonReleased = true;
<<<<<<< HEAD
    public bool draggingDrag = true;
    public float catchingDistance = 100;
    public float pickUpHeight = 2;
=======
    public float pickUpThreshold = 0.6f;
    public float catchingDistance;
    public float pickUpHeight;
>>>>>>> Maki
    public static Camera currentCamera;

    //Reduction for the four weight classes

    public float InfluenceWeightClass1 = 1f;
    public float InfluenceWeightClass2 = 1f;
    public float InfluenceWeightClass3 = 1f;
    public float InfluenceWeightClass4 = 1f;
    private float currentWeightInfluence;
    public float forceStrenght = 120f;
    float pickUpSpeed = 10f;
    bool isDragging = false;
    Vector3 pickUpScreenPos;
<<<<<<< HEAD
    float DropDistance;
    public float initialDropDistance = 120f;
    public float DorpDistanceMultiplierFor1 = 2f;
    public float DorpDistanceMultiplierFor2 = 1f;
    public float DorpDistanceMultiplierFor3 = 0.6f;
    public float DorpDistanceMultiplierFor4 = 0.6f;
    float heightOffset;
    public float HeightOffsetFor1 = -1f;
    public float HeightOffsetFor2 = -0.5f;
    public float HeightOffsetFor3 = 0.2f;
    public float HeightOffsetFor4 = 1f;
    public float mouseVectorMultiplier = 10f;
    public float upwardVelocityThreshhold = 1f;
   

=======
    public float screenDropDistance = 80f;
    public float mouseVectorMultiplier = 10f;
    public float upwardVelocityThreshhold = 1f;
    //These two values are used as there are some gameplay relevant functions that depend on the screen resolution
    private float editorScreenMean = (1053 + 459) / 2;
    private float playScreenMean;

    
>>>>>>> Maki

    public static GameObject draggingObject;
    Rigidbody DrObj;
    Vector3 MouseVector;

    void Start()
    {
        currentCamera = Camera.main;
        mask = 1 << LayerMask.NameToLayer("is Ground");
<<<<<<< HEAD
        //Every screen dependent variable has to be scaled to fit any resolution
        initialDropDistance = PersonalMath.ScreenSizeCompensation(initialDropDistance);
        mouseVectorMultiplier = PersonalMath.ScreenSizeCompensation(mouseVectorMultiplier);
=======
        playScreenMean = (Screen.width+Screen.height)/ 2;
        screenDropDistance = (screenDropDistance / editorScreenMean) * playScreenMean;
        mouseVectorMultiplier = (mouseVectorMultiplier / editorScreenMean) * playScreenMean;
        //Debug.Log("screenDropDistance " + screenDropDistance);
        //Debug.Log("mouseVectorMultiplier " + mouseVectorMultiplier);
>>>>>>> Maki
    }
    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD

        if (buttonReleased && Input.GetMouseButton(0) && (!isDragging || Vector3.Distance(pickUpScreenPos, Input.mousePosition) <= DropDistance))
=======
       
        if (buttonReleased && Input.GetMouseButton(0) && (!isDragging || Vector3.Distance(pickUpScreenPos, Input.mousePosition) <= screenDropDistance))
>>>>>>> Maki
        {
            //start dragging
            if (!isDragging)
            {
                //initiate dragging
                draggingObject = GetObjectFromMouseRaycast();
                if (draggingObject)
                {
                    draggingObject.GetComponent<Rigidbody>().gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                    isDragging = true;
                    pickUpScreenPos = currentCamera.WorldToScreenPoint(draggingObject.GetComponent<Rigidbody>().position);
                }
            }

            //while dragging
            else if (draggingObject != null)
            {

                DrObj = draggingObject.GetComponent<Rigidbody>();

                Debug.Log("Height: " + DrObj.transform.position.y);
                pickUpScreenPos = currentCamera.WorldToScreenPoint(DrObj.position);
                MouseVector = PersonalMath.CalculateMouse3DVector(currentCamera, mask, pickUpHeight + heightOffset);
                //Apply force 
                if (draggingDrag)
                {
                    Debug.Log("WeightInfluence: " + currentWeightInfluence);
                    DrObj.AddForce((MouseVector - DrObj.transform.position).normalized * forceStrenght, ForceMode.Force);
                    DrObj.drag = (currentWeightInfluence * 1) / Vector3.Distance(DrObj.transform.position, MouseVector);
                }
                else
                {
                    DrObj.velocity = pickUpSpeed * ((MouseVector - DrObj.transform.position).normalized) * Vector3.Distance(MouseVector, DrObj.transform.position);
                }

            }
        }

        //stop dragging
        else
        {
            if (draggingObject != null)
            {
                DrObj = draggingObject.GetComponent<Rigidbody>();
                DrObj.gameObject.layer = LayerMask.NameToLayer("Default");
                DrObj.constraints = RigidbodyConstraints.None;
                DrObj.drag = 0;
<<<<<<< HEAD
                if (DrObj.velocity.y > upwardVelocityThreshhold && !draggingDrag)
=======
                if (DrObj.velocity.y > upwardVelocityThreshhold)
>>>>>>> Maki
                {
                    DrObj.velocity = new Vector3(mouseVectorMultiplier * Input.GetAxis("Mouse X"), 0, mouseVectorMultiplier * Input.GetAxis("Mouse Y"));
                }
                draggingObject = null;

            }
            isDragging = false;

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

    }

    private GameObject GetObjectFromMouseRaycast()
    {
        GameObject gmObj = null;
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(currentCamera.ScreenPointToRay(Input.mousePosition), out hitInfo);
        if (hit)
        {
            if (hitInfo.collider.gameObject.GetComponent<Rigidbody>() &&
                Vector3.Distance(hitInfo.collider.gameObject.transform.position, currentCamera.transform.position) <= catchingDistance &&
                !hitInfo.collider.gameObject.GetComponent<Rigidbody>().isKinematic)
            {
                gmObj = hitInfo.collider.gameObject;
            }
        }
        if (gmObj != null && gmObj.tag != "Interactive")
        {
            gmObj = null;
        }

        //Assign the correct weightclass
        if(gmObj != null)
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
                    Debug.LogError("Unknown WeightClass assigned");
                    break;
            }
        }
<<<<<<< HEAD
        return gmObj;
=======
        return v3;
>>>>>>> Maki
    }
}
