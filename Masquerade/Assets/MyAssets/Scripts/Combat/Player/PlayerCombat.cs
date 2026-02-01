using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : CombatCharacter
{
    private PlayerInputActions inputActions;
    private IEnumerator ContinueShooting;
    bool isPunch = false;
    [Header("Firendly")]
    public MaskType.MaskColour maskColour;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputActions = new PlayerInputActions();
        inputActions.Enable();

        ContinueShooting = attack.GetComponent<PlayerAttack>().ContinueShooting(true);
    }

    // Update is called once per frame
    void Update()
    {
        var _input = inputActions.Combat;
        if (Time.timeScale <= 0) return;
        _input.Shoot.performed += ctx => StartCoroutine(ContinueShooting);
        _input.Shoot.canceled += ctx => StopCoroutine(ContinueShooting);

        if (_input.Punch.IsPressed())
        {
            if (isPunch) return;
            StartCoroutine(attack.GetComponent<PlayerAttack>().MeleeAttackRoutine());
            isPunch = true;
        }
        else
        {
            isPunch = false;
        }
    }
}
