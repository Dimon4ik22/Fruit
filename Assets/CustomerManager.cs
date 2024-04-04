using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerManager : Singleton<CustomerManager>
{
    public Transform centerPoint;
    public float radius = 5f;
    public float minDistance = 2f;
    public GameObject enemyPrefab;
    public int numEnemies = 2;
    public bool canSpawnEnemies = false;

    public void SpawnEnemies(int count)
    {
        StartCoroutine(SpawnEnemy(count));
    }
    public IEnumerator SpawnEnemy(int count)
    {
        if (ShouldSpawn())
        {
            for (int i = 0; i < count; i++)
            {
                Run.After(Random.Range(1, 4), () =>
                {
                    Instantiate(enemyPrefab, centerPoint.transform.position, Quaternion.identity);
                });
                yield return new WaitForSeconds(2f);
            }
        }

    }

    private bool ShouldSpawn()
    {
        var count = FindObjectsOfType<Customer>().ToList().Count;
        if (count <= 5 && canSpawnEnemies)
        {
            return true;
        }
        return false;
    }

    public void CanStartSpawn()
    {
        canSpawnEnemies = true;
        StartCoroutine(SpawnEnemy(2));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(centerPoint.position, radius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(centerPoint.position, radius + minDistance);
    }
}