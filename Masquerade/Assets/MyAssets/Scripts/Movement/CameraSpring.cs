using UnityEngine;

public class CameraSpring : MonoBehaviour
{
    [Min(0.01f)]
    [SerializeField] private float halfLife = 0.075f;
    [SerializeField] private float frequency = 18f;
    [SerializeField] private float angularDisplacement = 2f;
    [SerializeField] private float linearDisplacement = 0.05f;

    private Vector3 springPosition;
    private Vector3 springVelocity;
    public void Initialize()
    {
        springPosition = transform.position;
        springVelocity = Vector3.zero;
    }

    public void UpdateSpring(float deltaTime, Vector3 up)
    {
        transform.localPosition = Vector3.zero;
        Spring(ref springPosition, ref springVelocity, transform.position, halfLife, frequency, deltaTime);

        var _relativeSpringPosition = springPosition - transform.position;
        var _springHeight = Vector3.Dot(_relativeSpringPosition, up);

        transform.localEulerAngles = new Vector3(-_springHeight * angularDisplacement, 0f, 0f);
        transform.position += _relativeSpringPosition * linearDisplacement;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, springPosition);
        Gizmos.DrawSphere(springPosition, 0.1f);
    }

    //allen chou
    private static void Spring(ref Vector3 current, ref Vector3 velocity, Vector3 target, float halfLife, float frequency, float timeStep)
    {
        var _dampingRatio = -Mathf.Log(0.5f)/(frequency*halfLife);
        var _f = 1.0f + 2.0f * timeStep * _dampingRatio * frequency;
        var _oo = frequency * frequency;
        var _hoo = timeStep * _oo;
        var _hhoo = timeStep * _hoo;
        var _detInv = 1.0f /(_f + _hhoo);
        var _detX = _f * current + timeStep * velocity + _hhoo * target;
        var _detV = velocity + _hoo * (target - current);
        current = _detX * _detInv;
        velocity = _detV * _detInv;
    }
}
