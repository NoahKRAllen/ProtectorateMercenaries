using UnityEngine;

// === MOVEMENT COMPONENT ===
public class MovementComponent : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private float deceleration = 50f;
    [SerializeField] private bool useRigidbody = true;
    
    // State
    private MovementState currentState;
    private Rigidbody rb;
    
    // Events
    public System.Action<MovementState> OnMovementStateChanged;
    
    private void Awake()
    {
        if (useRigidbody)
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogWarning("MovementComponent: Rigidbody not found, adding one.");
                rb = gameObject.AddComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezeRotation;
            }
        }
        
        currentState = new MovementState
        {
            position = transform.position,
            velocity = Vector3.zero,
            timestamp = Time.time
        };
    }
    
    public void ProcessMovementInput(MovementInput input)
    {
        // Calculate target velocity
        Vector3 targetVelocity = new Vector3(input.moveInput.x, 0, input.moveInput.y) * moveSpeed;
        
        // Apply acceleration/deceleration
        float accelRate = (targetVelocity.magnitude > 0) ? acceleration : deceleration;
        currentState.velocity = Vector3.MoveTowards(
            currentState.velocity, 
            targetVelocity, 
            accelRate * Time.deltaTime
        );
        
        // Update position
        currentState.position = transform.position + currentState.velocity * Time.deltaTime;
        currentState.timestamp = Time.time;
        
        // Apply the movement
        ApplyMovement();
        
        // Notify listeners
        OnMovementStateChanged?.Invoke(currentState);
    }
    
    private void ApplyMovement()
    {
        if (useRigidbody && rb != null)
        {
            // Use Rigidbody for physics-based movement
            rb.linearVelocity = new Vector3(currentState.velocity.x, rb.linearVelocity.y, currentState.velocity.z);
        }
        else
        {
            // Direct transform movement
            transform.position = currentState.position;
        }
    }
    
    // Public interface for networking/other systems
    public MovementState GetCurrentState() => currentState;
    public void SetMovementState(MovementState state)
    {
        currentState = state;
        ApplyMovement();
    }
    
    public bool IsMoving() => currentState.velocity.magnitude > 0.1f;
    public Vector3 GetVelocity() => currentState.velocity;
    public float GetSpeed() => currentState.velocity.magnitude;
    
    // Configuration
    public void SetMoveSpeed(float speed) => moveSpeed = speed;
    public void SetAcceleration(float accel) => acceleration = accel;
}
