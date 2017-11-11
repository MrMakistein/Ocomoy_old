using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Health_Damage_System : MonoBehaviour {
    public float maxHealth = 100;
    public float currentHealth;
    public Image healthBar;

	// Use this for initialization
	void Start () {
        healthBar.fillAmount = 1;
        currentHealth = maxHealth;
    }
	
	// Update is called once per frame
	void Update () {
        
         
		
	}

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Interactive")
        {
            
            
            if (col.gameObject.GetComponent<ThrowObject>().weight_class == 1)
            {
                currentHealth = currentHealth - 10;
                healthBar.fillAmount = currentHealth/maxHealth;
            }

            if (col.gameObject.GetComponent<ThrowObject>().weight_class == 2)
            {
                currentHealth = currentHealth - 10;
                healthBar.fillAmount = currentHealth / maxHealth;
            }

            if (col.gameObject.GetComponent<ThrowObject>().weight_class == 3)
            {
                currentHealth = currentHealth - 10;
                healthBar.fillAmount = currentHealth / maxHealth;
            }

            if (col.gameObject.GetComponent<ThrowObject>().weight_class == 4)
            {
                currentHealth = currentHealth - 10;
                healthBar.fillAmount = currentHealth / maxHealth;
            }



        }
    }


  
}
