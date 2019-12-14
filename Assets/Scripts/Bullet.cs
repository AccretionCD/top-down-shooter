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
        if (!other.CompareTag("Bullet"))
            Destroy(gameObject);
    }
}