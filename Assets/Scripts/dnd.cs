using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class dnd : MonoBehaviour
{
    int mask;
    bool buttonReleased = true;
    public float catchingDistance;
    public float pickUpHeight;
    public Camera currentCamera;
    public static float forceStrenght = 20f;
    float pickUpSpeed = 10f;
    float cursorSpeed;
    bool atPickUpHeight = false;
    bool isDragging = false;
    float dropDistance = 2f;
    float pickUpDistance;
    Vector3 pickUpScreenPos;
    float screenDropDistance = 80f;
    float mouseVectorDevide = 5;

    static GameObject draggingObject;
    Rigidbody DrObj;
    Vector3 MouseVector;    
    Vector3 MouseDelta = Vector3.zero;
    private Vector3 lastPos = Vector3.zero;
    
    void Start()
    {
        currentCamera = Camera.main;
        mask = 1 << LayerMask.NameToLayer("is Ground");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastPos = Input.mousePosition;
        }

        if (buttonReleased && Input.GetMouseButton(0) && ( !atPickUpHeight && !isDragging || Vector3.Distance(pickUpScreenPos, Input.mousePosition) <= screenDropDistance))
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
                    pickUpDistance = Vector3.Distance(MouseVector, draggingObject.GetComponent<Rigidbody>().transform.position);
                    pickUpScreenPos = currentCamera.WorldToScreenPoint(draggingObject.GetComponent<Rigidbody>().position);
                }
            }

            //while dragging
            else if (draggingObject != null)
            {
                MouseDelta = Input.mousePosition - lastPos;
                Debug.Log("X: " + MouseDelta.x);
                Debug.Log("Y: " + MouseDelta.y);

                DrObj = draggingObject.GetComponent<Rigidbody>();
                //Apply force 
                MouseVector = CalculateMouse3DVector();
                DrObj.AddForce((MouseVector - DrObj.transform.position).normalized * forceStrenght, ForceMode.Force);
                pickUpScreenPos = currentCamera.WorldToScreenPoint(DrObj.position);
                
                if (!atPickUpHeight)
                {
                    pickUpDistance = Vector3.Distance(MouseVector, DrObj.transform.position);
                    DrObj.velocity = pickUpSpeed * ((MouseVector - DrObj.transform.position).normalized) * Vector3.Distance(MouseVector, DrObj.transform.position);
                    if (DrObj.transform.position.y >= pickUpHeight)
                    {
                        atPickUpHeight = true;
                        DrObj.constraints = RigidbodyConstraints.FreezePositionY;                        
                    }
                }
                if (atPickUpHeight)
                {
                    DrObj.drag = 1/Vector3.Distance(DrObj.transform.position, MouseVector);
                }
            }
        }

        //stop dragging
        else
        {
            if (draggingObject != null)
            {
                draggingObject.GetComponent<Rigidbody>().gameObject.layer = LayerMask.NameToLayer("Default");
                draggingObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                draggingObject.GetComponent<Rigidbody>().drag = 0;
            }
            if (!atPickUpHeight && isDragging)
            {
                draggingObject.GetComponent<Rigidbody>().velocity = new Vector3(MouseDelta.x / mouseVectorDevide, 0, MouseDelta.y / mouseVectorDevide);
            }
            isDragging = false;
            atPickUpHeight = false;
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

        lastPos = Input.mousePosition;
    }

    private GameObject GetObjectFromMouseRaycast()
    {
        GameObject gmObj = null;
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(currentCamera.ScreenPointToRay(Input.mousePosition), out hitInfo);
        if (hit)
        {
            Debug.Log(Vector3.Distance(hitInfo.collider.gameObject.transform.position, currentCamera.transform.position));
            if (hitInfo.collider.gameObject.GetComponent<Rigidbody>() &&
                Vector3.Distance(hitInfo.collider.gameObject.transform.position, currentCamera.transform.position) <= catchingDistance &&
                !hitInfo.collider.gameObject.GetComponent<Rigidbody>().isKinematic)
            {
                gmObj = hitInfo.collider.gameObject;
            }
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
            Debug.Log("RayHit: " + A);
            Vector3 CamPos = currentCamera.gameObject.transform.position;
            float originalDistance = Vector3.Distance(A, CamPos);

            //Use trigonometry to calculate the point on the ray, where the height is pickUpHeight
            float cosine = Vector3.Dot(r.direction, hitInfo.normal);
            float cosineDegrees = Mathf.Acos(cosine);
            float resutlingDistance = pickUpHeight / Mathf.Cos(Mathf.PI - cosineDegrees);

            v3 = Vector3.Lerp(A, CamPos, resutlingDistance / originalDistance);
            Debug.Log(v3);
        }
        else
        {
            Debug.Log("no Hit");
            Debug.Log("Mask: " + mask);
        }
        return v3;
    }
}

/* -- Hold for Slider
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Slider : MonoBehaviour {
    public Slider mainSlider;

    // Use this for initialization
    void Start() {
        mainSlider = GameObject.Find("Slider").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update(){
        Debug.Log(mainSlider);
        mainSlider.V
    }
}
*/