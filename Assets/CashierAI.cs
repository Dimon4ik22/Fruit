using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashierAI : MonoBehaviour
{
    

    void OnEnable()
    {
        CashierManager.Instance.Cashier.cashierAIHasUnlocked = true;
        if (PlayerPrefs.GetInt("Cashier") != 1)
        {
            PlayerPrefs.SetInt("Cashier",1);
        }
    }
}
