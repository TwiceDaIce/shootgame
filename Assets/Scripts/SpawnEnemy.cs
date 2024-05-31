using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] objectList;
    public GameObject EnemyPrefab;

    void Start()
    {
        StartCoroutine(SpawnEnemyWithDelay(5));
    }

    IEnumerator SpawnEnemyWithDelay(float secondsDelay)
    {
        while (true) // Infinite loop to continuously spawn enemies
        {
            yield return new WaitForSeconds(secondsDelay);

            int randomSpawnLocation = Random.Range(0, objectList.Length);
            Instantiate(EnemyPrefab, objectList[randomSpawnLocation].transform.position, Quaternion.identity);
        }
    }
}
