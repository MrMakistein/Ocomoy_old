using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float speed; //player speed

	private Rigidbody rb;
	private int count;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		count = 0; //reset count on startup
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//PLAYER MOVEMENT
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed);

//		//WIN CONDITIONS //VVV hier abfragen??
//		if((count == SpawnController.numberOfCollectibles) && (rb.)){
//			//VVV WO KONSTANTE FÜR ANZAHL AN COLLECTIBLES ERSTELLEN?
//		}
	}

	void OnTriggerEnter(Collider other)
	{
//		if (other.gameObject.CompareTag("Collectible")) {
//			other.gameObject.SetActive (false);
//			count++;
//		}

		if (other.gameObject.CompareTag("Interactive")) //if trigger is an interactive object...
		{



//TODO: CONTINUE!!

//			if(){ //test if it is also a collectible!
				other.gameObject.SetActive (false); //if yes: collect
            //for testing:
            GetComponent<Renderer>().material.color = Color.red;


            count++; //and increase collectible count
//			}


		}

		//WIN CONDITIONS
		//wenn alles eingesammelt und wieder zurück an Startzone -> gewonnen
		if ((count == SpawnController.numberOfCollectibles) &&  other.gameObject.CompareTag("Winzone")){
			//GEWONNEN
		}
	}
}
