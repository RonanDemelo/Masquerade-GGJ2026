using UnityEngine;
using UnityEditor;

public class WaterWheelController : MonoBehaviour
{
    [SerializeField] float rotationsPerSecond = 0.1f;
    float rotationDistance;

    private void Start()
    {
        rotationDistance = 360 * Time.fixedDeltaTime * rotationsPerSecond;
    }
    
    private void RotateWheel()
    {
        transform.Rotate(new Vector3(0, 1, 0f), -rotationDistance);
    }

    private void FixedUpdate()
    {
        RotateWheel();
    }
}
