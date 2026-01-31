using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    [SerializeField] private AudioSource audioSource2D;
    [SerializeField] private AudioSource audioSource3D;

    public void PlaySound3D(AudioClip clip, Vector3 position, float volume, float pitch)
    {
        audioSource3D.pitch = pitch;
        audioSource3D.transform.position = position;
        audioSource3D.PlayOneShot(clip, volume);
    }

    public void PlaySound2D(AudioClip clip, float volume, float pitch)
    {
        audioSource2D.pitch = pitch;
        audioSource2D.PlayOneShot(clip, volume);
    }


}
