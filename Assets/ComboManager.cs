using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour {

    public int combo_level = 0;
    public float combo_timer = 0;
    public float combo_time_limit;

    public Texture2D cursor1;
    public Texture2D cursor2;
    public Texture2D cursor3;
    public Texture2D cursor4;

    public static ComboManager instance;

    void Start () {
        ComboManager.instance = this;
    }
	
	
	void Update () {

        if (combo_timer == 10) // Increase Combo Level
        {
            if (combo_level < 5)
            {
                combo_level = combo_level + 1;
            }
            UpdateCursor();


        }

		if (combo_timer > 0) // Decrease Combo Timer
        {
            combo_timer = combo_timer - combo_time_limit;
        }


        if (combo_timer <= 0) //Reset Combo
        {
            combo_level = 0;
            customCursor.instance.cursorTexture = cursor1;
            customCursor.instance.UpdateCursor();
        }

        if (dnd.draggingObject != null && combo_level >= 3)
        {
            dnd.draggingObject.GetComponent<InteractiveSettings>().combo_particle_reset_timer = 2.5f;
        }



	}

    public void UpdateCursor() {

        if (combo_level <= 2)
        {
            customCursor.instance.cursorTexture = cursor1;
            Player.instance.combo_dmg_multiplier = 1f;
            
        }
        if (combo_level == 3)
        {
            customCursor.instance.cursorTexture = cursor2;
            Player.instance.combo_dmg_multiplier = 1.25f;
        }
        if (combo_level == 4)
        {
            customCursor.instance.cursorTexture = cursor3;
            Player.instance.combo_dmg_multiplier = 1.5f;
        }
        if (combo_level == 5)
        {
            customCursor.instance.cursorTexture = cursor4;
            Player.instance.combo_dmg_multiplier = 2f;
        }
        customCursor.instance.UpdateCursor();

    }
}
