using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour {

    public Transform target;
    public BoidFlock elResto;
    public float speed;
    public float rotSpeed;
    public Material mat;

    public Bala(Transform t)
    {
        target = t;
        
    }
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(target!=null)
        {
            Vector3 dir = target.transform.position - transform.position;
            transform.forward = Vector3.Slerp(transform.forward, dir, rotSpeed * Time.deltaTime);
            transform.position += transform.forward * Time.deltaTime * speed;

            transform.rotation = new Quaternion(0,
                                             transform.rotation.y,
                                             transform.rotation.z, transform.rotation.w);
            transform.position = new Vector3(transform.position.x,
                                             1.5f,
                                             transform.position.z);


        }else
        {
            Dead();
        }
       
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer==Layers.BOID)
        {
            BoidFlock bf = collision.gameObject.GetComponent<BoidFlock>();
            if(bf.isLeader)
            {
                BoidFlock nextLeader = bf.friends[0].gameObject.GetComponent<BoidFlock>();
                nextLeader.isLeader = true;
                Collider micol = nextLeader.GetComponent<Collider>();
                Renderer rd = nextLeader.GetComponent<Renderer>();
                rd.sharedMaterial = mat;
                for (int i = 0; i < nextLeader.friends.Count; i++)
                {
                    if(nextLeader.friends[i]!=micol )
                    {
                        elResto = nextLeader.friends[i].gameObject.GetComponent<BoidFlock>();
                        elResto.SetLider(nextLeader.name);
                        Debug.Log("entro" + elResto.BoidLeader.name);
                        Debug.Log("entro" + nextLeader.friends.Count);
                    } 
                 
                       
                }
               
               
            }
            bf.Dead();
            Dead();
           
        }
    }
    public void Dead()
    {
        Destroy(gameObject);
    }
}
