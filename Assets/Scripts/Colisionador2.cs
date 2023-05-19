using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colisionador2 : MonoBehaviour {
    public Material mat;
    public NPC hero;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="HUNTER")
        {

            hero = collision.gameObject.GetComponent<NPC>();
                hero.Ammunition = 2;
            Renderer rd = hero.gameObject.GetComponent<Renderer>();
            rd.sharedMaterial = mat;

            Debug.Log(hero.Ammunition);   
            
           
        }
    }
}
