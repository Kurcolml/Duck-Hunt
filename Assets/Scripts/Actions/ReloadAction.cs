using UnityEngine;
using System.Collections;
using System;

public class ReloadAction : ActionNode
{

    public override void Execute(Hero reference)
    {
        
        Boid b = reference.hero.GetComponent<Boid>();
        b.speed = 0;
        reference.hero.currentAmmunition = 2;
        reference.hero.Ammunition = 0;

    }
}
