using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerColliderSetter : MonoBehaviour
{
    [SerializeField] private CapsuleCollider _capsuleCollider;
    public float radiusCollider;
    public float radiusHeight;

    private void Start()
    {
        _capsuleCollider.radius = radiusCollider;
        _capsuleCollider.height = radiusHeight;
    }

    [Button]
    private void SetCollider()
    {
       radiusCollider =_capsuleCollider.radius; 
         radiusHeight =_capsuleCollider.height;
    }
    
}
