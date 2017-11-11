using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingBehaviour : MonoBehaviour {
	public float speed; //player speed

	private Rigidbody rb;
	private int count;

	void OnTriggerEnter(Collider other)
	{

		if (other.gameObject.CompareTag("Interactive")) //if trigger is an interactive object...
		{
			other.gameObject.SetActive (false); //if yes: collect
			//for testing:
			GetComponent<Renderer>().material.color = Color.red;


			count++;
		}

		//WIN CONDITIONS
		//wenn alles eingesammelt und wieder zurück an Startzone -> gewonnen
		if ((count == SpawnController.numberOfCollectibles) &&  other.gameObject.CompareTag("Winzone")){
			//GEWONNEN
		}
	}
}
