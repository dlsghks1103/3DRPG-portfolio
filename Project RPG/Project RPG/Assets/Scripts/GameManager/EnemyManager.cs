using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public Transform[] spawnPoints;
    
    public GameObject enemyPrefab;

    public float respawnTime = 0f;
    public int maxEnemy = 5;

    void Start()
    {
        spawnPoints = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();

        if (spawnPoints.Length > 0)
        {
            StartCoroutine(CreateEnemy2());
        }
    }

    IEnumerator CreateEnemy()
    {
        while (true)
        {
            int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

            if (enemyCount <1)
            {
                yield return new WaitForSeconds(respawnTime);

                for (int i = 1; i <= spawnPoints.Length; i++) 
                {
                    Vector3 setPoint = new Vector3(spawnPoints[i].position.x, 0, spawnPoints[i].position.z);

                    Instantiate(enemyPrefab, setPoint, spawnPoints[i].rotation);
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator CreateEnemy2()
    {
        while (true)
        {
            int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

            if (enemyCount < maxEnemy)
            {
                yield return new WaitForSeconds(respawnTime);

                int i = Random.Range(1, spawnPoints.Length);

                Vector3 setPoint = new Vector3(spawnPoints[i].position.x, 0, spawnPoints[i].position.z);

                Instantiate(enemyPrefab, setPoint, spawnPoints[i].rotation);
            }
            else
            {
                yield return null;
            }
        }
    }


}
