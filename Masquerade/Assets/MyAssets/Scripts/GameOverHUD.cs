using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverHUD : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text waveText;
    public TMP_Text moneyText;

    private void Awake()
    {
        
        scoreText.text = $"Score: {AccoladeTracker.Instance.score}";
        waveText.text = $"Wave Reached: {WaveManagement.Instance.currentWave}";
        moneyText.text = $"Remaining Shards: {AccoladeTracker.Instance.money}";
    }
    public void StopTime()
    {
        Time.timeScale = 0f;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(2);
    }
}
