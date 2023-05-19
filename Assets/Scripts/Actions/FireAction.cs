using UnityEngine;
using System.Collections;
using System;

public class FireAction : ActionNode
{
    public float contador ;
    public float tiempoHastaDisparar = 3;
    public GameObject prefab;
    public Material mat;
    public override void Execute(Hero reference)
    {
        contador += Time.deltaTime;
        Debug.Log("shoot");
        Boid b = reference.hero.GetComponent<Boid>();
        b.target = b.getTargetBoid();
        if (contador >= tiempoHastaDisparar)
        {

            contador = 0;
            Debug.Log("balas " + reference.hero.currentAmmunition);
            Bala bt = prefab.GetComponent<Bala>();
            bt.target = b.target.transform;
            Instantiate(prefab, reference.hero.transform.position, reference.hero.transform.rotation);
            reference.hero.currentAmmunition = reference.hero.currentAmmunition - 1;
            if(reference.hero.currentAmmunition==0)
            {
                Renderer rd = reference.hero.gameObject.GetComponent<Renderer>();
                rd.sharedMaterial = mat;
            }
           

        }


        //b.target = ;


    }
}
