using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class dnd : MonoBehaviour
{
    int mask;
    bool buttonReleased = true;
    public bool draggingDrag = true;
    public float catchingDistance = 100;
    public float pickUpHeight = 2;
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
    

    //needed to update the draggingobject when moving the camera
    private Vector3 oldCameraPosition;
    private Vector3 cameraDifference;

    public static GameObject draggingObject;
    Rigidbody DrObj;
    Vector3 MouseVector;

    //for math
    private static float editorScreenMean = (1053 + 459) / 2;
    private static float playScreenMean;
    

    void Awake  ()
    {
        playScreenMean = (Screen.width + Screen.height) / 2;
        currentCamera = Camera.main;
        oldCameraPosition = currentCamera.transform.position;
        mask = 1 << LayerMask.NameToLayer("is Ground");
        //Every screen dependent variable has to be scaled to fit any resolution
        initialDropDistance = ScreenSizeCompensation(initialDropDistance);
        mouseVectorMultiplier = ScreenSizeCompensation(mouseVectorMultiplier);
    }
    // Update is called once per frame
    void Update()
    {        


        if (buttonReleased && Input.GetMouseButton(0) && (!isDragging || Vector3.Distance(pickUpScreenPos, Input.mousePosition) <= DropDistance))
        {
            //Check and Apply Slow Effect
            slowEffectCheckAndApply();

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

               
                pickUpScreenPos = currentCamera.WorldToScreenPoint(DrObj.position);
                MouseVector = CalculateMouse3DVector(currentCamera, mask, pickUpHeight + heightOffset);
                //Apply force 
                if (draggingDrag)
                {
                   
                    DrObj.AddForce((MouseVector - DrObj.transform.position).normalized * forceStrenght, ForceMode.Force);
                    DrObj.drag = (currentWeightInfluence * 1) / Vector3.Distance(DrObj.transform.position, MouseVector);
                }
                else
                {
                    DrObj.velocity = pickUpSpeed * ((MouseVector - DrObj.transform.position).normalized) * Vector3.Distance(MouseVector, DrObj.transform.position);
                }

                //Update Position for camera movement
                cameraDifference = currentCamera.transform.position - oldCameraPosition;
                DrObj.transform.position += cameraDifference;
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
                if (DrObj.velocity.y > upwardVelocityThreshhold && !draggingDrag)
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

        //Camera Update
        oldCameraPosition = currentCamera.transform.position;        
    }

    public void ReleaseObject()
    {
        buttonReleased = false;
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
                (hitInfo.collider.gameObject.tag == "Interactive" || hitInfo.collider.gameObject.tag == "GodObject"))
            {
                gmObj = hitInfo.collider.gameObject;
            }
        }

        //Assign the correct weightclass
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
            if(draggingObject != null)
            {
                draggingObject.GetComponent<Rigidbody>().mass = Player.slow_mass;
            }
        }
    }


    //Math
    public static float ScreenSizeCompensation(float toCompensate)
        {        
            return (toCompensate / editorScreenMean) * playScreenMean;
        }

    //Calculates the point according to the mouse courser in a specified height(using pickUpHeight)
    public static Vector3 CalculateMouse3DVector(Camera currentCamera, LayerMask mask, float pickUpHeight)
    {
        Vector3 v3 = Input.mousePosition;
        v3 = currentCamera.ScreenToWorldPoint(v3);
        //raycast to determine the distance from the camera to an object and the angle of the hit
        RaycastHit hitInfo = new RaycastHit();
        Ray r = currentCamera.ScreenPointToRay(Input.mousePosition);

        //!!! A distance (3rd Position) is needed, because otherwise Unity behaves buggy !!!
        bool hit = Physics.Raycast(r, out hitInfo, 100, mask);
        if (hit)
        {
            //point where ray hits the surface
            Vector3 A = hitInfo.point;
            Vector3 CamPos = currentCamera.gameObject.transform.position;
            float originalDistance = Vector3.Distance(A, CamPos);

            //Use trigonometry to calculate the point on the ray, where the height is pickUpHeight
            float cosine = Vector3.Dot(r.direction, hitInfo.normal);
            float cosineDegrees = Mathf.Acos(cosine);
            float resutlingDistance = (pickUpHeight) / Mathf.Cos(Mathf.PI - cosineDegrees);

            v3 = Vector3.Lerp(A, CamPos, resutlingDistance / originalDistance);
        }
        return v3;
    }


}
