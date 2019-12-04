using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Transform muzzle;
    [SerializeField] Transform extractor;
    [SerializeField] Bullet[] bullets;
    [SerializeField] float bulletSpread = 10f;
    [SerializeField] Shell shell;

    [SerializeField] float rateOfFire = 10f;
    [SerializeField] float fireVelocity = 10f;
    float timeTillShot;

    MuzzleFlash muzzleFlash;

    enum FireMode { Auto, Burst, Single }
    [SerializeField] FireMode fireMode = FireMode.Auto;
    [SerializeField] int shotsPerBurst = 3;

    bool triggerPressed;
    int shotsPerBurstRemaining;

    void Awake()
    {
        muzzleFlash = GetComponent<MuzzleFlash>();

    }

    void Start()
    {
        shotsPerBurstRemaining = shotsPerBurst;
    }

    public void OnTriggerPress()
    {
        Fire();
        triggerPressed = true;
    }

    public void OnTriggerRelease()
    {
        shotsPerBurstRemaining = shotsPerBurst;
        triggerPressed = false;
    }

    void Fire()
    {
        if (Time.time > timeTillShot)
        {
            if (fireMode == FireMode.Burst)
            {
                if (shotsPerBurstRemaining == 0)
                    return;

                shotsPerBurstRemaining--;
            }

            if (fireMode == FireMode.Single)
            {
                if (triggerPressed)
                    return;
            }

            for (int i = 0; i < bullets.Length; i++)
            {
                float randomBulletSpread = Random.Range(-bulletSpread, bulletSpread);
                Quaternion randomRotation = Quaternion.Euler(randomBulletSpread, randomBulletSpread, randomBulletSpread);

                timeTillShot = Time.time + 100 / rateOfFire;
                Bullet newBullet = Instantiate(bullets[i], muzzle.position, muzzle.rotation * randomRotation);
                newBullet.SetSpeed(fireVelocity);
            }
            
            Instantiate(shell, extractor.position, extractor.rotation * Quaternion.Euler(0, 180, 0));
            muzzleFlash.Activate();
        }    
    }
}