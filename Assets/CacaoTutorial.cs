using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CacaoTutorial : MonoBehaviour
{
    public UnlockerManager UnlockerManager;
    public GameObject pickupcacaotext;
    void Start()
    {
        if (UnlockerManager.unlockIndex <= 3)
        {
            pickupcacaotext.gameObject.SetActive(true);
            Run.After(3, () =>
            {
                pickupcacaotext.gameObject.SetActive(false);
            });
        }
    }
}
