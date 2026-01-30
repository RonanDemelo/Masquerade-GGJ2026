using UnityEngine;

public class PlayerHealth : HealthClass
{
    public override void Death()
    {
        base.Death();
        //this is a temp mod
        int score = AccoladeTracker.Instance.GetScore();
        int money = AccoladeTracker.Instance.GetMoney();
        Debug.Log($"Player Death Not Implemented. Score: {score}. Remaining Money: {money}");
    }
}
