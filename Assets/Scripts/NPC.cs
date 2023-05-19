using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{

    public GameObject target;
    

    public List<GameObject> lugaresParaBuscar;
    public float viewAngle;
    public float viewDistance;

    private Vector3 _dirToTarget;
    private float _angleToTarget;
    private float _distanceToTarget;

    public int currentAmmunition;
    
    public int Ammunition;
    public bool hasGun;
    public bool _targetInSight;

    public GameObject armeria;
    public GameObject balas;

    public List<bool> deciciones = new List<bool>();
    public int lugar;
    private void Start()
    {
        lugar = setRandom();
    }
    void Update()
    {
        if(target!=null)
        {
            Sight();
        }else
        {
            _targetInSight = false;
        }
        
        
    }
   
    
   
    void Sight()
    {
        //La dirección desde un punto a otro es: Posición Final - Posición Inicial NORMALIZADA
        _dirToTarget = (target.transform.position - transform.position).normalized;

        //Vector3.Angle nos da el ángulo entre dos direcciones
        _angleToTarget = Vector3.Angle(transform.forward, _dirToTarget);

        //Vector3.Distance nos da la distancia entre dos posiciones
        //Es lo mismo que hacer: Posición Final - Posición Inicial y sacar la magnitud del vector
        //_distanceToTarget = (target.transform.position - transform.position).magnitude;
        _distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        
        //Si entra en el angulo y en el rango de vision 
        if (_angleToTarget <= viewAngle && _distanceToTarget <= viewDistance)
        {
            RaycastHit rch;
            bool obstaclesBetween = false;

            //Se hace un chequeo de colisiones
            if (Physics.Raycast(transform.position, _dirToTarget, out rch, _distanceToTarget))
                if (rch.collider.gameObject.layer == Layers.WALL)
                    obstaclesBetween = true;

            if (!obstaclesBetween)
                _targetInSight = true;
            else
                _targetInSight = false;
        }
        else
            _targetInSight = false;

    }

    public int setRandom()
    {
        int r = Random.Range(0, lugaresParaBuscar.Count);
        return r;
        
    }
   
    void OnDrawGizmos()
    {
        /*
        Dibujamos una línea desde el NPC hasta el enemigo.
        Va a ser de color verde si lo esta viendo, roja sino.
        */
        if (_targetInSight)
            Gizmos.color = Color.green;
        else if(target!=null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
           

        /*
        Dibujamos los límites del campo de visión.
        */
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * viewDistance));

        Vector3 rightLimit = Quaternion.AngleAxis(viewAngle, transform.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + (rightLimit * viewDistance));

        Vector3 leftLimit = Quaternion.AngleAxis(-viewAngle, transform.up) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + (leftLimit * viewDistance));
    }
}
