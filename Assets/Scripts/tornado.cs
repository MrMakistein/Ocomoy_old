using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tornado : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public float radius = 19.48f;
    public float outsideSpeed = 0.7f;
    public float maxPullInLength = 24.96f;
    public float power = 1;
    public float spin = 100;

    Collider[] colliders;

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void Update()
    {
        colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider c in colliders)
        {
            if (c.GetComponent<Rigidbody>() == null)
            {
                continue;
            }
            Rigidbody rigidbody = c.GetComponent<Rigidbody>();
            if (Vector3.Distance(transform.position, c.transform.position) > maxPullInLength)
            {
                continue;
            }
            Vector3 Force = new Vector3(transform.position.x - c.transform.position.x, rigidbody.velocity.y / 2, transform.position.z - c.transform.position.z) * power;
            rigidbody.AddForceAtPosition(Force, transform.position);
            
            rigidbody.AddForceAtPosition((transform.position - c.transform.position) * power, transform.position);
            rigidbody.AddForce((transform.position - c.transform.position).normalized * spin);
        }
    }
}


