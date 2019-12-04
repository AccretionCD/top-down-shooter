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

        equippedGun.transform.position = gunPosition.position;
        equippedGun.transform.rotation = gunPosition.rotation;

        Aim();
        Shoot();
        CheckHealth();
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

        if (ground.Raycast(ray, out float distance))
        {
            Vector3 point = ray.GetPoint(distance);
            Quaternion rotation = Quaternion.LookRotation(point - transform.position);

            transform.rotation = rotation;
        }
    }

    void Shoot()
    {
        if (equippedGun != null)
        {
            if (Input.GetKey(KeyCode.Mouse0))
                equippedGun.OnTriggerPress();

            else
                equippedGun.OnTriggerRelease();
        }
    }

    void EquipGun(Gun gun)
    {
        if (equippedGun != null)
            Destroy(equippedGun);

        equippedGun = Instantiate(gun, gunPosition.position, gunPosition.rotation);
    }

    void CheckHealth()
    {
        if (health <= 0)
        {
            StopCoroutine(DamageDelay());

            OnDeath?.Invoke();

            Destroy(gameObject);
            Destroy(equippedGun.gameObject);
        }
    }

    IEnumerator DamageDelay()
    {
        damagable = false;
        yield return new WaitForSeconds(damageDelay);
        damagable = true;
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && damagable)
        {
            health--;
            StartCoroutine(DamageDelay());
        }   
    }
}
