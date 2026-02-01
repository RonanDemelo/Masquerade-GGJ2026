using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : HealthClass
{

    public Image healthBar;

    protected override void Update()
    {
        base.Update();
        healthBar.fillAmount = currentHealth / baseHealth;
    }
    public void RestoreHealth()
    {
        currentHealth = baseHealth;
    }

    public override void Death()
    {
        base.Death();
        //this is a temp mod
        int score = AccoladeTracker.Instance.GetScore();
        int money = AccoladeTracker.Instance.GetMoney();
        Debug.Log($"Player Death Not Implemented. Score: {score}. Remaining Money: {money}");
    }
}
