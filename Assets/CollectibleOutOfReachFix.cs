using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleOutOfReachFix : MonoBehaviour {

    public Vector3 original_position;
    public Vector3 middle_position;
    public Vector3 current_position;
    public Vector3 reset_position;


    public string pathName;
    public float time;
    public GameObject dissolveparticles;
    public GameObject reset_collectible;
    public GameObject transitionObject;

    // Use this for initialization
    void Start () {
        original_position = new Vector3(0, 0, 0);
        
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("space"))
        {
            print("space key was pressed");

          

            var iTweenPath = this.GetComponent<iTweenPath>();
            iTweenPath.GetPath("ObjectDissolve");
            iTweenPath.nodes[0] = new Vector3(current_position.x, current_position.y, current_position.z);
            iTweenPath.nodes[1] = new Vector3(original_position.x, original_position.y, original_position.z);
            dissolveparticles.SetActive(true);
            dissolveparticles.transform.position = current_position;
            iTween.MoveTo(dissolveparticles, iTween.Hash("path", iTweenPath.GetPath(pathName), "easetype", iTween.EaseType.easeInOutSine, "time", time, "oncompleteTarget", this.gameObject, "oncomplete", "Finish"));


        }



        //Detect if collectible is at illegal position
        foreach (GameObject col in Player.instance.collectibles) {
            if (!col.GetComponent<InteractiveSettings>().collectibleOnFloor){

                if (col.GetComponent<Rigidbody>().velocity == new Vector3(0, 0, 0) && dnd.draggingObject == null) {
                    Debug.Log(col.name);


                    Debug.Log("STOPPED");
                    reset_collectible = col;
                    reset_position = original_position;
                    var iTweenPath = this.GetComponent<iTweenPath>();
                    iTweenPath.nodes[0] = new Vector3(current_position.x, current_position.y, current_position.z);
                    iTweenPath.nodes[1] = new Vector3(original_position.x, original_position.y, original_position.z);
                    dissolveparticles.SetActive(true);
                    dissolveparticles.transform.position = current_position;
                    transitionObject = col;
                    StartCoroutine("DissolveCollectible");


                   


                    col.GetComponent<InteractiveSettings>().collectibleOnFloor = true;
                }
            }
        }

        // Update reset position for collectible
        if (dnd.draggingObject != null)
        {
            RaycastHit hit;
            Ray landingRay = new Ray(new Vector3(current_position.x, current_position.y, current_position.z), Vector3.down);

       
            current_position = dnd.draggingObject.transform.position;

            float dist = Vector3.Distance(middle_position, current_position);
            if (dist >= 3)
            {
               
                if (Physics.Raycast(landingRay, out hit, Mathf.Infinity))
                {
                    if (hit.collider.tag == "isGround")
                    {
                        original_position = middle_position;
                        middle_position = dnd.draggingObject.transform.position;



                        //Debug.Log("updated original position");



                    }
                }
            }
        }

   






    }

    IEnumerator DissolveCollectible()
    {

        dissolveparticles.GetComponent<ParticleSystem>().Play();
        Debug.Log("d1");
        yield return new WaitForSeconds(1.5f);


        transitionObject.SetActive(false);
        Debug.Log("d2");
        yield return new WaitForSeconds(1f);


        iTween.MoveTo(dissolveparticles, iTween.Hash("path", iTweenPath.GetPath(pathName), "easetype", iTween.EaseType.linear, "time", time, "oncompleteTarget", this.gameObject, "oncomplete", "Finish"));
        Debug.Log("d3");
        yield return new WaitForSeconds(1f);
    }


    void Finish()
    {
        transitionObject.SetActive(true);
        reset_collectible.transform.position = reset_position;
        
    }


    void OnTriggerExit(Collider other)
    {
        var script = other.gameObject.GetComponent<InteractiveSettings>();

        if (script != null)
        {

            if (other.gameObject.GetComponent<InteractiveSettings>().isCollectible)
            {
                Debug.Log("coll");
                other.gameObject.GetComponent<InteractiveSettings>().collectibleOnFloor = false;
            }
        }

    }

    void OnTriggerEnter(Collider other)
    {
             
        var script = other.gameObject.GetComponent<InteractiveSettings>();

        if (script != null)
        {

            if (other.gameObject.GetComponent<InteractiveSettings>().isCollectible)
            {
                
                other.gameObject.GetComponent<InteractiveSettings>().collectibleOnFloor = true;
            }
        }

    }
}
