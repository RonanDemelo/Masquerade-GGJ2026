using UnityEngine;

public class MaskBreakScript : MonoBehaviour
{
    [SerializeField] GameObject[] maskPieces;
    [SerializeField] float DestroyTimer = 5;
    [SerializeField] float shatterForce = 5;
    [SerializeField] AudioClip shatterSfx;
    private void Start()
    {
        foreach (var piece in maskPieces)
        {
            if (piece != null && piece.TryGetComponent<Rigidbody>(out var rb))
            {
                Vector3 dir = Random.insideUnitSphere.normalized;
                rb.AddForce(dir * shatterForce, ForceMode.Impulse);
                rb.AddForce(piece.transform.up * 4, ForceMode.Impulse);
                rb.AddTorque(-dir * shatterForce, ForceMode.Impulse);
            }
        }
        SoundManager.instance.PlaySound2D(shatterSfx, 0.2f, 1);
        Invoke("DeleteMask", DestroyTimer);
    }

    private void DeleteMask()
    {
        Destroy(gameObject);
    }

}
