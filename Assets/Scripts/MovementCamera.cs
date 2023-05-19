using UnityEngine;
using System.Collections;

public class MovementCamera : MonoBehaviour {

    float speed;

    void Update()
    {
        Move();
    }

    void Move()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = 30;
        }
        else
        {
            speed = 15;
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(-Vector3.up * speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-Vector3.right * speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }

}
