using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HyperCasualPack;
using HyperCasualPack.ScriptableObjects;
using UnityEngine;

public class UpgradeHR : UpgradeBase
{
    
    protected override string s_upgradeOne { get; set; }
    protected override string s_upgradeTwo { get; set; }
    protected override string s_upgradeThree { get; set; }
    protected override int c_upgradeOne { get; set; }
    protected override int c_upgradeTwo { get; set; }
    protected override int c_upgradeThree { get; set; }

    [SerializeField] MovementDataSO _movementDataSo;
    public GameObject Employee;
    public Transform EmployeeSpawnPosition;
    int countOfEmployee = 0;
    void Start()
    {
        s_upgradeOne = "MoveSpeedEmployee";
        s_upgradeTwo = "CapacityEmployee";
        s_upgradeThree = "SpawnEmployee";

        Load();
        c_upgradeOne = 150 * upgradeOneLevel;
        c_upgradeTwo = 180 * upgradeTwoLevel;
        c_upgradeThree = 250 * upgradeThreeLevel;
        
        t_upgradeOneCost.text = (  c_upgradeOne * 1.5f).ToString();
        t_upgradeTwoCost.text = (  c_upgradeTwo * 1.6f).ToString();
        t_upgradeThreeCost.text = (  c_upgradeThree * 2).ToString();
        if (upgradeOneLevel == 3)
        {
            t_upgradeOneCost.text = "MAX";
        }
        if (upgradeThreeLevel == 3)
        {
            t_upgradeTwoCost.text = "MAX";

        }
        if (upgradeThreeLevel == 3)
        {
            t_upgradeThreeCost.text = "MAX";

        }
        if (upgradeThreeLevel > 1)
        {
            for (int i = 0; i < upgradeThreeLevel; i++)
            {
                Instantiate(Employee, EmployeeSpawnPosition.position, Quaternion.identity);

            }
        }
    }
    public override void UpgradeOne()
    {
        if (upgradeOneLevel == 3)
        {
            t_upgradeOneCost.text = "MAX";
            return;

        }
        int calculateCost = (int)(upgradeOneLevel * c_upgradeOne * 1.5f);
        if (money.RuntimeValue > calculateCost)
        {
            upgradeOneLevel++;
            t_upgradeOneCost.text = (upgradeOneLevel * c_upgradeOne * 1.5f).ToString();
            money.RuntimeValue -= calculateCost;
            UpgradeMovementSpeed();
            Save();
        }
    }

    public override void UpgradeTwo()
    {
        if (upgradeThreeLevel == 3)
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
            IncreaseCapacityOfAllEmployees();
            Save();
        }
    }

    public override void UpgradeThree()
    {
        if (upgradeThreeLevel == 3)
        {
            t_upgradeThreeCost.text = "MAX";
            return;

        }
        if (countOfEmployee < 5)
        {
            int calculateCost = (int)(upgradeThreeLevel * c_upgradeThree * 2f);
            if (money.RuntimeValue > calculateCost)
            {
                upgradeThreeLevel++;
                t_upgradeThreeCost.text = (upgradeThreeLevel * c_upgradeThree * 2f).ToString();
                money.RuntimeValue -= calculateCost;
                Instantiate(Employee, EmployeeSpawnPosition.position, Quaternion.identity);
                countOfEmployee++;
                Save();
            }
        }
        else
        {
            t_upgradeThreeCost.text = "MAX";
        }

    }

    private void Load()
    {
        if (PlayerPrefs.GetInt("HRupgradeOneLevel") != 0)
        {
            upgradeOneLevel = PlayerPrefs.GetInt("HRupgradeOneLevel");
        }
        else
        {
            upgradeOneLevel = 1;
        }
        if (PlayerPrefs.GetInt("HRupgradeTwoLevel") != 0)
        {
            upgradeTwoLevel = PlayerPrefs.GetInt("HRupgradeTwoLevel");
        }
        else
        {
            upgradeTwoLevel = 1;
        }
        if (PlayerPrefs.GetInt("HRupgradeThreeLevel") != 0)
        {
            upgradeThreeLevel = PlayerPrefs.GetInt("HRupgradeThreeLevel");
        }
        else
        {
            upgradeThreeLevel = 1;
        }
    }
    private void Save()
    {
        PlayerPrefs.SetInt("HRupgradeOneLevel",upgradeOneLevel);
        PlayerPrefs.SetInt("HRupgradeTwoLevel",upgradeTwoLevel);
        PlayerPrefs.SetInt("HRupgradeThreeLevel",upgradeThreeLevel);
    }
    private void IncreaseCapacityOfAllEmployees()
    {
        var tempList = FindObjectsOfType<Employee>().ToList();
        foreach (var item in tempList)
        {
            item.GetComponent<InventoryManager>().IncrementHeightCount();
        }
    }
    private void UpgradeMovementSpeed()
    {
        _movementDataSo.ForwardMoveSpeed++;
        _movementDataSo.SideMoveSpeed++;
    }
}
