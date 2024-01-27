using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [field: Header("Inputs")] public Vector2 MoveInput { get; set; }
    public bool IsSprinting { get; set; }

    [Header("Horizontal movement")] public float movementSpeed = 8f;
    public float airMovementSpeed = 6f;
    public float acceleration = 2f;
    public float deceleration = 8f;
    public float airAcceleration = 2f;
    public float airDeceleration = 8f;

    [Header("Jump")] public float jumpInputDuration = .5f;
    public float initialJumpForce = 10f;
    public float coyoteTime = .125f;

    [Tooltip("Maximum allowed speed when falling. Expressed as a positive quantity.")]
    public float maxFallSpeed = 40;

    [Header("Spring")] public float cruiseHeight = .5f;
    public float springStrength = 20f;
    public float dampenFactor = .7f;

    [Header("Layer collisions")] public LayerMask _groundCollisionMask;
    public LayerMask _headCollisionMask;

    [Header("Sprite")]
    public Transform body;

    private Rigidbody2D _rb;
    private float _previousInputVector;
    private float _previousInputMagnitude;
    private float _horizontalVelocity;
    private float _verticalVelocity;
    private float _castRadius = .4f;
    private Vector2 _groundCastStart;
    private Vector2 _headCastStart;
    private Vector2 _forwardLowCastStart;
    private Vector2 _forwardHighCastStart;
    private RaycastHit2D _groundHit;
    private RaycastHit2D _headHit;
    private RaycastHit2D _forwardHit;
    private float _timeSinceGrounded;
    private int _current2DLayer;
    public DragonPart _currentDragonPart;
    private bool _isFlyingOff;
    
    [Header("Debug (should be private)")]
    [SerializeField] private bool _isGrounded;
    [SerializeField] private float _jumpTimer;
    [SerializeField] private bool _isFalling;
    [SerializeField] private bool _isAnticipatingJump = false;
    [SerializeField] private bool _isReadyToJump = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        UpdateJump();
        // Not checking if actively jumping, to avoid the spring
        // from holding the player to the ground in the first frame
        if(_jumpTimer <= 0) GroundCheck();
        
        if(ForwardCheck(MoveInput.x) && !_isAnticipatingJump)
        {
            if(!_isFlyingOff)
            {
                float inputMagnitude = Mathf.Abs(MoveInput.x);
                float inputAcceleration = inputMagnitude > _previousInputMagnitude
                    ? _isGrounded ? acceleration : airAcceleration
                    : _isGrounded
                        ? deceleration
                        : airDeceleration;

                float lerpedInputVector = Mathf.Lerp(_previousInputVector, MoveInput.x,
                    Time.deltaTime * inputAcceleration);
                float lerpedMagnitude = Mathf.Abs(lerpedInputVector);
                _horizontalVelocity = lerpedInputVector * (_isGrounded ? movementSpeed : airMovementSpeed);

                body.localScale = new Vector3(-Mathf.Sign(_horizontalVelocity), 1f, 1f);

                // Caching for next frame
                _previousInputMagnitude = lerpedMagnitude;
                _previousInputVector = lerpedInputVector;
            }
        }
        else
        {
            _horizontalVelocity = 0f;
            _previousInputMagnitude = 0f;
            _previousInputVector = 0f;
        }
        
        // Apply both components to find final velocity
        _rb.velocity = new Vector2(_horizontalVelocity, _verticalVelocity);
    }

    private void GroundCheck()
    {
        if (_isFlyingOff) return;
        
        Vector2 velocity = _rb.velocity;
        
        Vector2 position2D = transform.position;
        _groundCastStart =  position2D + Vector2.up * (cruiseHeight + _castRadius * .5f);
        
        // Check for ground
        bool wasGrounded = _isGrounded;
        _groundHit = Physics2D.CircleCast(_groundCastStart, _castRadius, Vector2.down, 
                                            cruiseHeight, _groundCollisionMask);
        _isGrounded = _groundHit.collider != null;

        if (_isGrounded)
        {
            if(!wasGrounded) Land();

            // Ground spring
            float displacement = _groundHit.distance - cruiseHeight + _castRadius * .5f;
            float dampingForce = velocity.y * dampenFactor;
            float springForce = -displacement * springStrength - dampingForce;

            _verticalVelocity = Mathf.Lerp(_verticalVelocity, springForce, 
                                                    Time.deltaTime * 10f);
        }
        else
        {
            // Free-falling
            if(wasGrounded) Fall();
            ApplyGravity();
        }
    }

    private void ApplyGravity()
    {
        _verticalVelocity += Physics2D.gravity.y * Time.deltaTime;
        _timeSinceGrounded += Time.deltaTime;
    }

    public void AnticipateJump()
    {
        bool canJump = _isGrounded || _timeSinceGrounded < coyoteTime;
        if (canJump) _isAnticipatingJump = true;
    }

    public void ReadyToJump() {
        _isReadyToJump = true;
    }

    private void Jump()
    {
        _jumpTimer = jumpInputDuration;
        
        // Will be also applied in FixedUpdate
        _verticalVelocity = initialJumpForce;
        
        // Change it immediately
        _rb.velocity = new Vector2(MoveInput.x, _verticalVelocity);

        if (_currentDragonPart != null)
        {
            _currentDragonPart.ShakeOff -= FlyOff;
            _currentDragonPart = null;
        }

        _isGrounded = false;
    }

    private void Fall()
    {
        
    }

    private void Land()
    {
        _rb.gravityScale = 1f;
        _timeSinceGrounded = 0f;
        _verticalVelocity = 0f;

        if (_groundHit.collider.TryGetComponent(out DragonPart newDragonPart))
        {
            if (_currentDragonPart == null && newDragonPart != _currentDragonPart)
            {
                _currentDragonPart = newDragonPart;
                _currentDragonPart.ShakeOff += FlyOff;
            }
        }
        else
        {
            if (_currentDragonPart != null)
            {
                _currentDragonPart.ShakeOff -= FlyOff;
                _currentDragonPart = null;
            }
        }
    }

    private void FlyOff()
    {
        _isFlyingOff = true;
        _isGrounded = false;
        
        _verticalVelocity = initialJumpForce * 3f;
        _horizontalVelocity = Random.Range(-20f, 20f);
        _rb.velocity = new Vector2(_horizontalVelocity, _verticalVelocity);
        
        _currentDragonPart.ShakeOff -= FlyOff;
        _currentDragonPart = null;

        StartCoroutine(EndFlyOff());
    }

    private IEnumerator EndFlyOff()
    {
        yield return new WaitForSeconds(2f);
        _isFlyingOff = false;
    }

    public void InterruptJump()
    {
        if (_isReadyToJump)
        {
            Jump();
        }
        else
        {
            _jumpTimer = -1f;
        }
        _isReadyToJump = false;
        _isAnticipatingJump = false;
    }

    private void UpdateJump()
    {
        if (_isGrounded) return;

        HeadCheck();
        
        if (_jumpTimer > 0f)
        {
            _rb.gravityScale = 0f;
            
            _jumpTimer -= Time.deltaTime;
            if (_jumpTimer < 0f) InterruptJump();
        }
        else
        {
            float jumpProgress = 1f - Mathf.InverseLerp(0f, jumpInputDuration, _jumpTimer);
            const float a = .4f;
            const float b = .6f;
            _rb.gravityScale = jumpProgress switch
            {
                < a => 1f,
                > a and <= b => 0f,
                > b => 3f,
                _ => _rb.gravityScale
            };
        }
        
        _verticalVelocity += Physics2D.gravity.y * _rb.gravityScale * Time.deltaTime;
        
        _isFalling = !_isGrounded && _rb.velocity.y <= 0f;
        
        // Cap vertical velocity when falling
        if (_isFalling && _verticalVelocity < -maxFallSpeed)
            _verticalVelocity = -maxFallSpeed;
    }

    private void HeadCheck()
    {
        Vector2 position2D = transform.position;
        _headCastStart =  position2D + Vector2.up * 1.6f;
        
        // Check for ground
        _headHit = Physics2D.CircleCast(_headCastStart, _castRadius, Vector2.up, 
            .1f, _headCollisionMask);
        bool headHit = _headHit.collider != null;

        if (headHit)
        {
            InterruptJump();
            _verticalVelocity = 0f;
        }
    }

    private bool ForwardCheck(float input)
    {
        if (_isFlyingOff) return true;
        
        Vector2 position2D = transform.position;
        
        // Low sweep
        _forwardLowCastStart =  position2D + Vector2.up * 1f;
        _forwardHighCastStart = _forwardLowCastStart + Vector2.up * .4f;
            
        _forwardHit = Physics2D.CircleCast(_forwardLowCastStart, _castRadius, Vector2.right * Mathf.Sign(input), 
            .2f, _headCollisionMask);
        bool obstacleFound = _forwardHit.collider != null;
        
        if (!obstacleFound)
        {
            // High sweep
            _forwardHit = Physics2D.CircleCast(_forwardHighCastStart, _castRadius, Vector2.right * Mathf.Sign(input), 
                .2f, _groundCollisionMask);
            obstacleFound = _forwardHit.collider != null;
        }

        return !obstacleFound;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position, .1f);
        
        // Ground check cast
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_groundCastStart, _castRadius);
        Gizmos.DrawWireSphere(_groundCastStart + Vector2.down * cruiseHeight, _castRadius);
        
        // Head check cast
        Gizmos.color = new Color(1f, 0.39f, 0.18f);
        Gizmos.DrawWireSphere(_headCastStart + Vector2.up * .2f, _castRadius);
        
        // Forward check cast
        Gizmos.color = new Color(1f, 0.35f, 0.63f);
        Gizmos.DrawWireSphere(_forwardLowCastStart + Vector2.right * Mathf.Sign(_previousInputVector) * .2f, _castRadius);
        Gizmos.DrawWireSphere(_forwardHighCastStart + Vector2.right * Mathf.Sign(_previousInputVector) * .2f, _castRadius);
    }
}
