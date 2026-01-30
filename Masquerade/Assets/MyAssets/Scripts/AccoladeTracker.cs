using UnityEngine;

public class AccoladeTracker : MonoBehaviour
{

    public static AccoladeTracker Instance { get; private set; }
    public int score;
    public int money;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        WaveManagement.Instance.masksHUDText.text = $"{money}";
    }
        public void IncreaseScore(int _scoreIncrease)
    {
        score += _scoreIncrease;
    }

    public void ChangeMoney(int _moneyChange)
    {
        money +=_moneyChange;
        WaveManagement.Instance.masksHUDText.text = $"{money}";
    }


    public int GetScore()
    {
        return score;
    }

    public int GetMoney()
    {
        return money;
    }
}
