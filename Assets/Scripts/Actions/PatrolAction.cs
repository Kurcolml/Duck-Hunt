using UnityEngine;
using System.Collections;
using System;


public class PatrolAction : ActionNode
{
    
    public int lugar;

    private void Start()
    {
       
    }
    public override void Execute(Hero reference)
    {
        
          
        
        Boid b = reference.hero.GetComponent<Boid>();
        b.speed = 10;
        b.target = reference.hero.lugaresParaBuscar[reference.hero.lugar];
        b.GetBoids();
        reference.hero.target = b.getTargetBoid();

      
    
    }
}
