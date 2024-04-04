using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MMColorChanger : MonoBehaviour
{
    public List<Color> MMChocolateColors;
    [SerializeField] MeshRenderer _renderer;

    void Awake()
    {
        SetRandomColor();
    }

    void SetRandomColor()
    {
        var random = Random.Range(0, MMChocolateColors.Count);
        _renderer.material.color = MMChocolateColors[random];
    }
}
