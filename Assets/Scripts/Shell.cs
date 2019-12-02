using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    [SerializeField] float minEjectionForce;
    [SerializeField] float maxEjectionForce;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        float ejectionForce = Random.Range(minEjectionForce, maxEjectionForce);

        rb.AddForce(transform.right * ejectionForce);
        rb.AddTorque(Random.insideUnitSphere * ejectionForce);

        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(4f);

        float percent = 0;
        float fadeRate = 0.5f;

        Material material = GetComponent<MeshRenderer>().material;
        Color color = material.color;

        while (percent < 1)
        {
            percent += Time.deltaTime + fadeRate;
            material.color = Color.Lerp(color, Color.clear, percent);

            yield return null;
        }

        Destroy(gameObject);
    }
}
