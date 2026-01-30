using UnityEngine;

public class WaveDebug : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            AIAgent enemy = FindFirstObjectByType<AIAgent>();
            // WaveManagement.Instance.EnemyDied();
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            WaveManagement.Instance.StartNextWave();

        }
    }
}
