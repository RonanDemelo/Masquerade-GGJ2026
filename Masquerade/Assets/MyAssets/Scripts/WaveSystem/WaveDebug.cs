using UnityEngine;

public class WaveDebug : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            EnemyTemp enemy = FindFirstObjectByType<EnemyTemp>();
            if (enemy != null)
            {
                enemy.TakeDamage(99999);
            }
            // WaveManagement.Instance.EnemyDied();
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            WaveManagement.Instance.StartNextWave();

        }
    }
}
