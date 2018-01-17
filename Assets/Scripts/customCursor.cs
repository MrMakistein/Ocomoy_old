using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;




public class customCursor : MonoBehaviour
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public float CursorTextureDivider = 4;

    public GameObject LineRender;

    Vector3 p = new Vector3();
    Vector2 objectPoint2D = new Vector2();
    Vector3 objectPoint = new Vector3();
    Vector3 objectPosition = new Vector3();
    Camera c;
    float Distance;
    float relDistance;
    Color dragColor;
    float LineWidth;

    void Start()
    {
        Vector2 hotSpot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        c = Camera.main;
    }

    private void OnGUI()
    {
        if(dnd.draggingObject != null) { 
            objectPosition = c.WorldToScreenPoint(dnd.draggingObject.transform.position);
            objectPoint2D = new Vector2(objectPosition.x, objectPosition.y);

            Distance = Vector2.Distance(Input.mousePosition, objectPoint2D);
            //0 - 1 Value for the distance from the cursor to the object. used to display the "strenght" of the connection
            relDistance = Distance/dnd.DropDistance;
            dragColor = Color.Lerp(Color.green, Color.red, relDistance);

            //thickness from 1 - 10
            LineWidth = 1 + 9.0f * (1.0f - relDistance);


            //update params
            LineRender.SetActive(true);
            LineRender.GetComponent<UILineRenderer>().LineThickness = LineWidth;
            LineRender.GetComponent<UILineRenderer>().color = dragColor;
            LineRender.GetComponent<UILineRenderer>().Points[0] = objectPoint2D;
            LineRender.GetComponent<UILineRenderer>().Points[1] = Input.mousePosition;
        }
        else
        {
            LineRender.SetActive(false);
        }       
    }
}

