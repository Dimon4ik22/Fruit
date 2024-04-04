using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HyperCasualPack;
using HyperCasualPack.Pickables;
using Sirenix.OdinInspector;
using UnityEngine;

public class Cardboard : MonoBehaviour
{
    public List<ShelfSlot> slotsInBox;
    [SerializeField] private Vector3 scalToCome;
    [SerializeField] List<ShelfSlot> CocaoSeeds;
    [SerializeField] List<ShelfSlot> MMChocolate;
    [SerializeField] List<ShelfSlot> FERREROROCHER;
    [SerializeField] List<ShelfSlot> HERSHEYKISSES;
    [SerializeField] List<ShelfSlot> CHOCALATECOOKIE;
    [SerializeField] List<ShelfSlot> CHOCALATEPACKAGE;
    [Button]
    private void SetScaleForAnimation()
    {
        scalToCome = transform.localScale;
    }

    private void Awake()
    {
        transform.DOScale(scalToCome, .5f).SetEase(Ease.InQuad);
    }

    public ShelfSlot ReturnEmptySlot()
    {
        foreach (var item in slotsInBox)
        {
            if (item.isAvailable && item.Pickable == null)
            {
                return item;
            }
        }

        return null;
    }

    public List<ShelfSlot> SlotToUse(Pickable pickable)
    {
        switch (pickable.PickableTypes)
        {
            case PickableTypes.COCAOSEED:
                return CocaoSeeds;
            case PickableTypes.MMCHOCOLATE:
                return MMChocolate;
            case PickableTypes.FERREROROCHER:
                return FERREROROCHER;
            case PickableTypes.HERSHEYKISSES:
                return HERSHEYKISSES;
            case PickableTypes.CHOCALATECOOKIE:
                return CHOCALATECOOKIE;
            case PickableTypes.CHOCALATEPACKAGE:
                return CHOCALATEPACKAGE;
        }

        return null;
    }
    public void MoveToSlot(Pickable pickable)
    {
        var slot = ReturnEmptySlot();
        if (slot != null)
        {
            slot.isAvailable = false;
            slot.Pickable = pickable;
            pickable.transform.parent = null;
            pickable.transform.Jump(slot.transform, 1f, 1, 1).OnComplete(() =>
            {
                pickable.transform.parent = this.transform;
            });
        }
    }
}
