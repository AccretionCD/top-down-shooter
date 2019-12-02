using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Transform muzzle;
    [SerializeField] Bullet bullet;

    [SerializeField] float rateOfFire = 100f;
    [SerializeField] float fireVelocity = 50f;

    float timeTillShot;

    public void Shoot()
    {
        if (Time.time > timeTillShot)
        {
            timeTillShot = Time.time + rateOfFire / 1000;
            Bullet newBullet = Instantiate(bullet, muzzle.position, muzzle.rotation);
            newBullet.SetSpeed(fireVelocity);
        }
    }
}