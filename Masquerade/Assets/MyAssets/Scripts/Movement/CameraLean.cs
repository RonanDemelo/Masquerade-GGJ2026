using UnityEngine;

public class CameraLean : MonoBehaviour
{
    [SerializeField] private float attackDamp = 0.5f;
    [SerializeField] private float decayDamp = 0.3f;
    [SerializeField] private float walkStrength = 0.075f;
    [SerializeField] private float slideStrength = 0.2f;
    [SerializeField] private float strengthResponse = 5f;

    private Vector3 dampedAcceleration;
    private Vector3 dampedAccelerationVelocity;
    private float smoothStrength;
    public void Initialize()
    {
        smoothStrength = walkStrength;
    }

    public void UpdateLean(float deltaTime, bool sliding, Vector3 acceleration, Vector3 up)
    {
        var _planarAcceleration = Vector3.ProjectOnPlane(acceleration, up);
        var _damping = _planarAcceleration.magnitude > dampedAcceleration.magnitude 
            ? attackDamp
            : decayDamp;
        dampedAcceleration = Vector3.SmoothDamp
            (
                current: dampedAcceleration,
                target: _planarAcceleration,
                currentVelocity: ref dampedAccelerationVelocity,
                smoothTime: _damping,
                maxSpeed: float.PositiveInfinity,
                deltaTime: deltaTime
            );

        var _leanAxis = Vector3.Cross(dampedAcceleration.normalized, up).normalized;

        transform.localRotation = Quaternion.identity;

        var _targetStrength = sliding
            ? slideStrength
            : walkStrength;
        smoothStrength = Mathf.Lerp(smoothStrength, _targetStrength, 1f - Mathf.Exp(-strengthResponse * deltaTime));

        transform.rotation = Quaternion.AngleAxis(-dampedAcceleration.magnitude * smoothStrength, _leanAxis) * transform.rotation;
    }
}
