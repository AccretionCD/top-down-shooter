using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Transform muzzle;
    [SerializeField] Transform extractor;
    [SerializeField] Bullet bullet;
    [SerializeField] Shell shell;

    [SerializeField] float rateOfFire = 100f;
    [SerializeField] float fireVelocity = 50f;
    float timeTillShot;

    MuzzleFlash muzzleFlash;

    void Start()
    {
        muzzleFlash = GetComponent<MuzzleFlash>();
    }

    public void Shoot()
    {
        if (Time.time > timeTillShot)
        {
            timeTillShot = Time.time + rateOfFire / 1000;
            Bullet newBullet = Instantiate(bullet, muzzle.position, muzzle.rotation);
            newBullet.SetSpeed(fireVelocity);

            Instantiate(shell, extractor.position, extractor.rotation * Quaternion.Euler(0, 180, 0));
            muzzleFlash.Activate();
        }
    }
}