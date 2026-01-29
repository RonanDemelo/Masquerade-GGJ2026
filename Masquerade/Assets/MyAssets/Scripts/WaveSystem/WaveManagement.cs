using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class WaveManagement : MonoBehaviour
{

    public static WaveManagement Instance { get; private set; }

    //wave tracker
    public int currentWave = 0;
    public float waveModifier = 1.0f;
    public float modifierIncreasePerWave = 0.2f;

    public int baseEnemyCount = 5;
    public int enemyIncreasePerWave = 3; //this is just a default value, will be math

    //enemies
    public List<EnemySpawnInfo> enemyTypes;
    public GameObject spawnpoint;
    [SerializeField] private Transform[] spawnPoints;

    private int m_enemiesAlive = 0;
    private bool m_waveActive = false;

    public GameObject waveBar;
    public TMP_Text waveBarText;
    public GameObject wavePrompt;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        waveBarText = waveBar.GetComponent<WaveBar>().waveBarText;
        spawnPoints =  spawnpoint.GetComponentsInChildren<Transform>();
        Instance = this;
    }

    public void StartNextWave()
    {
        if (m_waveActive) return;

        currentWave++;
        waveModifier = 1.0f + (currentWave - 1) * modifierIncreasePerWave;

        int enemiesToSpawn = baseEnemyCount + (currentWave -1) * enemyIncreasePerWave; // again this is temp!

        waveBarText.text = $"Wave {currentWave} start!";
        wavePrompt.SetActive(false);
        waveBar.SetActive(true);

        StartCoroutine(SpawnWave(enemiesToSpawn));
    }

    public void EnemyDied()
    {
        m_enemiesAlive--;
        if(m_enemiesAlive <= 0)
        {
            EndWave();
        }
    }

    IEnumerator SpawnWave(int _count)
    {
        m_waveActive = true;

        for(int i = 0; i < _count; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f); // just a small delay to try and make it so enemies don't all spawn on top of eachother
        }
    }

    void SpawnEnemy()
    {
        EnemySpawnInfo spawnInfo = GetRandomEnemyForWave();
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemy = Instantiate(spawnInfo.enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemy.GetComponent<EnemyTemp>().Initialize(waveModifier);

        m_enemiesAlive++;
    }


    EnemySpawnInfo GetRandomEnemyForWave()
    {
        float totalweight = 0f;
        List<(EnemySpawnInfo enemy, float weight)> weightedList = new List<(EnemySpawnInfo enemy, float weight)>();

        foreach(var enemy in enemyTypes)
        {
            float weight = CalculateWeight(enemy);
            if (weight <= 0f) continue;

            weightedList.Add((enemy, weight));
            totalweight += weight;
        }

        float roll = Random.Range(0, totalweight);
        float cumulative = 0f;

        foreach(var entry in weightedList)
        {
            cumulative += entry.weight;
            if (roll <= cumulative)
                return entry.enemy;
        }

        return weightedList[0].enemy;
    }

    float CalculateWeight(EnemySpawnInfo enemy)
    {
        int wavesSinceUnlock = currentWave - enemy.minWave;
        if (wavesSinceUnlock < 0)
            return 0f;

        float weight = enemy.baseWeight;

        // decay phase
        if (wavesSinceUnlock >= enemy.growthDelayWaves)
        {
            int growthWaves = wavesSinceUnlock - enemy.growthDelayWaves + 1;
            weight += enemy.bonusWeightPerWave * growthWaves;
        }

        //decay phase
        if (wavesSinceUnlock >= enemy.decayStartWaves)
        {
            int decayWaves = wavesSinceUnlock - enemy.decayStartWaves + 1;
            weight -= enemy.decayPerWave * decayWaves;
        }

        return Mathf.Clamp(weight, enemy.minWeight, enemy.maxWeight);
    }

    void EndWave()
    {
        m_waveActive = false;
        waveBarText.text = $"Wave {currentWave} complete!";
        wavePrompt.SetActive(true);
        waveBar.SetActive(true);

        //TODO
        //enable end of round stuff here
    }

}
