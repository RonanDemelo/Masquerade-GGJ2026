using UnityEngine;

public class WaveDebug : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            EnemyHealth enemy = FindFirstObjectByType<EnemyHealth>();
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


        if (Input.GetKeyDown(KeyCode.H))
        {
            FindAnyObjectByType<PlayerHealth>().TakeDamage(999999999999999);
        }
    }
}
