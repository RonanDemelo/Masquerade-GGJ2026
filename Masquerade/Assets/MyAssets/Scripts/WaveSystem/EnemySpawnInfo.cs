using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;
    public int minWave = 1;

    //weight growth
    public float baseWeight = 1f;
    public float bonusWeightPerWave = 0.5f;
    public int growthDelayWaves = 2;

    //weight decay
    public int decayStartWaves = 6;
    public float decayPerWave = 0.5f;

    //min/max
    public float minWeight = 0.5f;
    public float maxWeight = 10f;
}