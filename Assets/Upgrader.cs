using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasualPack;
using UnityEngine;

public class Upgrader : MonoBehaviour
{
    public GameObject CanvasToOpen;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out InventoryManager inventoryManager))
        {
            CanvasToOpen.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out InventoryManager inventoryManager))
        {
            CanvasToOpen.gameObject.SetActive(false);
        }
    }
}
