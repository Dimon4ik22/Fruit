using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using HyperCasualPack;
using HyperCasualPack.Pickables;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum CustomerCarrierType
{
    OnHand,
    OnBasket
}

public enum CustomerState
{
    GoToShelf,
    PickUpItems,
    GoToCashier,
    PayAndGo
}

public class Customer : MonoBehaviour
{
    [FoldoutGroup(("UI stuff"),expanded: true)]
    [SerializeField] private Image destinationShelfPng;
    [SerializeField] private Canvas pngCanvas;
    [FoldoutGroup(("UI stuff"))]
    [SerializeField] private TMP_Text quantityText;
    [FoldoutGroup(("UI stuff"))]
    [SerializeField] private Image GoToCashierPng;
    [FoldoutGroup(("Customer Components"),expanded:true)]
    [SerializeField] private Animator animator;
    [FoldoutGroup("Customer Components")]
    [SerializeField] private InventoryManager inventory;
    [FoldoutGroup("Customer Components")]
    [SerializeField] private Transform cardboardMovePos;
    [FoldoutGroup("Customer Components")]
    public Transform TransformOfLine;
    [FoldoutGroup("Customer Components")]
    public NavMeshAgent agent;
    private MarketShelf _marketShelf;
    [FoldoutGroup(("Customer Bools"),expanded:true)]
    public bool readyToPay = false;
    [FoldoutGroup("Customer Bools")]
    public bool goCashier = false;
    [FoldoutGroup("Customer Bools")]
    public bool customerHasPaid = false;
    [FoldoutGroup(("Customer Enums"),expanded:true)]
    public CustomerState CustomerState;
    [FoldoutGroup("Customer Enums")]
    public CustomerCarrierType CarrierType;
    [FoldoutGroup(("Customer Lists"),expanded:true)]
    [SerializeField] private List<Pickable> collectedPickables;
    private int totalCollectedCount;
    private int amountToCollect;
    private Action OnArrivedCashier;
    private void Start()
    {
        collectedPickables = new List<Pickable>();
        _marketShelf = GetMarketShelf();
        agent.SetDestination(_marketShelf.transform.position);
        CustomerState = CustomerState.GoToShelf;
        destinationShelfPng.sprite = _marketShelf.marketShelfPng;
        SetRandomQuantitiy();
        SetWalkAnimation(1, 1f);
    }

    public void SetWalkAnimation(float value, float duration)
    {
        float initialValue = animator.GetFloat("WalkSpeed");
        DOTween.To(() => initialValue, x => animator.SetFloat("WalkSpeed", x), value, duration);
    }

    private void SetRandomQuantitiy()
    {
        var random = Random.Range(1, 6);
        amountToCollect = random;
        quantityText.text = random.ToString();
    }

    private void ExtractTxt()
    {
        quantityText.text = (amountToCollect - totalCollectedCount).ToString();
    }

    private void Update()
    {
        switch (CustomerState)
        {
            case CustomerState.GoToShelf:
                float remainingDistance = CalculateRemainingDistance();
                if (remainingDistance <= 2)
                {
                    agent.isStopped = true;
                    CustomerState = CustomerState.PickUpItems;
                }
                break;
            case CustomerState.PickUpItems:
                WaitForPickUp();
                break;
            case CustomerState.GoToCashier:
                if (!goCashier)
                {
                    goCashier = true;
                    GoToCashierLine();
                }
                float remainingDistanceCashier = CalculateRemainingDistance();
                if (remainingDistanceCashier <= 0.5f)
                {
                    //TURN TOWARDS CASHIER
                    CashierManager.Instance.Cashier.hasCompleted = false;
                    OnArrivedCashier?.Invoke();
                }
                break;
            case CustomerState.PayAndGo:
                break;
        }
    }

    public void IsFirstOnLine(Transform linePos)
    {

        OnArrivedCashier += OnArrivedCashiers;
        if (CashierManager.Instance.Cashier.IsFirstOnLine(linePos))
        {
            OnArrivedCashier += RotationFirstLine;
            OnArrivedCashier += () => { readyToPay = true; };
        }
        else
        {
            OnArrivedCashier += RotationBehindLine;

        }
    }

    private void OnArrivedCashiers()
    {
        agent.isStopped = true;
        SetWalkAnimation(0,0.1f);
        animator.SetBool("Carrying",true);       
    }
    private void RotationBehindLine()
    {
        transform.DORotate(new Vector3(0,-90,0), 0.5f);
    }

    private void RotationFirstLine()
    {
        transform.DORotate(new Vector3(0,-180,0), 0.5f);
        
    }

    private void WaitForPickUp()
    {
        SetWalkAnimation(0, 0.1f);
        if (totalCollectedCount < amountToCollect)
        {
            var shelfSlot = _marketShelf.GiveCustomerItem();
            if (shelfSlot != null)
            {
                var pickable = shelfSlot.Pickable;
                if (pickable != null)
                {
                    shelfSlot.isAvailable = true;
                    shelfSlot.Pickable = null;
                    
                    inventory.AddPickable(pickable);
                    collectedPickables.Add(pickable); 
                    totalCollectedCount++;
                    ExtractTxt();

                }
            }
            
        }

        if (totalCollectedCount == amountToCollect && !CashierManager.Instance.Cashier.IsCapacityEmpty())
        {
            ChangeSprite();
        }
        if (totalCollectedCount == amountToCollect && CashierManager.Instance.Cashier.IsCapacityEmpty())
        {
            ChangeSprite();
            CustomerState = CustomerState.GoToCashier;
        }
    }
    
    private float CalculateRemainingDistance()
    {
        if (agent.hasPath)
        {
            float distance = Vector3.Distance(transform.position, agent.destination);
            return distance;
        }
        return 0f;
    }

    private void GoToCashierLine()
    {
        Run.After(Random.Range(1f, 2f), () =>
        {
            SetWalkAnimation(1,0.1f);
            animator.SetBool("Carrying",true);
            if (CashierManager.Instance.Cashier.IsCapacityEmpty())
            {
                TransformOfLine = CashierManager.Instance.Cashier.AddCustomer(this);
                IsFirstOnLine(TransformOfLine);
                agent.isStopped = false;
                agent.SetDestination(TransformOfLine.position);
            }
            else
            {
                goCashier = false;
            }
        });
        
    }
    private MarketShelf GetMarketShelf()
    {
        var allPossibleShelves = FindObjectsOfType<MarketShelf>().ToList();
        var getRandomShelf = Random.Range(0, allPossibleShelves.Count);
        return allPossibleShelves[getRandomShelf];
    }

    public IEnumerator DoPackageItems(Cardboard cardboard,CashierMoneySpawner cashierMoneySpawner)
    {
        CustomerState = CustomerState.PayAndGo;
        var allPickables = GetComponentsInChildren<Pickable>().ToList();
        int moneyToGive = 0;
        for (int i = 0; i < allPickables.Count; i++)
        {
            moneyToGive += allPickables[i].GetSellValue();
            cardboard.slotsInBox = cardboard.SlotToUse(allPickables[0]);
            cardboard.MoveToSlot(allPickables[i]);
            yield return new WaitForSeconds(0.25f);
        }

        yield return new WaitForSeconds(0.25f);
        cardboard.GetComponent<Animator>().SetBool("CloseBox",true);
        cashierMoneySpawner.SpawnXAmount(moneyToGive);
        transform.DORotate(new Vector3(0, -90, 0), 0.5f);
        yield return new WaitForSeconds(1.5f);
        cardboard.transform.parent = cardboardMovePos;
        cardboard.transform.Jump(cardboardMovePos, 2, 1, 0.5f);
        cardboard.transform.DORotate(new Vector3(0, 90, 0), 0.5f);
        yield return new WaitForSeconds(1f);
        agent.isStopped = false;
        pngCanvas.gameObject.SetActive(false);
        agent.SetDestination(CashierManager.Instance.customerExitPosition.position);
        SetWalkAnimation(1, 1f);
        CashierManager.Instance.Cashier.LeaveFirstLine(this);
        CashierManager.Instance.Cashier.CheckPossibleForwardMove();

        yield return new WaitForSeconds(4f);
        CustomerManager.Instance.SpawnEnemies(Random.Range(1,3));
        Destroy(gameObject);

    }
    
    private void ChangeSprite()
    {
        quantityText.gameObject.transform.parent.gameObject.SetActive(false);
        GoToCashierPng.gameObject.SetActive(true);
    }
}