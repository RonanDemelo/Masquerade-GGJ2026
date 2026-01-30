using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : CombatCharacter
{
    private PlayerInputActions inputActions;
    private IEnumerator ContinueShooting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputActions = new PlayerInputActions();
        inputActions.Enable();

        ContinueShooting = attack.GetComponent<PlayerAttack>().ContinueShooting(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var _input = inputActions.Combat;

        _input.Shoot.performed += ctx => StartCoroutine(ContinueShooting);
        _input.Shoot.canceled += ctx => StopCoroutine(ContinueShooting);

        if (_input.Punch.IsPressed())
        {
            attack.MeleeAttack();
        }    
    }
}
