using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region Variables
    public Transform[] spawnPoints;
    
    public GameObject[] enemyPrefab;

    public float respawnTime = 0f;
    public int maxEnemy = 6;
    public float spawnRange = 20;
    #endregion Variables

    #region Unity Methods
    void Start()
    {
        spawnPoints = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();

        if (spawnPoints.Length > 0)
        {
            StartCoroutine(CreateEnemy());
        }
    }
    #endregion Unity Methods

    #region Methods
    IEnumerator CreateEnemy()
    {
        while (true)
        {
            int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

            if (enemyCount < maxEnemy)
            {
                yield return new WaitForSeconds(respawnTime);
                SpawnEnemy1();
                SpawnEnemy2();
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

                //Instantiate(enemyPrefab, setPoint, spawnPoints[i].rotation);
            }
            else
            {
                yield return null;
            }
        }
    }

    private void SpawnEnemy1()
    {
        float pointX = Random.Range(spawnPoints[1].position.x - spawnRange, spawnPoints[1].position.x + spawnRange);
        float pointZ = Random.Range(spawnPoints[1].position.z - spawnRange, spawnPoints[1].position.z + spawnRange);

        Vector3 setPoint = new Vector3(pointX, 0, pointZ);

        Instantiate(enemyPrefab[0], setPoint, spawnPoints[0].rotation);
    }

    private void SpawnEnemy2()
    {
        float pointX = Random.Range(spawnPoints[2].position.x - spawnRange, spawnPoints[2].position.x + spawnRange);
        float pointZ = Random.Range(spawnPoints[2].position.z - spawnRange, spawnPoints[2].position.z + spawnRange);

        Vector3 setPoint = new Vector3(pointX, 0, pointZ);

        Instantiate(enemyPrefab[1], setPoint, spawnPoints[0].rotation);
    }
    #endregion Methods
}
