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
    [SerializeField] int bulletsPerMagazine = 50;
    [SerializeField] float reloadSpeed = 2f;
    float timeTillShot;
    int bulletsRemaining;
    bool reloading;

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
        bulletsRemaining = bulletsPerMagazine;
    }

    void Update()
    {
        if (!reloading && bulletsRemaining <= 0 || Input.GetKeyDown(KeyCode.R))
            StartCoroutine(Reload());
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
        if (Time.time > timeTillShot && bulletsRemaining > 0 && !reloading)
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

            if (bulletsRemaining != 0)
            {
                for (int i = 0; i < bullets.Length; i++)
                {
                    float randomBulletSpread = Random.Range(-bulletSpread, bulletSpread);
                    Quaternion randomRotation = Quaternion.Euler(randomBulletSpread, randomBulletSpread, randomBulletSpread);

                    timeTillShot = Time.time + 100 / rateOfFire;
                    Bullet newBullet = Instantiate(bullets[i], muzzle.position, muzzle.rotation * randomRotation);
                    newBullet.SetSpeed(fireVelocity);
                    bulletsRemaining--;
                }

                Instantiate(shell, extractor.position, extractor.rotation * Quaternion.Euler(0, 180, 0));
                muzzleFlash.Activate();
            }
        }
    }

    IEnumerator Reload()
    {
        reloading = true;

        yield return new WaitForSeconds(0.25f);

        float percent = 0;
        Vector3 rotation = transform.eulerAngles;

        while (percent < 1)
        {
            percent += Time.deltaTime * reloadSpeed;

            float interpolation = 4 * (-Mathf.Pow(percent, 2) + percent);
            float reloadAngle = Mathf.Lerp(0, 30, interpolation);
            transform.eulerAngles = rotation + Vector3.right * reloadAngle;

            yield return null;
        }

        reloading = false;
        bulletsRemaining = bulletsPerMagazine;
    }
}