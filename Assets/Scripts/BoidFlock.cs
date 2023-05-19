using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BoidFlock : MonoBehaviour
{
    public bool isLeader;

    public LayerMask LayerBoid;
    public LayerMask LayerObst;
    public LayerMask LayerHunt;

    public List<Collider> friends;
	public List<Collider> obstacles;

    public float speed;
    public float rotSpeed;

    public Vector3 dir;

    private Vector3 _vectCohesion;
    private Vector3 _vectAlineacion;
    private Vector3 _vectSeparacion;
    private Vector3 _vectLeader;
    private Vector3 _vectAvoidance;
    private Vector3 _vectWander;

    public float radFlock;
    public float radObst;
    public float radHunter;

    public float avoidWeight;
    public float leaderWeight;
    public float alineationWeight;
    public float separationWeight;
    public float cohesionWeight;

    public Transform BoidLeader;

    public float WanderThink;
    float currentTimeToWander;

    public float TimeToGetFriends = 1f;
    float currentTimeToGetFriends;

    public float TimeToGetObst = 1f;
    float currentTimeToGetObst;

    public Collider closerOb;

    public GameObject limiteX;
    public GameObject limiteXm;
    public GameObject limiteZ;
    public GameObject limiteZm;


    ///////////////////////////////SEEK//////////////////////////
    public List<GameObject> ubicaciones;

    public float rotationSpeed;
    public float distanciaMinima;

    private Vector3 _dirToGo;

    ///////////////////////////////////////Flee/////////////////////////

    ///Roulette //////////////////
    ///
    public const int SEEK_A = 0;
    public const int SEEK_B = 1;
    public const int SEEK_C = 2;
    public const int SEEK_D = 3;
    public const int SEEK_E = 4;

    float coefA;
    float coefB;
    float coefC;
    float coefD;
    float coefE;

    List<float> initialValues = new List<float>();
    int decisionIndex;
    private bool isSeek;
    private bool isFlee;
    public List<Collider> hunters;
    private void Start()
    {
        coefA = 10;
        coefB = 20;
        coefC = 30;
        coefD = 40;
        coefE = 50;

        initialValues.Add(coefA);
        initialValues.Add(coefB);
        initialValues.Add(coefC);
        initialValues.Add(coefD);
        initialValues.Add(coefE);
        if (isLeader)
        {
            isSeek = true;
            decisionIndex = RouletteWheelSelection(initialValues);
        }
    }
    public void Dead()
    {
        for (int i = 0; i < friends.Count; i++)
        {
            if (friends[i].gameObject == this.gameObject) ;
            friends.RemoveAt(i);
        }
        Destroy(gameObject);
    }
    public void SetLider(string name)
    {
        BoidLeader = GameObject.Find(name).transform;
    }
    void Update()
    {
        GetFriendsAndObstacles();
        if (hunters.Count > 0)
        {
            isFlee = true;
        }
        else
        {
            isFlee = false;
        }


        if (!isSeek)
        {
            Flock();

        }
        if (isLeader)
        {
           
            if (isSeek)
            {


                Seek(ubicaciones[decisionIndex]);
               

            }
            if (isFlee)
            {

                isSeek = false;
                Flee(hunters[0].gameObject);

                
            }
            if (transform.position.z > limiteZm.transform.position.z || transform.position.z <limiteZ.transform.position.z || transform.position.x > limiteXm.transform.position.x|| transform.position.x < limiteX.transform.position.x)
            {
                isSeek = true;

            }
        }

        transform.rotation = new Quaternion(0,
                                         transform.rotation.y,
                                         transform.rotation.z, transform.rotation.w);
        transform.position = new Vector3(transform.position.x,
                                         1.5f,
                                         transform.position.z);
    }
    void Flock()
    {
        GetFriendsAndObstacles();
        closerOb = GetCloserOb();
        _vectCohesion = getCohesion () * cohesionWeight;
		_vectAlineacion = getAlin () * alineationWeight;
		_vectSeparacion = getSep () * separationWeight;
		_vectLeader = getLeader () * leaderWeight;
        _vectWander = getWander();
        _vectAvoidance = getObstacleAvoidance() * avoidWeight;

        dir = _vectAvoidance;
		dir += isLeader ?  _vectWander : _vectCohesion + _vectAlineacion + _vectSeparacion + _vectLeader;

		transform.forward = Vector3.Slerp (transform.forward, dir, rotSpeed * Time.deltaTime);
        transform.position += transform.forward * Time.deltaTime * speed;
    }


    void GetFriendsAndObstacles()
    {
		friends.Clear ();
		friends.AddRange (Physics.OverlapSphere (transform.position, radFlock, LayerBoid));
		obstacles.Clear ();
		obstacles.AddRange (Physics.OverlapSphere (transform.position, radObst, LayerObst));

        hunters.Clear();
        hunters.AddRange(Physics.OverlapSphere(transform.position, radHunter, LayerHunt));

    }
    Collider GetCloserOb()
    {
        if (obstacles.Count > 0)
        {
            Collider closer = null;
            float dist = 99999;
            foreach (var item in obstacles)
            {
                var newDist = Vector3.Distance(item.transform.position, transform.position);
                if (newDist < dist)
                {
                    dist = newDist;
                    closer = item;
                }
            }
            return closer;
        }
        else
            return null;
    }
    Vector3 getWander()
    {
        Vector3 wander = _vectWander;

        if (isLeader && currentTimeToWander >= WanderThink)
        {
            wander = Vector3.Slerp(transform.forward, new Vector3(Random.Range(-1, 2), 0, Random.Range(-1, 2)), rotSpeed);
            currentTimeToWander = 0;
        }
        else
        {
            currentTimeToWander += Time.deltaTime;
        }

        return wander;
    }

    Vector3 getAlin()
    {
		Vector3 al = new Vector3();
        foreach (var item in friends)
            al += item.transform.forward;
        return al /= friends.Count;
    }

    Vector3 getSep()
    {
		Vector3 sep = new Vector3 ();
        foreach (var item in friends)
        {
            Vector3 f = new Vector3();
            f = transform.position - item.transform.position;
            float mag = radFlock - f.magnitude;
            f.Normalize();
            f *= mag;
            sep += f;
        }
        return sep /= friends.Count;
    }

    Vector3 getCohesion()
    {
		Vector3 coh = new Vector3 ();
        foreach (var item in friends)
            coh += item.transform.position - transform.position;
        return coh /= friends.Count;
    }
    Vector3 getObstacleAvoidance()
    {
        Vector3 sep = new Vector3();
        if(closerOb)
        {
            Vector3 f = new Vector3();
            f = transform.position - closerOb.transform.position;
            float mag = radFlock - f.magnitude;
            f.Normalize();
            f *=mag;
            sep +=f;
            return sep;
        }
        else return Vector3.zero;

        //if (closerOb)
        //    return transform.position - closerOb.transform.position;
        //else return Vector3.zero;
    }
    Vector3 getLeader()
    {
        if (!isLeader)
            return BoidLeader.transform.position - transform.position;
        else
            return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        if(_vectAvoidance != Vector3.zero)
        Gizmos.DrawLine(transform.position, _vectAvoidance);
        Gizmos.color = Color.red;
        if (_vectAvoidance != Vector3.zero)
            Gizmos.DrawLine(transform.position, _vectAvoidance);
        
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, dir);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radObst);
    }


    ///////////////////////////////////////////////////////////SEEK/////////////////////////////////////////
    void Seek(GameObject target)
    {
        var dist = Vector3.Distance(transform.position, target.transform.position);

        if (distanciaMinima < dist)
        {

            _dirToGo = target.transform.position - transform.position;

            transform.forward = Vector3.Lerp(transform.forward, _dirToGo, rotationSpeed * Time.deltaTime);

            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else
        {
            isSeek = false;
            decisionIndex = RouletteWheelSelection(initialValues);
        }
    }


    /// /////////////////////////////////FLEE//////////////////////////////////

    void Flee(GameObject target)
    {
        //Calculamos la direccion hacia la que tenemos que ir
        _dirToGo = -(target.transform.position - transform.position);
        //Vamos ajustando el foward
        transform.forward = Vector3.Lerp(transform.forward, _dirToGo, rotationSpeed * Time.deltaTime);
        //Avanzamos hacia adelante
        transform.position += transform.forward * speed * Time.deltaTime;
    }


    /////////////////////////////////////////ROULETTE//////////////////////////////////////////////////

    public static int RouletteWheelSelection(List<float> values)
    {
        float sumatoria = 0;

        //calculo la sumatoria de todas los coeficientes iniciales:
        sumatoria = values.Sum();

        List<float> coefList = new List<float>();

        //calculo la lista de coeficiente calculados para el roullette:
        foreach (var item in values)
        {
            coefList.Add(item / sumatoria);

        }

        //calculo valor random:
        float rnd = Random.Range(0, 100);
        Mathf.Round(rnd);
        int rndPercent = (int)rnd;
        float r = rndPercent / 100f;


        //corro el algoritmo de seleccion de ruleta
        float sumCoef = 0;
        for (int i = 0; i < values.Count; i++)
        {
            //sumo los deltas a la variable sumcoef para saber en que slot cayo el valor random:
            sumCoef += coefList[i];

            if (sumCoef > r)
                return i;
        }

        return -1;


    }

}
