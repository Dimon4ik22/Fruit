using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Sirenix.OdinInspector;
using UnityEngine;

public class Cashier : MonoBehaviour
{
    public List<Customer> CustomerLine;
    public CashierMoneySpawner CashierMoneySpawner;
    public List<Transform> cashierLineTransforms;
    [SerializeField] private GameObject cardboardBox;
    [SerializeField] private Transform cardboardBoxSpawnTransform;
    [ShowInInspector] private int maximumCountOfLine = 3;
    public bool hasCompleted = false;
    public bool cashierAIHasUnlocked = false;
    public CashierAI CashierAI;
    private void Awake()
    {
        CustomerLine = new List<Customer>(maximumCountOfLine);
    }

    void OnEnable()
    {
        if(PlayerPrefs.GetInt("Cashier") == 1)
            CashierAI.gameObject.SetActive(true);
    }

    public Transform AddCustomer(Customer customer)
    {
        if (!CustomerLine.Contains(customer))
        {
            CustomerLine.Add(customer);
            var index = CustomerLine.IndexOf(customer);
            return cashierLineTransforms[index];
        }
        return null;
    }

    public bool IsFirstOnLine(Transform transform)
    {
        if (cashierLineTransforms.Contains(transform))
        {
            var index = cashierLineTransforms.IndexOf(transform);
            if (index == 0)
            {
                return true;
            }
        }

        return false;
    }

    public void LeaveFirstLine(Customer customer)
    {
        CustomerLine.RemoveAt(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (other.transform.CompareTag("Player"))
        // {
        //     if (CustomerLine.Count > 0)
        //     {
        //         if (!hasCompleted)
        //         {
        //             hasCompleted = true;
        //
        //             CheckCustomer();
        //         }
        //     }
        // }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!cashierAIHasUnlocked)
        {
            if (other.transform.CompareTag("Player"))
            {
                if (CustomerLine.Count > 0)
                {
                    if (!hasCompleted)
                    {
                        hasCompleted = true;

                        CheckCustomer();
                    }
                }
            }
        }
        else
        {
            if (other.transform.CompareTag("CashierAI"))
            {
                if (CustomerLine.Count > 0)
                {
                    if (!hasCompleted)
                    {
                        hasCompleted = true;

                        CheckCustomer();
                    }
                }
            }
        }

    }

    private void CheckCustomer()
    {
        var customer = CustomerLine[0];
        if (customer != null && customer.readyToPay && !customer.customerHasPaid)
        {
            switch (customer.CarrierType)
            {
                case CustomerCarrierType.OnHand:
                    customer.customerHasPaid = true;
                    GameObject go = Instantiate(cardboardBox, cardboardBoxSpawnTransform.position, Quaternion.identity);
                    Cardboard cardboard = go.GetComponent<Cardboard>();
                    Run.After(1, () =>
                    {
                        StartCoroutine(customer.DoPackageItems(cardboard,CashierMoneySpawner));
                    });
                    break;
                case CustomerCarrierType.OnBasket:
                    break;
            }
        }
    }

    public void CheckPossibleForwardMove()
    {
        foreach (var customer in CustomerLine)
        {
            var index = CustomerLine.IndexOf(customer);
            var indexTransform = cashierLineTransforms.IndexOf(customer.TransformOfLine);
            if (index < indexTransform)
            {
                customer.agent.isStopped = false;
                customer.SetWalkAnimation(1,0.3f);
                customer.TransformOfLine = cashierLineTransforms[indexTransform - 1];
                customer.agent.SetDestination(cashierLineTransforms[indexTransform - 1].position);
                if (IsFirstOnLine(customer.TransformOfLine))
                {
                    customer.SetWalkAnimation(0,0.1f);
                    customer.readyToPay = true;
                }
            }
        }

    }

    public bool IsCapacityEmpty()
    {
        return CustomerLine.Count < maximumCountOfLine ? true : false;
    }

    public void RemoveCustomer(Customer customer)
    {
        if (CustomerLine.Count > 0)
        {
            CustomerLine.Remove(customer);
        }
    }
}