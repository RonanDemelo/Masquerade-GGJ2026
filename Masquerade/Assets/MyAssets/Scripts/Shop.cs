using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject shopScreen;
    public Player player;
    [Header("upgradeRanks")]
    public int reloadRank, fireRateRank, smgReloadRank, smgFireRateRank, moveSpeedRank, shardRateRank;
    [Header("upgradeCosts")]
    public int reloadCost, fireRateCost, smgReloadCost, smgFireRateCost, moveSpeedCost, shardRateCost, maskCost, healthCost;
    public float costModifier = 0.2f;

    [Header("rankImages")]
    public GameObject[] reloadImages, fireRateImages, smgReloadImages, smgFireRateImages, moveSpeedImages, shardRateImages;

    public void OnOpen()
    {
        shopScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
    }

    public void OnClose()
    {
        shopScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    public void FireRate()
    {
        if(AccoladeTracker.Instance.money >= fireRateCost)
        {
            PlayerAttack playerAttack = player.GetComponentInChildren<PlayerAttack>();
            if(playerAttack == null) {Debug.Log($"player Attack is null"); return; }
            if (fireRateRank >= 5) return;

            playerAttack.ChangeFireRate(-0.1f);
            AccoladeTracker.Instance.money -= fireRateCost;
            fireRateRank++;
            fireRateCost = (int)(fireRateCost * costModifier);
            fireRateImages[fireRateRank - 1].SetActive(true);
            Debug.Log($"Rank up!");
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }
}
