using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hero : MonoBehaviour {

    public GameObject NPC;
    public NPC hero;
    public int currentAmmunition;
    public bool enemyInSight;
    public int Ammunition;
    public bool hasGun;
    public Node decisionTreeRoot;

    

    void Awake()
    {
        hero = (NPC)NPC.GetComponent<NPC>();
     
}

    void Update() {
        

        enemyInSight = hero._targetInSight;
        Ammunition = hero.Ammunition;
        hasGun = hero.hasGun;
        currentAmmunition = hero.currentAmmunition;
        decisionTreeRoot.Execute(this);
        

          }
}
