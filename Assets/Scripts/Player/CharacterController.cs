using UnityEngine;
using UnityEngine.InputSystem;

// === MAIN CONTROLLER ===
public class CharacterController : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private InputComponent inputComponent;
    [SerializeField] private MovementComponent movementComponent;
    [SerializeField] private AimingComponent aimingComponent;
    
    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = true;
    
    private void Awake()
    {
        // Auto-find components if not assigned
        if (inputComponent == null) inputComponent = GetComponent<InputComponent>();
        if (movementComponent == null) movementComponent = GetComponent<MovementComponent>();
        if (aimingComponent == null) aimingComponent = GetComponent<AimingComponent>();
        
        // Wire up the components
        SetupComponentConnections();
    }
    
    private void SetupComponentConnections()
    {
        if (inputComponent != null)
        {
            inputComponent.OnInputGathered += ProcessInput;
        }
        
        if (movementComponent != null)
        {
            movementComponent.OnMovementStateChanged += OnMovementChanged;
        }
        
        if (aimingComponent != null)
        {
            aimingComponent.OnAimDirectionChanged += OnAimChanged;
        }
    }
    
    private void ProcessInput(MovementInput input)
    {
        // Process movement
        movementComponent?.ProcessMovementInput(input);
        
        // Process aiming
        aimingComponent?.ProcessAimInput(input);
    }
    
    private void OnMovementChanged(MovementState state)
    {
        if (showDebugInfo)
        {
            Debug.DrawRay(transform.position, state.velocity, Color.green);
        }
    }
    
    private void OnAimChanged(Vector3 aimDirection)
    {
        if (showDebugInfo)
        {
            Debug.DrawRay(transform.position, aimDirection * 2f, Color.red);
        }
    }
    
    // Public interface for other systems
    public Vector3 GetAimDirection() => aimingComponent?.GetAimDirection() ?? Vector3.forward;
    public Vector3 GetMoveDirection() => movementComponent?.GetVelocity().normalized ?? Vector3.zero;
    public bool IsMoving() => movementComponent?.IsMoving() ?? false;
    public float GetCurrentSpeed() => movementComponent?.GetSpeed() ?? 0f;
    
    // Network-ready interface
    public MovementState GetMovementState() => movementComponent?.GetCurrentState() ?? new MovementState();
    public void ApplyNetworkState(MovementState state) => movementComponent?.SetMovementState(state);
}

