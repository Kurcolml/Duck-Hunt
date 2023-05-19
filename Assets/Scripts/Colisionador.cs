using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colisionador : MonoBehaviour {

    public NPC hero;
    public Material mat;
  
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "HUNTER")
        {
            hero = collision.gameObject.GetComponent<NPC>();
            
                hero.hasGun = true;
            Renderer rd = hero.gameObject.GetComponent<Renderer>();
            rd.sharedMaterial = mat;



        }
    }
}
