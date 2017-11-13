using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signpost : MonoBehaviour {

    public GameObject[] collectibles;
    private GameObject arena;

    public Transform target;
    public float angle;
    public float angleOpt;

    // Use this for initialization
    void Start () {
        Invoke("GetCollectiblesArray", 0.1f);
        collectibles = null;

    }

    float Angle360(Vector3 from, Vector3 to, Vector3 right)
    {
        float angle = Vector3.Angle(from, to);
        return (Vector3.Angle(right, to) > 90f) ? 360f - angle : angle;
    }

    // Update is called once per frame
    void Update () {

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.down);


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
        angleOpt = Atan2Approximation(toOther.z, toOther.x) * Mathf.Rad2Deg + 180;

        Debug.DrawLine(myPos, targetPos, Color.yellow);




        if (Input.GetKeyDown("j"))
        {
            Vector3 to = transform.position - GetClosestCollectible().transform.position;
            Vector3 from = new Vector3(0, 0, 1);
            print("Player Position Vector: " + transform.position);
            print("Collectible Position Vector: " + GetClosestCollectible().transform.position);
            print("Vector between: " + (transform.position - GetClosestCollectible().transform.position));
            print("Vector from" + from);
            float angle = Vector3.Angle(transform.forward, (transform.position - GetClosestCollectible().transform.position));
            //print(angle);
            print(Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z);
        }

        //Vector3 targetDir = GetClosestCollectible().transform.position - transform.position;

        /*
        Vector3 targetDir = GetClosestCollectible().transform.position - transform.position;
        float step = 5 * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        Debug.DrawRay(transform.position, newDir, Color.red);
        transform.rotation = Quaternion.LookRotation(newDir); */
        //print(GetClosestCollectible().gameObject.name);
        // print("Signpost Vector: " + transform.position);
        //print("Collectible Vector: " + (transform.position + GetClosestCollectible().transform.position));

        //Vector3 collectibleVector = transform.position + GetClosestCollectible().transform.position;
        //float angle = Vector3.Angle(transform.forward, GetClosestCollectible().transform.position);
        //print(angle);






    }

    float Atan2Approximation(float y, float x) // http://http.developer.nvidia.com/Cg/atan2.html
    {
        float t0, t1, t2, t3, t4;
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


    void GetCollectiblesArray()
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
            if (dist < minDist)
            {
                gMin = g;
                minDist = dist;
            }
        }
        return gMin;
    }
}
