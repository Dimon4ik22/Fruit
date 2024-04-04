using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HyperCasualPack;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class StepOver : MonoBehaviour
{
    private Vector3 localScale;
    private BoxCollider _boxCollider;
    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
        localScale = transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            transform.DOScaleX(transform.localScale.x + transform.localScale.x * 0.2f, 0.5f);
            transform.DOScaleZ(transform.localScale.z + transform.localScale.z * 0.2f, 0.5f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            transform.DOScale(localScale, 0.5f);
        }
    }
}
