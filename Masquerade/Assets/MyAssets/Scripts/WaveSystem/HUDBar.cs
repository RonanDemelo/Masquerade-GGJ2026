using UnityEngine;
using TMPro;

public class HUDBar : MonoBehaviour
{
    public TMP_Text hudBarText;

    public void DisableBar()
    {
        gameObject.SetActive(false);
    }
}
