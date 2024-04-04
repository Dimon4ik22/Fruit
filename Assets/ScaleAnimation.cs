using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScaleAnimation : MonoBehaviour
{
    void Start()
    {
        transform.DOScale(transform.localScale * 1.3f, 1f).SetLoops(-1, LoopType.Yoyo);
    }
}
