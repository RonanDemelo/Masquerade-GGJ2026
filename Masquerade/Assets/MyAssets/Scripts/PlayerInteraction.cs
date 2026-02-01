
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask interactionLayerMask;
    [SerializeField] private float interactionRange = 4f;

    private Camera playerCamera;

    private void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
    }


    public void TryInteract()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactionLayerMask))
        {
            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
            interactable?.Interaction(this.gameObject); // Let the object decide what to do
        }
    }
}
