using System;
using UnityEngine;

/// <summary>
/// Base class for storing attack stats, attack type, and logic
/// </summary>
public class AttackClass : MonoBehaviour
{
    CombatCharacter character;
    public float damage = 1;
    public AttackType attackType = AttackType.None;

    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected float meleeRange = 0;

    public enum AttackType
    {
        None,
        Melee,
        Ranged
    }

    private void Awake()
    {
        character = GetComponent<CombatCharacter>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void MeleeAttack()
    {
        float maxDistance;
        if (attackType == AttackType.Ranged) maxDistance = meleeRange;
        else maxDistance = Mathf.Infinity;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxDistance, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log(hit.collider);
        }
    }

    public virtual void RangedAttack()
    {
        Debug.LogError("Ranged attack not implemented yet");
    }
}
