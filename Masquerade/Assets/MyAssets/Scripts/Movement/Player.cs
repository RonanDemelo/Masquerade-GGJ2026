using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerCharacter playerCharacter;
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private CameraSpring cameraSpring;
    [SerializeField] private CameraLean cameraLean;
    [Space]
    [SerializeField] private Volume volume;
    [SerializeField] private StanceVignette stanceVignette;

    private PlayerInputActions inputActions;
    private PlayerInteraction playerInteraction;

    void Start()
    {
        playerInteraction = GetComponent<PlayerInteraction>();
        Cursor.lockState = CursorLockMode.Locked;

        inputActions = new PlayerInputActions();
        inputActions.Enable();

        playerCharacter.Initialize();
        playerCamera.Initialize(playerCharacter.GetCameraTarget());
        cameraSpring.Initialize();
        cameraLean.Initialize();
        stanceVignette.Initialize(volume.profile);
    }

    private void OnDestroy()
    {
        inputActions.Dispose();
    }
    void Update()
    {
        //temp interact ronan or mei can change if they want to cos i sill not be changing it.
        if(Input.GetKeyDown(KeyCode.E))
        {
            playerInteraction.TryInteract();
        }
        var _input = inputActions.Gameplay;
        var _deltaTime = Time.deltaTime;

        //Get camera input and update its rotation
        var _cameraInput = new CameraInput {Look = _input.Look.ReadValue<Vector2>()};
        playerCamera.UpdateRotation(_cameraInput);

        //Get Character input
        var characterInput = new CharacterInput
        {
            Rotation    = playerCamera.transform.rotation,
            Move        = _input.Move.ReadValue<Vector2>(),
            Jump        = _input.Jump.WasPressedThisFrame(),
            JumpSustain = _input.Jump.IsPressed(),
            Crouch      = _input.Crouch.WasPressedThisFrame()
                ? CrouchInput.Toggle
                : CrouchInput.None

        };
        playerCharacter.UpdateInput(characterInput);
        playerCharacter.UpdateBody(_deltaTime);

        //Teleport
        #if UNITY_EDITOR
        if(Keyboard.current.tKey.wasPressedThisFrame)
        {
            var _ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if(Physics.Raycast(_ray, out var hit))
            {
                Teleport(hit.point);
            }
        }
        #endif
    }

    private void LateUpdate()
    {
        var _deltaTime = Time.deltaTime;
        var _cameraTarget = playerCharacter.GetCameraTarget();
        var _state = playerCharacter.GetState();
        playerCamera.UpdatePosition(_cameraTarget);
        cameraSpring.UpdateSpring(_deltaTime, _cameraTarget.up);
        cameraLean.UpdateLean(_deltaTime, _state.stance is Stance.Slide,_state.Acceleration,_cameraTarget.up);

        stanceVignette.UpdateVignette(_deltaTime, _state.stance);
    }

    public void Teleport(Vector3 position)
    {
        playerCharacter.SetPosition(position);
    }
}
