using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colisionador3: MonoBehaviour {

    public NPC hero;
    public int next;
    public Boid b;
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
            hero.lugar = next;


        }
    }
    
}
