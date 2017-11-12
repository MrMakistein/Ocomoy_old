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
        
}
