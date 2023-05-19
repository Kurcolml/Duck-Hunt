    using UnityEngine;
using System.Collections.Generic;
using System;

public class QuestionNode : Node {

    public Questions question;
    public Dictionary<Questions, Func<Hero, bool>> DesicionTable = new Dictionary<Questions, Func<Hero, bool>>();
    public Node trueNode;
    public Node falseNode;

    
    Func<Hero, bool> inSight = x => x.enemyInSight;
    Func<Hero, bool> HasGun = x => x.hasGun;
    Func<Hero, bool> HasAmmo = x => x.Ammunition+x.currentAmmunition > 0;
    Func<Hero, bool> amoGreaterThan = x => x.currentAmmunition > 0;

    public enum Questions
    {
        HasGun,
        EnemyInSight,
        AmmunitionGreaterThan,
        HasAmmo,
    }

    void Awake()
    {
        
        DesicionTable.Add(Questions.EnemyInSight, inSight);
        DesicionTable.Add(Questions.AmmunitionGreaterThan, amoGreaterThan);
        DesicionTable.Add(Questions.HasAmmo, HasAmmo);
        DesicionTable.Add(Questions.HasGun, HasGun);
    }

    public override void Execute(Hero reference)
    {
        foreach (var desicion in DesicionTable)
            if (desicion.Key == question)
               (desicion.Value(reference) ? trueNode : falseNode).Execute(reference);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, trueNode.transform.position);
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, falseNode.transform.position);
    }
}
