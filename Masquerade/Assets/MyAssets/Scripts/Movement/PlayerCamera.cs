using System;
using UnityEngine;

public struct CameraInput
{
    public Vector2 Look;
}

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float camSen = 0.1f;
    private Vector3 eulerAng;
    public void Initialize(Transform target)
    {
        transform.position = target.position;
        transform.rotation = target.rotation;

        transform.eulerAngles = eulerAng = target.eulerAngles;
    }

    public void UpdateRotation(CameraInput input)
    {
        eulerAng += new Vector3(-input.Look.y, input.Look.x) * camSen;

        transform.eulerAngles = eulerAng;
    }

    public void UpdatePosition(Transform target)
    {
        transform.position = target.position;
    }
}
