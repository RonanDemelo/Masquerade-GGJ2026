using UnityEngine;

public class HitBox : MonoBehaviour
{
    public EnemyHealth enemyHealth;
    public void OnRaycastHit(PlayerAttack weapon)
    {
        enemyHealth.TakeDamage(weapon.damage);
        Debug.Log(weapon.damage);
    }
}
