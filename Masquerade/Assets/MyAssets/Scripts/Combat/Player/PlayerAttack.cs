using UnityEngine;

public class PlayerAttack : AttackClass
{
    public PlayerCamera cameraClass;

    public override void MeleeAttack()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, cameraClass.gameObject.transform.TransformDirection(Vector3.forward), out hit, meleeRange, layerMask))
        {
            Debug.DrawRay(transform.position, cameraClass.gameObject.transform.TransformDirection(Vector3.forward) * hit.distance, Color.blue);
            hit.collider.GetComponent<EnemyCombat>().health.TakeDamage(damage);
        }
    }
}
