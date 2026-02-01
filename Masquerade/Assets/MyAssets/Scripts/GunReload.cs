using UnityEngine;

public class GunReload : MonoBehaviour
{
    public PlayerAttack playerAttack;
    Animator animator;
    public AudioClip reloadClip;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayReloadSound()
    {
        SoundManager.instance.PlaySound2D(reloadClip, 1, 1);
    }


    public void ReloadStart()
    {
        playerAttack.canShoot = false;
    }
    public void ReloadComplete()
    {
        playerAttack.canShoot = true;
        if (AccoladeTracker.Instance.money >= playerAttack.maxClipSize)
        {
            playerAttack.currentClip = playerAttack.maxClipSize;
            AccoladeTracker.Instance.ChangeMoney(-playerAttack.maxClipSize);
        }
        else if (AccoladeTracker.Instance.money < playerAttack.maxClipSize && AccoladeTracker.Instance.money > 0)
        {
            playerAttack.currentClip = AccoladeTracker.Instance.money;
            AccoladeTracker.Instance.ChangeMoney(-AccoladeTracker.Instance.money);
        }
        animator.SetBool("isReload", false);
    }
}
