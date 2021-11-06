using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public enum EnemyTypes
{
    Base,
    Null
}

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnLocations;
    public Enemy[] enemyPrefab;
    public EnemyTypes[] enemyWave;
    public int waveCount;
    public int currentWave;
    public int enemyCount;
    public int currentEnemy;
    public float minSpawnTime, maxSpawnTime;
    IEnumerator enemySpawningFirst;
    IEnumerator enemySpawning;

    // Start is called before the first frame update
    void Awake()
    {
        waveCount = Random.Range(2, 5);
        WaveGenereate();
    }

    void WaveGenereate()
    {
        if (currentWave < waveCount)
        {
            if (enemySpawningFirst == null)
            {
                currentWave++;
                currentEnemy = 0;
                enemyCount = Random.Range(5, 10);
                enemyWave = new EnemyTypes[enemyCount];

                for (int i = 0; i < enemyCount; i++)
                {
                    EnemyTypes _enemyType;
                    _enemyType = (EnemyTypes)Random.Range(0, (int)EnemyTypes.Null);
                    enemyWave[i] = _enemyType;
                }

                enemySpawningFirst = FirstSpawn();
                StartCoroutine(enemySpawningFirst);
            }
            else
            {
                Debug.Log("woops first spawn is bugged");
            }
        }

    }

    IEnumerator FirstSpawn()
    {
        float _spawnTime = maxSpawnTime;
        int _spawnLoc = Random.Range(0, spawnLocations.Length);
        yield return new WaitForSeconds(_spawnTime);
        Instantiate(enemyPrefab[(int)enemyWave[currentEnemy]].gameObject, spawnLocations[_spawnLoc]);
        currentEnemy++;
        enemySpawningFirst = null;
        enemySpawning = EnemySpawn();
        StartCoroutine(enemySpawning);
    }


    IEnumerator EnemySpawn()
    {
        if (currentEnemy < enemyCount)
        {
            float _spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            int _spawnLoc = Random.Range(0, spawnLocations.Length);
            yield return new WaitForSeconds(_spawnTime);
            Instantiate(enemyPrefab[(int)enemyWave[currentEnemy]], spawnLocations[_spawnLoc]);
            currentEnemy++;
            StartCoroutine(EnemySpawn());
        }
        else
        {
            yield return new WaitForSeconds(maxSpawnTime);
            WaveGenereate();
        }
    }

}
