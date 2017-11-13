using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalMath : MonoBehaviour {

    //These two values are used as there are some gameplay relevant functions that depend on the screen resolution
    private static float editorScreenMean = (1053 + 459) / 2;
    private static float playScreenMean;

    // Use this for initialization
    void Start () {
        playScreenMean = (Screen.width + Screen.height) / 2;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

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
