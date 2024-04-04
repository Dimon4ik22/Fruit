using System.Collections;
using System.Collections.Generic;
using HyperCasualPack;
using HyperCasualPack.Pickables;
using HyperCasualPack.ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;

public class UpgradePlayer : UpgradeBase
{
    // Start is called before the first frame update
    protected override string s_upgradeOne { get; set; }
    protected override string s_upgradeTwo { get; set; }
    protected override string s_upgradeThree { get; set; }
    protected override int c_upgradeOne { get; set; }
    protected override int c_upgradeTwo { get; set; }
    protected override int c_upgradeThree { get; set; }

    [SerializeField] MovementDataSO _movementDataSo;
    [SerializeField] InventoryManager _inventoryManager;
    [SerializeField] List<PickableData> IncomeDatas;
    void Start()
    {
        s_upgradeOne = "MoveSpeed";
        s_upgradeTwo = "Capacity";
        s_upgradeThree = "ProfitsUp";
        Load();
        c_upgradeOne = 150 *upgradeOneLevel;
        c_upgradeTwo = 225 * upgradeTwoLevel;
        c_upgradeThree = 250 * upgradeThreeLevel;
        
        t_upgradeOneCost.text = (  c_upgradeOne * 1.5f).ToString();
        t_upgradeTwoCost.text = (c_upgradeTwo * 1.6f).ToString();
        t_upgradeThreeCost.text = (  c_upgradeThree * 2f).ToString();
        
        if (upgradeOneLevel == 5)
        {
            t_upgradeOneCost.text = "MAX";
        }
        if (upgradeThreeLevel == 5)
        {
            t_upgradeTwoCost.text = "MAX";

        }
        if (upgradeThreeLevel == 5)
        {
            t_upgradeThreeCost.text = "MAX";

        }
    }

    public override void UpgradeOne()
    {
        if (upgradeOneLevel == 5)
        {
            t_upgradeOneCost.text = "MAX";
            return;
        }
        int calculateCost = (int)(upgradeOneLevel * c_upgradeOne * 1.5f);
        if (money.RuntimeValue > calculateCost)
        {
            upgradeOneLevel++;
            t_upgradeOneCost.text = (upgradeOneLevel * c_upgradeOne * 1.5f).ToString();
            UpgradeMovementSpeed();
            money.RuntimeValue -= calculateCost;
            Save();
        }
    }

    private void Load()
    {
        if (PlayerPrefs.GetInt("PlayerupgradeOneLevel") != 0)
        {
            upgradeOneLevel = PlayerPrefs.GetInt("PlayerupgradeOneLevel");
        }
        else
        {
            upgradeOneLevel = 1;
        }
        if (PlayerPrefs.GetInt("PlayerupgradeTwoLevel") != 0)
        {
            upgradeTwoLevel = PlayerPrefs.GetInt("PlayerupgradeTwoLevel");
        }
        else
        {
            upgradeTwoLevel = 1;
        }
        if (PlayerPrefs.GetInt("PlayerupgradeThreeLevel") != 0)
        {
            upgradeThreeLevel = PlayerPrefs.GetInt("PlayerupgradeThreeLevel");
        }
        else
        {
            upgradeThreeLevel = 1;
        }
    }
    private void Save()
    {
        PlayerPrefs.SetInt("PlayerupgradeOneLevel",upgradeOneLevel);
        PlayerPrefs.SetInt("PlayerupgradeTwoLevel",upgradeTwoLevel);
        PlayerPrefs.SetInt("PlayerupgradeThreeLevel",upgradeThreeLevel);
    }
    [Button]
    public void GiveMoney()
    {
        money.RuntimeValue = 1;
    }

    public override void UpgradeTwo()
    {
        if (upgradeTwoLevel == 5)
        {
            t_upgradeTwoCost.text = "MAX";
            return;
        }
        int calculateCost = (int)(upgradeTwoLevel * c_upgradeTwo * 1.6f);
        if (money.RuntimeValue > calculateCost)
        {
            upgradeTwoLevel++;
            t_upgradeTwoCost.text = (upgradeTwoLevel * c_upgradeTwo * 1.6f).ToString();
            money.RuntimeValue -= calculateCost;
            _inventoryManager.IncrementHeightCount();
            Save();
        }
    }

    public override void UpgradeThree()
    {
        if (upgradeThreeLevel == 5)
        {
            t_upgradeThreeCost.text = "MAX";
            return;

        }
        int calculateCost = (int)(upgradeThreeLevel * c_upgradeThree * 2f);
        if (money.RuntimeValue > calculateCost)
        {
            upgradeThreeLevel++;
            t_upgradeThreeCost.text = (upgradeThreeLevel * c_upgradeThree * 2f).ToString();
            money.RuntimeValue -= calculateCost;
            IncrementIncome();
            Save();
        }
    }

    private void UpgradeMovementSpeed()
    {
        _movementDataSo.ForwardMoveSpeed++;
        _movementDataSo.SideMoveSpeed++;
    }

    private void IncrementIncome()
    {
        foreach (var item in IncomeDatas)
        {
            item.SellValue += 5;
        }
    }
    
}