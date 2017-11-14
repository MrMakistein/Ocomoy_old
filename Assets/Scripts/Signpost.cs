using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signpost : MonoBehaviour {

    public GameObject[] collectibles;
    private GameObject arena;

    public Transform target;
    public float angle;
    public float wiggle_angle;
    public bool collectibleMoving;
    public float wiggleSmooth = 0.2f;
    public float wiggleFrequency = 4.0f;
    public float wiggle_strength = 40;
    public float final_angle;
    public float current_angle;



    // Use this for initialization
    void Start () {
        collectibles = null;
        InvokeRepeating("SetNewWiggleRotation", 0.0f, wiggleFrequency);

    }

    public void SetNewWiggleRotation()
    {
        wiggle_angle = Random.Range((angle - wiggle_strength), (angle + wiggle_strength));

    }


    // Update is called once per frame
    void Update()
    {
        float yVelocity = 0.0F;
        final_angle = Mathf.SmoothDamp(final_angle, wiggle_angle, ref yVelocity, wiggleSmooth);

        current_angle = final_angle;

        if (current_angle > 360.0F)
        {
            current_angle -= 360;
        }
        if (current_angle < 0.0F)
        {
            current_angle += 360;
        }
        //transform.rotation = Quaternion.Lerp(transform.localRotation, Quaternion.AngleAxis(current_angle, Vector3.down), Time.time);
        //transform.rotation.y = final_angle;  
        // Quaternion.AngleAxis(angle, Vector3.down);
        //transform.rotation = Quaternion.Euler(new Vector3(0, 185, 0));




        // Sets collectible moving to true if an active collectible is being moved.
        collectibleMoving = false;
        GetCollectiblesArray();

        foreach (GameObject g in collectibles)
        {
            Rigidbody rb = g.GetComponent<Rigidbody>();
            if (rb.velocity != Vector3.zero && g.activeSelf)
            {
                collectibleMoving = true;
            }

        }
        // Signpost Rotation is only updated when a collectible is moving.
        if (collectibleMoving)
        {
            UpdateSignpostDetection();
        }
    }

    public void UpdateSignpostDetection() // Updates the all fenceposts to face toward the closest collectible
    {
       

        if (collectibles != null)
        {
            target = GetClosestCollectible().transform;
        }

        if (!target) return;

        var myPos = transform.position;
        myPos.y = 0;

        var targetPos = target.position;
        targetPos.y = 0;

        Vector3 toOther = (myPos - targetPos).normalized;

        angle = Mathf.Atan2(toOther.z, toOther.x) * Mathf.Rad2Deg + 180;


    }

    float Atan2Approximation(float y, float x)
    {
        float t0, t1, t3, t4;
        t3 = Mathf.Abs(x);
        t1 = Mathf.Abs(y);
        t0 = Mathf.Max(t3, t1);
        t1 = Mathf.Min(t3, t1);
        t3 = 1f / t0;
        t3 = t1 * t3;
        t4 = t3 * t3;
        t0 = -0.013480470f;
        t0 = t0 * t4 + 0.057477314f;
        t0 = t0 * t4 - 0.121239071f;
        t0 = t0 * t4 + 0.195635925f;
        t0 = t0 * t4 - 0.332994597f;
        t0 = t0 * t4 + 0.999995630f;
        t3 = t0 * t3;
        t3 = (Mathf.Abs(y) > Mathf.Abs(x)) ? 1.570796327f - t3 : t3;
        t3 = (x < 0) ? 3.141592654f - t3 : t3;
        t3 = (y < 0) ? -t3 : t3;
        return t3;
    }


    public void GetCollectiblesArray()
    {
        arena = GameObject.Find("Arena");
        collectibles = arena.GetComponent<SpawnController>().collectibles;
    }


    public GameObject GetClosestCollectible()
    {
        GameObject gMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject g in collectibles)
        {
            float dist = Vector3.Distance(g.transform.position, currentPos);
            if (dist < minDist && g.gameObject.activeSelf)
            {
                print(g.name);
                gMin = g;
                minDist = dist;
            }
        }
        return gMin;
    }

    public void InitializeSignpostRotation()
    {
        UpdateSignpostDetection();
    }
}
