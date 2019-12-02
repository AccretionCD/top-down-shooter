using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 10f;
    Vector3 move;
    Vector3 velocity;

    [SerializeField] float health = 10f;
    [SerializeField] float damageDelay = 1f;
    bool damagable = true;
    bool dead;
    public event System.Action OnDeath;

    [SerializeField] Gun startingGun;
    [SerializeField] Transform gunPosition;
    Gun equippedGun;

    Rigidbody rb;
    Camera cam;


    void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
        cam = Camera.main;
    }

    void Start()
    {
        if(startingGun != null)
            EquipGun(startingGun);
    }

    void Update()
    {
        move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        Aim();

        equippedGun.transform.position = gunPosition.position;
        equippedGun.transform.rotation = gunPosition.rotation;

        Shoot();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        velocity = move.normalized * movementSpeed;
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    void Aim()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, transform.position);

        float distance;

        if (ground.Raycast(ray, out distance))
        {
            Vector3 point = ray.GetPoint(distance);
            Quaternion rotation = Quaternion.LookRotation(point - transform.position);

            transform.rotation = rotation;
        }
    }

    void Shoot()
    {
        if (Input.GetKey(KeyCode.Mouse0))
            if (equippedGun != null)
                equippedGun.Shoot();
    }

    void EquipGun(Gun gun)
    {
        if (equippedGun != null)
            Destroy(equippedGun);

        equippedGun = Instantiate(gun, gunPosition.position, gunPosition.rotation);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && damagable)
        {
            health--;
            StartCoroutine(DamageDelay());
        }
            
        if (health <= 0)
        {
            dead = true;

            OnDeath?.Invoke();

            Destroy(gameObject);
            Destroy(equippedGun.gameObject);
            StopCoroutine(DamageDelay());
        }         
    }

    IEnumerator DamageDelay()
    {
        damagable = false;
        yield return new WaitForSeconds(damageDelay);
        damagable = true;
    }
}
