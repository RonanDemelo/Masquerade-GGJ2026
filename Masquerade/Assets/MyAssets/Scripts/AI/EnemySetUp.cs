using UnityEngine;

public class EnemySetUp : MonoBehaviour
{
    public EnemyHealth enemyHealth;
    public EnemyAttack enemyAttack;
    public int scoreValue;
    public int minMoneyValue = 0;
    public int maxMoneyValue = 10;

    public void Initialize(float _waveModifier)
    {
        if (enemyHealth == null) return;
        if (enemyAttack == null) return;

        enemyHealth.baseHealth *= _waveModifier;
        enemyAttack.damage *= _waveModifier;
    }


    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        enemyAttack = GetComponent<EnemyAttack>();

        if (enemyHealth == null) return;
        enemyHealth.scoreValue = scoreValue;
        enemyHealth.moneyValue = Random.Range(minMoneyValue, maxMoneyValue);
    }
}
