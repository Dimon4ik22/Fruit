using System.Collections;
using System.Collections.Generic;
using HyperCasualPack.Pickables;
using UnityEngine;

public class ShelfSlot : MonoBehaviour
{
    public bool isAvailable = true;
    public Pickable Pickable;

    public void ClearSlot()
    {
        isAvailable = true;
        Pickable = null;
    }
}
