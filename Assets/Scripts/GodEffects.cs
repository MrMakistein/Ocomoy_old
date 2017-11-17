using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

public class GodEffects : MonoBehaviour
{
    public enum GodEffectType
    {
        tornado, rain, earthshatter, thunder, avalanche, blizzard
    };

    public Material chargeMaterial;
    public GameObject tornadoEffect;
    public GameObject rainEffect;
    public GameObject earthshatterEffect;
    public GameObject thunderEffect;
    public GameObject avalancheEffect;
    public GameObject blizzardEffect;
    public int uid;
    public GameObject display;


    private bool charged = false;

    public GodEffectType CurrentType = 0;
    // Use this for initialization
    void Start()
    {
        CurrentType = (GodEffectType) Random.Range(0, 6);
        display = this.gameObject.transform.GetChild(0).GetChild(0).gameObject;
    }


    // Update is called once per frame
    void Update()
    {

        if (CurrentType == GodEffectType.tornado)
        {
            display.gameObject.GetComponent<Text>().text = "tornado";
        }
        if (CurrentType == GodEffectType.rain)
        {
            display.gameObject.GetComponent<Text>().text = "rain";
        }
        if (CurrentType == GodEffectType.earthshatter)
        {
            display.gameObject.GetComponent<Text>().text = "earthshatter";
        }
        if (CurrentType == GodEffectType.thunder)
        {
            display.gameObject.GetComponent<Text>().text = "thunder";
        }
        if (CurrentType == GodEffectType.avalanche)
        {
            display.gameObject.GetComponent<Text>().text = "avalanche";
        }
        if (CurrentType == GodEffectType.blizzard)
        {
            display.gameObject.GetComponent<Text>().text = "blizzard";
        }

        if (!charged && dnd.draggingObject == this.gameObject && Input.GetMouseButton(1))
        {
            charged = true;
            GetComponent<Renderer>().material = chargeMaterial;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (charged)
        {
            SpawnEffect(CurrentType, this.gameObject.transform.position);
            Destroy(this.gameObject);
        }
    }

    private void SpawnEffect(GodEffectType effectType, Vector3 pos)
    {
        GameObject[] godObjectSpawnPositions = GameObject.FindGameObjectsWithTag("GodObjectSpawnPosition");

        foreach (GameObject godObjectSpawnPosition in godObjectSpawnPositions)
        {
            if (uid == godObjectSpawnPosition.GetComponent<GodEffectSpawnPosition>().uid)
            {
                godObjectSpawnPosition.GetComponent<GodEffectSpawnPosition>().new_spawn_timer = godObjectSpawnPosition.GetComponent<GodEffectSpawnPosition>().new_spawn_duration;
            }

        }




        switch (effectType)
        {
            case GodEffectType.tornado:
                Instantiate(tornadoEffect, pos, Quaternion.identity);
                break;
            case GodEffectType.rain:
                Instantiate(rainEffect, pos, Quaternion.identity);
                break;
            case GodEffectType.avalanche:
                Instantiate(avalancheEffect, pos, Quaternion.identity);
                break;
            case GodEffectType.blizzard:
                Instantiate(blizzardEffect, pos, Quaternion.identity);
                break;
            case GodEffectType.earthshatter:
                Instantiate(earthshatterEffect, pos, Quaternion.identity);
                break;
            case GodEffectType.thunder:
                Instantiate(thunderEffect, pos, Quaternion.identity);
                break;
            default:
                break;
        }
    }
}
