using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed;
    void Update()
    {
        var modifiedTarget = new Vector3(target.position.x, target.position.y+2, target.position.z);
        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(modifiedTarget-transform.position),
            rotationSpeed*Time.deltaTime);
    }
}
