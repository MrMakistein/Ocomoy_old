using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class dnd : MonoBehaviour
{
    int mask;
    bool buttonReleased = true;
    public float pickUpThreshhold = 0.6f;
    public float catchingDistance;
    public float pickUpHeight;
    public static Camera currentCamera;
    public static float forceStrenght = 20f;
    float pickUpSpeed = 10f;
    bool isDragging = false;
    Vector3 pickUpScreenPos;
    public float screenDropDistance = 80f;
    public float mouseVectorMultiplier = 10f;
    public float upwardVelocityThreshhold = 1f;
    //These two values are used as there are some gameplay relevant functions that depend on the screen resolution
    private float editorScreenMean = (1053 + 459) / 2;
    private float playScreenMean;

    

    public static GameObject draggingObject;
    Rigidbody DrObj;
    Vector3 MouseVector;
    
    void Start()
    {
        currentCamera = Camera.main;
        mask = 1 << LayerMask.NameToLayer("is Ground");
        playScreenMean = (Screen.width+Screen.height)/ 2;
        screenDropDistance = (screenDropDistance / editorScreenMean) * playScreenMean;
        mouseVectorMultiplier = (mouseVectorMultiplier / editorScreenMean) * playScreenMean;
        Debug.Log("screenDropDistance " + screenDropDistance);
        Debug.Log("mouseVectorMultiplier " + mouseVectorMultiplier);
    }
    // Update is called once per frame
    void Update()
    {
       
        if (buttonReleased && Input.GetMouseButton(0) && (!isDragging || Vector3.Distance(pickUpScreenPos, Input.mousePosition) <= screenDropDistance))
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
                //Apply force 
                MouseVector = CalculateMouse3DVector();
                //DrObj.AddForce((MouseVector - DrObj.transform.position).normalized * forceStrenght, ForceMode.Force);
                pickUpScreenPos = currentCamera.WorldToScreenPoint(DrObj.position);
                DrObj.velocity = pickUpSpeed * ((MouseVector - DrObj.transform.position).normalized) * Vector3.Distance(MouseVector, DrObj.transform.position);

                draggingObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

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
                if (DrObj.velocity.y > upwardVelocityThreshhold)
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
        return gmObj;
    }

    //Calculates the point according to the mouse courser in a specified height(using pickUpHeight)
    private Vector3 CalculateMouse3DVector()
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
            float resutlingDistance = pickUpHeight / Mathf.Cos(Mathf.PI - cosineDegrees);

            v3 = Vector3.Lerp(A, CamPos, resutlingDistance / originalDistance);
        }
        return v3;
    }
}
