using UnityEngine;

public class MasqueradeMaskBreak : MonoBehaviour
{
    [SerializeField] private GameObject shatteringMask;

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
        if(collision != null)
        {
            if (collision.gameObject.name == "Mask Shard")
            {
                Transform newParent = gameObject.transform.parent;
                GameObject mask = Instantiate(shatteringMask, newParent);
                Destroy(gameObject);
            }
        }
    }
}
