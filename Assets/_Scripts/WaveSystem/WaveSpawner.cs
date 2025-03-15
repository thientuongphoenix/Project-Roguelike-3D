using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private float countdown;
    [SerializeField] private GameObject spawnPoint;

    public Wave[] waves;

    public int currentWaveIndex = 0;

    private bool readyToCountdown;

    private void Start()
    {
        readyToCountdown = true;

        for(int i = 0; i < waves.Length; i++)
        {
            waves[i].enemyLeft = waves[i].enemies.Length;
        }
    }

    private void Update()
    {
        if(currentWaveIndex >= waves.Length)
        {
            Debug.Log("Hết Wave rồi, Thắng rồi!");
            return;
        }

        if (readyToCountdown == true)
        {
            countdown -= Time.deltaTime;
        }

        if (countdown <= 0)
        {
            readyToCountdown = false;

            countdown = waves[currentWaveIndex].timeToNextWave;
            StartCoroutine(SpawnWave());
        }

        // Nếu không còn con enemy nào sống thì đi tiếp tới wavw sau
        if (waves[currentWaveIndex].enemyLeft == 0)
        {
            readyToCountdown = true;

            currentWaveIndex++;
        }
    }

    private IEnumerator SpawnWave()
    {
        if(currentWaveIndex < waves.Length)
        {
            for (int i = 0; i < waves[currentWaveIndex].enemies.Length; i++)
            {
                Enemy enemy = Instantiate(waves[currentWaveIndex].enemies[i], spawnPoint.transform);

                enemy.transform.SetParent(spawnPoint.transform);

                yield return new WaitForSeconds(waves[currentWaveIndex].timeToNextEnemy);
            }
        }
        
    }
}

[System.Serializable]
public class Wave
{
    public Enemy[] enemies;
    public float timeToNextEnemy;
    public float timeToNextWave;

    [HideInInspector] public int enemyLeft; //Số lượng enemy còn sống, số này về 0 sẽ kích hoạt thời gian đếm ngược cho wave tiếp theo
}