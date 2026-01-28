using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : CombatCharacter
{
    private PlayerInputActions inputActions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputActions = new PlayerInputActions();
        inputActions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        var _input = inputActions.Combat;
        if (_input.Shoot.IsPressed())
        {
            attack.RangedAttack();
        }
        else if (_input.Punch.IsPressed())
        {
            attack.MeleeAttack();
        }    
    }
}
