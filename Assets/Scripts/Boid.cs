using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Boid : MonoBehaviour
{
    public GameObject target;
    public NPC hero;
    public List<Collider> obstacles = new List<Collider>();
    public List<Collider> palomas = new List<Collider>();
    
    Vector3 dir;
    Vector3 _vectTarget;
    Vector3 _vectAvoidance;
    public Collider closerOb;

    public LayerMask LayerObst;
    public LayerMask LayerBoid;
    public float speed;
    public float rotSpeed;
    public float radObst;
    public float radBoids;
    public float avoidWeight;
    public float targetWeight;

    private void Start()
    {
        hero = this.GetComponent<NPC>();
    }

    void Update()
    {
        
            GetObstacles();
            Move();
        
        
    }
    public void GetBoids()
    {
        palomas.Clear();
        palomas.AddRange(Physics.OverlapSphere(transform.position, radBoids, LayerBoid));


    }
    public GameObject getTargetBoid()
    {
        if (palomas.Count > 0)
        {
            Collider closer = null;
            float dist = radBoids;
            foreach (var item in palomas)
            {
                if(item!=null)
                {

                    var newDist = Vector3.Distance(item.transform.position, transform.position);
                    if (newDist < dist)
                    {
                        dist = newDist;
                        closer = item;
                    }
                }
            }
            return closer.gameObject;
        }
        else
            return null;
    }
    void Move()
    {
        GetObstacles();
        closerOb = GetCloserOb();
        _vectAvoidance = getObstacleAvoidance() * avoidWeight;
        _vectTarget = getTarget() * targetWeight;

        dir = _vectAvoidance + _vectTarget;
		transform.forward = Vector3.Slerp(transform.forward, dir, rotSpeed * Time.deltaTime);
        transform.position += transform.forward * Time.deltaTime * speed;

        transform.rotation = new Quaternion(0,
                                         transform.rotation.y,
                                         transform.rotation.z,transform.rotation.w);
        transform.position = new Vector3(transform.position.x,
                                         1.5f,
                                         transform.position.z);

    }


    void GetObstacles()
    {
		obstacles.Clear ();
		obstacles.AddRange (Physics.OverlapSphere (transform.position, radObst, LayerObst));
    }

    Collider GetCloserOb()
    {
        if (obstacles.Count > 0)
            return obstacles.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).First();
        else
            return null;
    }
    Vector3 getObstacleAvoidance()
    {
        if (closerOb)
            return transform.position - closerOb.transform.position;
        else return Vector3.zero;
    }
    Vector3 getTarget()
    {
        
        if(target!=null)
        {
            return target.transform.position - transform.position;

        }else
            return transform.position;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (_vectAvoidance != Vector3.zero)
            Gizmos.DrawLine(transform.position, _vectAvoidance);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, _vectTarget);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, dir);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radObst);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radBoids);
    }
   
}
