using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSeedSlot : MonoBehaviour
{
    public SeedSlot SeedSlot;
    [SerializeField] PickableTreeSpawner _pickableTreeSpawner;
    private void Awake()
    {
        SeedSlot = new SeedSlot(true,transform.position);
        _pickableTreeSpawner.seedSpawnerPositions.Add(SeedSlot);
    }

    public void AddToTree()
    {
        _pickableTreeSpawner.seedSpawnerPositions.Add(SeedSlot);
    }
}
