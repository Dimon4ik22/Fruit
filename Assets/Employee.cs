using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using HyperCasualPack;
using HyperCasualPack.Pickables;
using HyperCasualPack.Pools;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum EmployeeState
{
    WAIT,
    COLLECT,
    DELIVER
}

public enum DeliveryState
{
    MARKETSHELF,
    FACTORY
}

public enum JobTypes
{
    NONE,
    TREE,
    MARKETSHELF,
    FACTORY
}

public class Employee : MonoBehaviour
{
    public EmployeeState EmployeeState;
    public DeliveryState DeliveryState;
    [SerializeField] private List<PickableTreeSpawner> treeObjects;
    [SerializeField] private List<MarketShelf> marketShelves;
    [SerializeField] private List<Factory> Factories;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private JobTypes assignedJob;
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] PickablePoolerSO cocaoPod;
    private PickableTreeSpawner jobAssignedTree;
    private Factory jobAssignedFactory;
   [SerializeField] Factory chocolateFactory;
    public bool collectingSeed = false;
    bool droppingFactoryItem = false;
    private void Start()
    {
        assignedJob = JobTypes.NONE;
        treeObjects = FindObjectsOfType<PickableTreeSpawner>().ToList();
        marketShelves = FindObjectsOfType<MarketShelf>().ToList();
        Factories = FindObjectsOfType<Factory>().ToList();
        GetChocolateFactory();
        
    }

    private void FixedUpdate()
    {
        if (assignedJob == JobTypes.NONE)
        {
            assignedJob = SearchJob();
            SetWalkAnimation(0,0);
        }
        if (assignedJob != JobTypes.NONE)
        {
            switch (assignedJob)
            {
                case JobTypes.TREE:
                    if (!collectingSeed)
                    {
                        SetDestination(jobAssignedTree.transform);
                        StartCoroutine(CollectCocaoEvent());
                    }
                    break;
                case JobTypes.FACTORY:
                    if (!droppingFactoryItem)
                    {
                        SetDestination(jobAssignedFactory.FactoryCollectTransform.transform);
                        StartCoroutine(CollectFromFactoryDeliverShelf());
                    }
                    break;
            }
        }
    }

    private IEnumerator CollectFromFactoryDeliverShelf()
    {
        while (true)
        {
            droppingFactoryItem = true;
            if (CalculateRemainingDistance() <= 1 && agent.hasPath)
            {
                StartCoroutine(jobAssignedFactory.GetComponentInChildren<PickableSourceCollector>().AI_Collect(_inventoryManager));
                StartCoroutine(GoToMarketShelf());
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator  GoToMarketShelf()
    {
        while (true)
        {
            SetDestination(jobAssignedFactory.MarketShelfToGo.transform);
            if (CalculateRemainingDistance() <= 1 && agent.hasPath)
            {
                StartCoroutine(jobAssignedFactory.MarketShelfToGo.GiveToAI(_inventoryManager));
                droppingFactoryItem = false;
                assignedJob = JobTypes.NONE;
                yield break;
            }
            yield return null;
        }

    }

    private IEnumerator CollectCocaoEvent()
    {
        while (true)
        {
            collectingSeed = true;
            if (CalculateRemainingDistance() <= 1 && agent.hasPath)
            {
                jobAssignedTree._pickableTreeCollector.GiveToAI(_inventoryManager);
                var random = Random.Range(0, 2);
                if(random == 0)
                    StartCoroutine(GiveSeedToFactory());
                if (random == 1)
                    StartCoroutine(GiveSeedToMarketShelf());
                yield break;
            }
            yield return null;
        }

    }

    IEnumerator GiveSeedToMarketShelf()
    {
        while (true)
        {
            MarketShelf cocaoShelf = FindMarketShelf();
            if (cocaoShelf != null)
            {
                SetDestination(cocaoShelf.transform);
                if (CalculateRemainingDistance() <= 1 && agent.hasPath)
                {
                    StartCoroutine(cocaoShelf.GiveToAI(_inventoryManager));
                    collectingSeed = false;
                    assignedJob = JobTypes.NONE;
                    yield break;
                }    
            }
            yield return null;
        }
    }

    private IEnumerator GiveSeedToFactory()
    {
        while (true)
        {
            SetDestination(chocolateFactory.FactoryCollectTransform.transform);
            if (CalculateRemainingDistance() <= 1 && agent.hasPath)
            {
                StartCoroutine(chocolateFactory.ReturnStockPiler().CollectFromAI(_inventoryManager));
                collectingSeed = false;
                assignedJob = JobTypes.NONE;
                yield break;
            }
            yield return null;
        }
    }
    private void GetChocolateFactory()
    {
        for (int i = 0; i < Factories.Count; i++)
        {
            if (Factories[i].ReturnStockPiler()._ruleset.InputPoolerSO == cocaoPod)
            {
                chocolateFactory = Factories[i];
            }
        }
    }
    private Transform CheckChocolateFactory()
    {
        for (int i = 0; i < Factories.Count; i++)
        {
            if (!Factories[i].ReturnStockPiler().IsUnmodifiedItemCapacityFull && Factories[i].ReturnStockPiler()._ruleset.InputPoolerSO == cocaoPod)
            {
                chocolateFactory = Factories[i];
                return Factories[i].GetComponent<FactoryAI>().FactoryCollectPoint;
            }
        }

        return null;
    }

    private MarketShelf FindMarketShelf()
    {
        for (int i = 0; i < marketShelves.Count;)
        {
            if (marketShelves[i].neededPickablePool == cocaoPod && marketShelves[i].CanPlaceInMarket())
            {
                return marketShelves[i];
            }
            else
            {
                return null;
            }
        }

        return null;
    }

    private void CollectModifiedCurrentFactoryItems()
    {
        StartCoroutine(jobAssignedFactory.GetComponentInChildren<PickableSourceCollector>().AI_Collect(_inventoryManager));
    }

    private JobTypes SearchJob()
    {
        foreach (var item in Factories)
        {
            if (item.ReturnStockPiler().spawnedPickables.Count > 0)
            {
                jobAssignedFactory = item;
                return JobTypes.FACTORY;
            }
        }


        foreach (var item in treeObjects)
        {
            if (item.HasUncollectedSeeds() && item.isCocaoSeed)
            {
                jobAssignedTree = item;
                return JobTypes.TREE;
            }
        }

        return JobTypes.NONE;
    }

    private bool IsInventoryEmpty()
    {
        return _inventoryManager.IsInventoryEmpty();
    }

    private MarketShelf FindTargetMarketShelf()
    {
        foreach (var item in marketShelves)
        {
            if (item.CheckForAI(jobAssignedTree.GetPool()))
            {
                EmployeeState = EmployeeState.DELIVER;
                return item;
            }
        }

        return null;
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

    public void SetWalkAnimation(float value, float duration)
    {
        float initialValue = animator.GetFloat("WalkSpeed");
        DOTween.To(() => initialValue, x => animator.SetFloat("WalkSpeed", x), value, duration);
    }

    public void SetDestination(Transform destination)
    {
        agent.isStopped = false;
        SetWalkAnimation(1, 1);
        agent.SetDestination(destination.position);
    }
}