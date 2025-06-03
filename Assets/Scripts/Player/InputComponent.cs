using UnityEngine;
using UnityEngine.InputSystem;

// === INPUT COMPONENT ===
public class InputComponent : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private bool enableInput = true;
    
    // Input state
    private Vector2 moveInput;
    private Vector2 aimInput;
    private bool isUsingController;
    
    // References
    private PlayerInput playerInput;
    
    // Events for loose coupling
    public System.Action<MovementInput> OnInputGathered;
    
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    
    private void Update()
    {
        if (enableInput)
        {
            GatherAndBroadcastInput();
        }
    }
    
    private void GatherAndBroadcastInput()
    {
        MovementInput input = new MovementInput
        {
            moveInput = this.moveInput,
            rawAimInput = this.aimInput,
            isUsingController = this.isUsingController,
            timestamp = Time.time
        };
        
        OnInputGathered?.Invoke(input);
    }
    
    // Input System callbacks
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    
    public void OnAim(InputAction.CallbackContext context)
    {
        aimInput = context.ReadValue<Vector2>();
        isUsingController = playerInput.currentControlScheme == "Gamepad";
    }
    
    // Public interface
    public void SetInputEnabled(bool enabled) => enableInput = enabled;
    public Vector2 GetCurrentMoveInput() => moveInput;
    public Vector2 GetCurrentAimInput() => aimInput;
    public bool IsUsingController() => isUsingController;
}