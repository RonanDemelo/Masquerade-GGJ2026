using System.Linq;
using UnityEngine;

public class PlayerStepSounds : MonoBehaviour
{
    [Header("Config Values:")]
    [SerializeField] float stepsPerMeter = 1f; // how many steps per meter
    [SerializeField] float volumePerMeter = 1f; // volume scaling based on movement

    [Header("References:")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] footsteps;
    [SerializeField] LayerMask groundLayers;

    private Vector3 lastPosition;
    public float distanceAccumulated;
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastPosition = rb.position;
    }

    private void Update()
    {
        // ADD A GROUNDCHECK V IMPORTANT.
        if (!BasicGroundCheck()) { return; }
        PlayFootstepSoundsBySpeed();
    }

    private void PlayFootstepSoundsBySpeed()
    {
        // Calculate distance moved this frame
        float distanceThisFrame = Vector3.Distance(rb.position, lastPosition);
        lastPosition = rb.position;

        // Add to accumulator
        distanceAccumulated += distanceThisFrame;

        // Current speed
        float speed = rb.linearVelocity.magnitude;

        // Nonlinear stride scaling (sqrt for diminishing growth)
        float strideScaling = Mathf.Pow(speed, 0.5f);

        // If player is not moving (or almost not), preload accumulator
        if (speed < 0.1f)
        {
            // set it so the *next movement* will immediately cross threshold
            distanceAccumulated = stepsPerMeter * 0.985f;
        }

        // Trigger step when threshold crossed
        if (distanceAccumulated >= stepsPerMeter * strideScaling && speed > 0.1f)
        {
            AudioClip newClip = footsteps[Random.Range(0, footsteps.Length)];
            audioSource.PlayOneShot(newClip, volumePerMeter);

            distanceAccumulated = 0;
        }
    }


    private bool BasicGroundCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 1, groundLayers))
        {
            return true;
        }
        distanceAccumulated = stepsPerMeter * 0.985f;
        return false;
    }
}

