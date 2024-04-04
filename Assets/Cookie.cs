using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookie : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Collider col;
    [SerializeField] float delay;
    void OnEnable()
    {
        Run.After(delay, () =>
        {
            rb.AddForce(transform.right * -5, ForceMode.Impulse);
        });
    }

    public void DisablePhyiscs()
    {
        rb.isKinematic = true;
        col.enabled = false;
    }

    void OnDisable()
    {
        rb.isKinematic = false;
        col.enabled = true;
    }
}
