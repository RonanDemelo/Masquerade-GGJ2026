using UnityEngine;
using TMPro;

public class WaveBar : MonoBehaviour
{
    public TMP_Text waveBarText;

    public void DisableBar()
    {
        gameObject.SetActive(false);
    }
}
