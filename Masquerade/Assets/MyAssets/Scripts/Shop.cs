using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour, IInteractable
{
    public GameObject shopScreen;
    public Player player;

    [Header("Upgrade Ranks")]
    public int reloadRank;
    public int fireRateRank;
    public int smgReloadRank;
    public int smgFireRateRank;
    public int moveSpeedRank;
    public int shardRateRank;

    [Header("Upgrade Costs")]
    public int reloadCost;
    public int fireRateCost;
    public int smgReloadCost;
    public int smgFireRateCost;
    public int moveSpeedCost;
    public int shardRateCost;
    public int maskCost;
    public int healthCost;
    public float costModifier = 0.2f;

    [Header("Rank Images")]
    public GameObject[] reloadImages;
    public GameObject[] fireRateImages;
    public GameObject[] moveSpeedImages;
    public GameObject[] shardRateImages;

    [Header("Buttons")]
    public Button reloadButton;
    public Button fireRateButton;
    public Sprite smgReloadSprite;
    public Sprite smgFireRateSprite;

    [Header("Gun Upgrade variables")]
    public GameObject pistol;
    public GameObject smg;
    public AudioClip smgSound;

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


    public void ReloadSpeed()
    {
        if (AccoladeTracker.Instance.money >= reloadCost)
        {
            PlayerAttack playerAttack = player.GetComponentInChildren<PlayerAttack>();
            if (playerAttack == null) { Debug.Log($"player Attack is null"); return; }
            if (reloadRank >= 5) return;

            playerAttack.ChangeReloadSpeed(-0.1f);
            AccoladeTracker.Instance.money -= reloadCost;
            reloadRank++;
            reloadCost = (int)(reloadCost * costModifier);
            reloadImages[reloadRank - 1].SetActive(true);
            Debug.Log($"Rank up!");

            if (fireRateRank >= 5 && reloadRank >= 5)
            {
                reloadButton.onClick.AddListener(SMGReloadSpeed);
                reloadButton.image.sprite = smgReloadSprite;
                fireRateButton.onClick.AddListener(SMGFireRate);
                fireRateButton.image.sprite = smgFireRateSprite;
                pistol.SetActive(false);
                smg.SetActive(true);
                playerAttack.rangedAttackSound = smgSound;
                playerAttack.ChangeFireRate(-0.2f);
                playerAttack.ChangeReloadSpeed(-0.2f);
                foreach (var images in fireRateImages)
                {
                    images.SetActive(false);
                }
                foreach (var images in reloadImages)
                {
                    images.SetActive(false);
                }
            }

        }
        else
        {
            Debug.Log("Not enough money");
        }
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

            if(fireRateRank >= 5 && reloadRank >= 5)
            {
                reloadButton.onClick.AddListener(SMGReloadSpeed);
                reloadButton.image.sprite = smgReloadSprite;
                fireRateButton.onClick.AddListener(SMGFireRate);
                fireRateButton.image.sprite = smgFireRateSprite;
                pistol.SetActive(false);
                smg.SetActive(true);
                playerAttack.rangedAttackSound = smgSound;
                playerAttack.ChangeFireRate(-0.2f);
                playerAttack.ChangeReloadSpeed(-0.2f);
                foreach(var images in fireRateImages)
                {
                    images.SetActive(false);
                }
                foreach (var images in reloadImages)
                {
                    images.SetActive(false);
                }
            }

        }
        else
        {
            Debug.Log("Not enough money");
        }
    }

    public void SMGReloadSpeed()
    {
        if (AccoladeTracker.Instance.money >= smgReloadCost)
        {
            PlayerAttack playerAttack = player.GetComponentInChildren<PlayerAttack>();
            if (playerAttack == null) { Debug.Log($"player Attack is null"); return; }
            if (smgReloadRank >= 5) return;

            playerAttack.ChangeReloadSpeed(-0.05f);
            AccoladeTracker.Instance.money -= reloadCost;
            smgReloadRank++;
            smgReloadCost = (int)(smgReloadCost * costModifier);
            reloadImages[smgReloadRank - 1].SetActive(true);
            Debug.Log($"Rank up!");
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }

    public void SMGFireRate()
    {
        if (AccoladeTracker.Instance.money >= smgFireRateCost)
        {
            PlayerAttack playerAttack = player.GetComponentInChildren<PlayerAttack>();
            if (playerAttack == null) { Debug.Log($"player Attack is null"); return; }
            if (smgFireRateRank >= 5) return;

            playerAttack.ChangeFireRate(-0.05f);
            AccoladeTracker.Instance.money -= reloadCost;
            smgFireRateRank++;
            smgFireRateCost = (int)(smgFireRateCost * costModifier);
            reloadImages[smgFireRateRank - 1].SetActive(true);
            Debug.Log($"Rank up!");
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }

    public void MoveSpeed()
    {
        if (AccoladeTracker.Instance.money >= moveSpeedCost)
        {
            PlayerCharacter playerMovement = player.GetComponentInChildren<PlayerCharacter>();
            if (playerMovement == null) { Debug.Log($"player Attack is null"); return; }
            if (moveSpeedRank >= 5) return;

            playerMovement.ChangeWalkSpeed(-0.4f);
            AccoladeTracker.Instance.money -= moveSpeedCost;
            moveSpeedRank++;
            moveSpeedCost = (int)(moveSpeedCost * costModifier);
            moveSpeedImages[moveSpeedRank - 1].SetActive(true);
            Debug.Log($"Rank up!");
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }

    public void ShardRate()
    {
        if (AccoladeTracker.Instance.money >= shardRateCost)
        {
            if (moveSpeedRank >= 5) return;

            AccoladeTracker.Instance.ChangeShardModifier(0.2f);
            AccoladeTracker.Instance.money -= shardRateCost;
            shardRateRank++;
            shardRateCost = (int)(shardRateCost * costModifier);
            shardRateImages[shardRateRank - 1].SetActive(true);
            Debug.Log($"Rank up!");
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }

    public void WearMask()
    {

    }
    public void HealthPack()
    {
        if (AccoladeTracker.Instance.money >= moveSpeedCost)
        {
            PlayerHealth playerHealth = player.GetComponentInChildren<PlayerHealth>();
            if (playerHealth == null) { Debug.Log($"player Attack is null"); return; }
            if (playerHealth.currentHealth >= playerHealth.baseHealth) return;

            playerHealth.RestoreHealth();
            AccoladeTracker.Instance.money -= healthCost;
            healthCost = (int)(healthCost * costModifier);
            Debug.Log($"Rank up!");
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }

    public void Interaction(GameObject player)
    {
        OnOpen();
    }
}
