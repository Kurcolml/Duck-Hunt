using UnityEngine;
using System.Collections;
using System;

public class BuscarBalas : ActionNode   
{

    public override void Execute(Hero reference)
    {
        Boid b = reference.hero.GetComponent<Boid>();
        b.target = reference.hero.balas;

    }
}
