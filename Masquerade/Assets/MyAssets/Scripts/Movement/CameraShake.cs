using UnityEngine;

/// <summary>
/// Code stolen from: https://gist.github.com/ftvs/5822103
/// </summary>

public class CameraShake : MonoBehaviour
{
    public Transform camTransform;
    public bool shouldShake;

    [Header("Shake Duration")]
    public float shakeDuration = 0f;
    private float originalShakeDuration;

    [Header("Shake Properties")]
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
        originalShakeDuration = shakeDuration;
    }

    void Update()
    {
        if (shouldShake)
        {
            if (shakeDuration > 0)
            {
                camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

                shakeDuration -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                shakeDuration = 0f;
                camTransform.localPosition = originalPos;
                shouldShake = false;
                shakeDuration = originalShakeDuration;
            }
        }
    }
}
