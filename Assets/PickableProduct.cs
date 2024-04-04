using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PickableProduct : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Collider col;
    [SerializeField] float delay;
    public bool isSpawnerProduct = false;
    Vector3 initialRotation;
    void Awake()
    {
        initialRotation = transform.eulerAngles;
    }

    void OnEnable()
    {
        Run.After(delay, () =>
        {
            if (!isSpawnerProduct)
            {
                if (transform.eulerAngles != initialRotation)
                    transform.eulerAngles = initialRotation;
                rb.AddForce(transform.right * -5, ForceMode.Impulse);
            }
            else
            {
                if (transform.eulerAngles != initialRotation)
                    transform.eulerAngles = initialRotation;
                rb.AddForce(new Vector3(0,0,-1)* 4, ForceMode.Impulse);
            }
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
