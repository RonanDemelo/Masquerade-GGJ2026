using KinematicCharacterController;
using KinematicCharacterController.Examples;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;

public enum CrouchInput
{
    None, Toggle
}
public enum Stance
{
    Stand, Crouch, Slide
}

public struct CharacterState
{
    public bool Grounded;

    public Stance stance;

    public Vector3 Velocity;

    public Vector3 Acceleration;
}

public struct CharacterInput
{
    public Quaternion Rotation;

    public Vector2 Move;

    public bool Jump;
    public bool JumpSustain;

    public CrouchInput Crouch;
}

public class PlayerCharacter : MonoBehaviour, ICharacterController
{
    [SerializeField] private Transform root;
    [SerializeField] private KinematicCharacterMotor motor;
    [SerializeField] private Transform camTarget;
    [Space]
    [SerializeField] private float walkSpeed = 25f;
    [SerializeField] private float crouchSpeed = 7f;
    [SerializeField] private float walkResponse = 20f;
    [SerializeField] private float crouchResponse = 15f;
    [Space]
    [SerializeField] private float airSpeed = 5f;
    [SerializeField] private float airAcceleration = 70f;
    [SerializeField] private float jumpForce = 5f;
    [Range(0f, 1f)]
    [SerializeField] private float jumpSustainGravity = 0.5f;
    [SerializeField] private float gravity = -90f;
    [SerializeField] private float coyoteTime = 0.2f;
    [Space]
    [SerializeField] private float standHeight = 2f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float crouchHeightResponse = 15f;
    [Range(0f, 1f)]
    [SerializeField] private float standCameraTargetHeight = 0.9f;
    [Range(0f, 1f)]
    [SerializeField] private float crouchCameraTargetHeight = 0.7f;
    [Space]
    [SerializeField] private float slideGravity = -90f;
    [SerializeField] private float slideStartSpeed = 20f;
    [SerializeField] private float slideEndSpeed = 7f;
    [SerializeField] private float slideFriction = 0.8f;
    [SerializeField] private float slideSteerAccleration = 5f;

    private CharacterState state;
    private CharacterState lastState;
    private CharacterState tempState;

    private Quaternion requestedRotation;
    private Vector3 requestedMovement;
    private bool requestedJump;
    private bool requestedJumpSustain;
    private bool requestedCrouch;
    private bool requestedCrouchInAir;

    private float timeSinceUngrounded;
    private float timeSinceJumpRequest;
    private bool ungroundedDueToJump;

    private Collider[] uncrouchOverlapResults;

    public void Initialize()
    {
        state.stance = Stance.Stand;
        lastState = state;
        uncrouchOverlapResults = new Collider[8];
        motor.CharacterController = this;
    }

    public Transform GetCameraTarget() => camTarget;

    public void UpdateInput(CharacterInput input)
    {
        requestedRotation = input.Rotation;

        requestedMovement = new Vector3(input.Move.x, 0f, input.Move.y);
        requestedMovement = Vector3.ClampMagnitude(requestedMovement, 1f);
        requestedMovement = input.Rotation * requestedMovement;

        var _wasRequestingJump = requestedJump;
        requestedJump = requestedJump || input.Jump;
        if(requestedJump && !_wasRequestingJump)
        {
            timeSinceJumpRequest = 0f;
        }
        requestedJumpSustain = input.JumpSustain;

        var _wasRequestingCrouch = requestedCrouch;

        requestedCrouch = input.Crouch switch
        {
            CrouchInput.Toggle => !requestedCrouch,
            CrouchInput.None => requestedCrouch,
            _ => requestedCrouch
        };
        if(requestedCrouch && _wasRequestingCrouch)
        {
            requestedCrouchInAir = !state.Grounded;
        }
        else if(!requestedCrouch && _wasRequestingCrouch)
        {
            requestedCrouchInAir = false;
        }

    }

    public void UpdateBody(float deltaTime)
    {
        var _currentHeight = motor.Capsule.height;
        var _normalizedHeight = _currentHeight / standHeight;
        var _cameraTargetHeight = _currentHeight *
            (
                state.stance is Stance.Stand
                    ? standCameraTargetHeight
                    : crouchCameraTargetHeight
            );
        var _rootTargetScale = new Vector3(1f, _normalizedHeight, 1f);

        camTarget.localPosition = Vector3.Lerp(
            a: camTarget.localPosition,
            b: new Vector3(0f, _cameraTargetHeight, 0f),
            t: 1f - Mathf.Exp(-crouchHeightResponse* deltaTime)
            );

        root.localScale = Vector3.Lerp(
            a: root.localScale,
            b: _rootTargetScale,
            t: 1f - Mathf.Exp(-crouchHeightResponse * deltaTime)
            );

    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) 
    {
        state.Acceleration = Vector3.zero;
        if (motor.GroundingStatus.IsStableOnGround)
        {
            timeSinceUngrounded = 0f;
            ungroundedDueToJump = false;
            var _groundedMovement = motor.GetDirectionTangentToSurface
            (
                direction: requestedMovement,
                surfaceNormal: motor.GroundingStatus.GroundNormal
            ) * requestedMovement.magnitude;

            //Sliding
            {
                var _moving = _groundedMovement.sqrMagnitude > 0f;
                var _crouching = state.stance is Stance.Crouch;
                var _wasStanding = lastState.stance is Stance.Stand;
                var _wasInAir = !lastState.Grounded;
                if(_moving && _crouching && (_wasStanding || _wasInAir))
                {
                    state.stance = Stance.Slide;

                    //preserve falling momentum
                    if(_wasInAir)
                    {
                        currentVelocity = Vector3.ProjectOnPlane
                            (
                                vector: lastState.Velocity,
                                planeNormal: motor.GroundingStatus.GroundNormal
                            );
                    }

                    var _effectiveSlideStartSpeed = slideStartSpeed;
                    if(!lastState.Grounded && !requestedCrouchInAir)
                    {
                        _effectiveSlideStartSpeed = 0f;
                        requestedCrouchInAir= false;
                    }
                    var _slideSpeed = Mathf.Max(_effectiveSlideStartSpeed, currentVelocity.magnitude);

                    currentVelocity = motor.GetDirectionTangentToSurface
                        (
                            direction: currentVelocity,
                            surfaceNormal: motor.GroundingStatus.GroundNormal
                        ) * _slideSpeed;
                }
            }
            //Move
            if(state.stance is Stance.Stand or Stance.Crouch)
            {
                var _speed = state.stance is Stance.Stand
                    ? walkSpeed
                    : crouchSpeed;
                var _response = state.stance is Stance.Stand
                        ? walkResponse
                        : crouchResponse
                        ;

                var _targetVelocity = _groundedMovement * _speed;

                var _moveVelocity = Vector3.Lerp
                    (
                        a: currentVelocity,
                        b: _targetVelocity,
                        t: 1f - Mathf.Exp(-_response * deltaTime)
                    );
                state.Acceleration = (_moveVelocity - currentVelocity) / deltaTime;
                currentVelocity = _moveVelocity;
            }
            else
            {
                //Friction
                currentVelocity -= currentVelocity* (slideFriction * deltaTime);
                //Slope
                var _force = Vector3.ProjectOnPlane
                    (
                        vector: -motor.CharacterUp,
                        planeNormal: motor.GroundingStatus.GroundNormal
                    ) * slideGravity;
                currentVelocity -= _force * deltaTime;

                //Slide Steer
                var _currentSpeed = currentVelocity.magnitude;
                var _targetVelocity = _groundedMovement * _currentSpeed;
                var _steerVelocity = currentVelocity;
                var _steerForce = (_targetVelocity - _steerVelocity) * slideSteerAccleration * deltaTime;
                _steerVelocity += _steerForce;
                _steerVelocity = Vector3.ClampMagnitude(_steerVelocity, _currentSpeed );

                state.Acceleration = (_steerVelocity - currentVelocity) / deltaTime;
                currentVelocity = _steerVelocity;
                //Stop Slide
                if(currentVelocity.magnitude < slideEndSpeed)
                {
                    state.stance = Stance.Crouch;
                }
                
            }
        }
        else
        {
            timeSinceUngrounded += deltaTime;
            //Air Movement
            if(requestedMovement.sqrMagnitude > 0)
            {
                var _planarMovement = Vector3.ProjectOnPlane
                    (
                        vector: requestedMovement,
                        planeNormal: motor.CharacterUp

                    ) * requestedMovement.magnitude;

                var _currentPlanarVelocity = Vector3.ProjectOnPlane
                    (
                        vector: currentVelocity,
                        planeNormal: motor.CharacterUp
                    );

                var _movementForce = _planarMovement * airAcceleration * deltaTime;

                //if moving slower than max air speed, treat movementforce as simple steering force
                if(_currentPlanarVelocity.magnitude < airSpeed)
                {
                    var _targetPlanarVelocity = _currentPlanarVelocity + _movementForce;

                    _targetPlanarVelocity = Vector3.ClampMagnitude(_targetPlanarVelocity, airSpeed);

                    _movementForce += _targetPlanarVelocity - _currentPlanarVelocity;
                }
                //otherwise nerf the movement when it is in the direction of the current planar velocity
                else if (Vector3.Dot(_currentPlanarVelocity, _movementForce) > 0f)
                {
                    var _constrainedMovementForce = Vector3.ProjectOnPlane
                        (
                            vector: _movementForce,
                            planeNormal: _currentPlanarVelocity.normalized
                        );

                    _movementForce = _constrainedMovementForce;
                }

                //prevent air climbing
                if(motor.GroundingStatus.FoundAnyGround)
                {
                    if(Vector3.Dot(_movementForce, currentVelocity +  _movementForce) > 0f)
                    {
                        var _obstructionNormal = Vector3.Cross
                            (
                                motor.CharacterUp,
                                Vector3.Cross
                                (
                                    motor.CharacterUp,
                                    motor.GroundingStatus.GroundNormal
                                )
                            ).normalized;

                        _movementForce = Vector3.ProjectOnPlane(_movementForce, _obstructionNormal);
                    }
                }

                currentVelocity += _movementForce;
            }
            //Gravity
            var _effectiveGravity = gravity;
            var _verticalSpeed = Vector3.Dot(currentVelocity, motor.CharacterUp);
            if(requestedJumpSustain && _verticalSpeed > 0f)
            {
                _effectiveGravity *= jumpSustainGravity;
            }
            currentVelocity += motor.CharacterUp * _effectiveGravity * deltaTime;
        }
        
        if( requestedJump )
        {
            var _grounded = motor.GroundingStatus.IsStableOnGround;
            var _canCoyoteJump = timeSinceUngrounded < coyoteTime && !ungroundedDueToJump;
            if( _grounded || _canCoyoteJump )
            {
                requestedJump = false;
                requestedCrouch = false;//reset character to standing
                requestedCrouchInAir = false;
                motor.ForceUnground(time: 0f);
                ungroundedDueToJump = true;

                var _currentVerticalSpeed = Vector3.Dot(currentVelocity, motor.CharacterUp);
                var _targetVerticalSpeed = Mathf.Max(_currentVerticalSpeed, jumpForce);
                currentVelocity += motor.CharacterUp * (_targetVerticalSpeed - _currentVerticalSpeed);
            }
            else
            {
                timeSinceJumpRequest += deltaTime;
                var _canJumpLater = timeSinceJumpRequest < coyoteTime;
                requestedJump = _canJumpLater;
            }
        }
    }

    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime) 
    {
        var _forward = Vector3.ProjectOnPlane
            (
                requestedRotation * Vector3.forward,
                motor.CharacterUp
            );

        if(_forward != Vector3.zero )
        {
            currentRotation = Quaternion.LookRotation(_forward, motor.CharacterUp);
        }
        
    }
    public void BeforeCharacterUpdate(float deltaTime)
    {
        tempState = state;
        //Crouch
        if(requestedCrouch && state.stance is Stance.Stand)
        {
            state.stance = Stance.Crouch;
            motor.SetCapsuleDimensions
                (
                    radius: motor.Capsule.radius,
                    height: crouchHeight,
                    yOffset: crouchHeight * 0.5f
                );
        }
    }
    public void PostGroundingUpdate(float deltaTime)
    {
        if(!motor.GroundingStatus.IsStableOnGround && state.stance is Stance.Slide)
        {
            state.stance = Stance.Crouch;
        }
    }
    public void AfterCharacterUpdate(float deltaTime)
    {
        var _totalAcceleration = (state.Velocity - lastState.Velocity) / deltaTime;

        state.Acceleration = Vector3.ClampMagnitude(state.Acceleration, _totalAcceleration.magnitude);
        //Uncrouch
        if (!requestedCrouch && state.stance is not Stance.Stand)
        {
            
            motor.SetCapsuleDimensions
                (
                    radius: motor.Capsule.radius,
                    height: standHeight,
                    yOffset: standHeight * 0.5f
                );

            var _pos = motor.TransientPosition;
            var _rot = motor.TransientRotation;
            var _mask = motor.CollidableLayers;

            if (motor.CharacterOverlap(_pos, _rot, uncrouchOverlapResults, _mask, QueryTriggerInteraction.Ignore) > 0)
            {
                //recrouch
                requestedCrouch = true;
                motor.SetCapsuleDimensions
                (
                    radius: motor.Capsule.radius,
                    height: crouchHeight,
                    yOffset: crouchHeight * 0.5f
                );
            }
            else { state.stance = Stance.Stand; }
        }

        state.Grounded = motor.GroundingStatus.IsStableOnGround;
        state.Velocity = motor.Velocity;
        lastState = tempState;
    }
    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) { }
    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
        state.Acceleration = Vector3.ProjectOnPlane(state.Acceleration, hitNormal);
    }
    public bool IsColliderValidForCollisions(Collider coll) => true;
    public void OnDiscreteCollisionDetected(Collider hitCollider) { }
    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport) { }

    //Teleport
    public void SetPosition(Vector3 position, bool killVelocity = true)
    {
        motor.SetPosition(position);
        if(killVelocity)
        {
            motor.BaseVelocity = Vector3.zero;
        }
    }

    public CharacterState GetState() => state;
    public CharacterState GetLastState() => lastState;
}
