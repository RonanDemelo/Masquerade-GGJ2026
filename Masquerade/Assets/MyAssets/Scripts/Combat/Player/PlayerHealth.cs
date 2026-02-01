using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : HealthClass
{

    public Image healthBar;
    public GameObject gameOverHud;

    private void Awake()
    {
        Time.timeScale = 1f;
    }
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
        gameOverHud.SetActive(true);
        Debug.Log($"Player Death Not Implemented. Score: {score}. Remaining Money: {money}");
    }
}
