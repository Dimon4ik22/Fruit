using System;
using Unity;
using System.Collections;
using System.Collections.Generic;
using HyperCasualPack.ScriptableObjects;
using TMPro;
using UnityEngine;

public abstract class UpgradeBase : MonoBehaviour
{
    protected int upgradeOneLevel;
    protected int upgradeTwoLevel;
    protected int upgradeThreeLevel;

    protected abstract string s_upgradeOne { get; set; }
    protected abstract string s_upgradeTwo { get; set; }
    protected abstract string s_upgradeThree { get; set; }

    protected abstract int c_upgradeOne { get; set; }
    protected abstract int c_upgradeTwo { get; set; }
    protected abstract int c_upgradeThree { get; set; }
    
    [SerializeField] protected TMP_Text t_upgradeOneCost;
    [SerializeField] protected TMP_Text t_upgradeTwoCost;
    [SerializeField] protected TMP_Text t_upgradeThreeCost;


    [SerializeField] protected SaveableRuntimeIntVariable money;
    

    public abstract void UpgradeOne();

    public abstract void UpgradeTwo();

    public abstract void UpgradeThree();
}