using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float cleanDelay = 2f;

    float speed;

    void Update()
    {
        transform.Translate(Vector3.back * Time.deltaTime * speed);

        Destroy(gameObject, cleanDelay);
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}