using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class dndold : MonoBehaviour
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
    float screenDropDistance = 50f;

    static GameObject draggingObject;
    Rigidbody DrObj;
    Vector3 MouseVector;
    // Use this for initialization
    void Start()
    {
        currentCamera = Camera.main;
        mask = 1 << LayerMask.NameToLayer("is Ground");
    }
    // Update is called once per frame
    void Update()
    {
        if (buttonReleased && Input.GetMouseButton(0) && !atPickUpHeight && !isDragging ||
            buttonReleased && Input.GetMouseButton(0) && !atPickUpHeight && Vector3.Distance(pickUpScreenPos, Input.mousePosition) <= screenDropDistance ||
            Input.GetMouseButton(0) && draggingObject != null && atPickUpHeight && Vector3.Distance(MouseVector, draggingObject.GetComponent<Rigidbody>().transform.position) < dropDistance)
        {
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
            else if (draggingObject != null)
            {
                DrObj = draggingObject.GetComponent<Rigidbody>();
                //Apply force y
                MouseVector = CalculateMouse3DVector();
                DrObj.AddForce((MouseVector - DrObj.transform.position).normalized * forceStrenght, ForceMode.Force);
                pickUpScreenPos = currentCamera.WorldToScreenPoint(DrObj.position);

                Debug.Log("Screendistance: " + Vector3.Distance(pickUpScreenPos, Input.mousePosition));

                if (!atPickUpHeight)
                {
                    pickUpDistance = Vector3.Distance(MouseVector, DrObj.transform.position);
                    DrObj.velocity = pickUpSpeed * ((MouseVector - DrObj.transform.position).normalized) * Vector3.Distance(MouseVector, DrObj.transform.position);
                    Debug.Log("Pick up distance: " + pickUpDistance);
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
                draggingObject.GetComponent<Rigidbody>().velocity = new Vector3(draggingObject.GetComponent<Rigidbody>().velocity.x, 0, draggingObject.GetComponent<Rigidbody>().velocity.z);
            }
            isDragging = false;
            atPickUpHeight = false;
        }

        //Check if the mouse button was released
        Debug.Log(buttonReleased);
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

