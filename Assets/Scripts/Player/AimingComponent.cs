using UnityEngine;
using UnityEngine.InputSystem;

// === AIMING COMPONENT ===
public class AimingComponent : MonoBehaviour
{
    [Header("Aiming Settings")]
    [SerializeField] private Transform aimTransform;
    [SerializeField] private float controllerSensitivity = 200f;
    [SerializeField] private LayerMask aimingLayer = -1;
    
    // State
    private Vector3 currentAimDirection = Vector3.forward;
    private Camera mainCamera;
    
    // Events
    public System.Action<Vector3> OnAimDirectionChanged;
    
    private void Awake()
    {
        mainCamera = Camera.main ?? FindObjectOfType<Camera>();
        SetupAimTransform();
    }
    
    public void ProcessAimInput(MovementInput input)
    {
        Vector3 newAimDirection;
        
        if (input.isUsingController)
        {
            newAimDirection = ProcessControllerAiming(input);
        }
        else
        {
            newAimDirection = ProcessMouseAiming();
        }
        
        if (newAimDirection != Vector3.zero)
        {
            currentAimDirection = newAimDirection;
            ApplyAiming();
            OnAimDirectionChanged?.Invoke(currentAimDirection);
        }
    }
    
    private Vector3 ProcessControllerAiming(MovementInput input)
    {
        if (input.rawAimInput.magnitude > 0.1f)
        {
            return new Vector3(input.rawAimInput.x, 0, input.rawAimInput.y).normalized;
        }
        else if (input.moveInput.magnitude > 0.1f)
        {
            // Fallback to movement direction
            return new Vector3(input.moveInput.x, 0, input.moveInput.y).normalized;
        }
        return currentAimDirection;
    }
    
    private Vector3 ProcessMouseAiming()
    {
        if (mainCamera == null) return currentAimDirection;
        
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane groundPlane = new Plane(Vector3.up, transform.position);
        
        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 mouseWorldPos = ray.GetPoint(distance);
            return (mouseWorldPos - transform.position).normalized;
        }
        
        return currentAimDirection;
    }
    
    private void ApplyAiming()
    {
        if (aimTransform != null && currentAimDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentAimDirection);
            aimTransform.rotation = targetRotation;
        }
    }
    
    private void SetupAimTransform()
    {
        if (aimTransform == null)
        {
            GameObject aimObj = new GameObject("AimTransform");
            aimObj.transform.SetParent(transform);
            aimObj.transform.localPosition = Vector3.zero;
            aimTransform = aimObj.transform;
        }
    }
    
    // Public interface
    public Vector3 GetAimDirection() => currentAimDirection;
    public Transform GetAimTransform() => aimTransform;
    public void SetAimDirection(Vector3 direction)
    {
        currentAimDirection = direction.normalized;
        ApplyAiming();
    }
}
