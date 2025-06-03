using UnityEngine;
using UnityEngine.InputSystem;

// === TARGETING RETICLE COMPONENT ===
public class TargetingReticle : MonoBehaviour
{
    [Header("Reticle Settings")]
    [SerializeField] private GameObject reticlePrefab;
    [SerializeField] private float maxRange = 50f;
    [SerializeField] private float controllerRange = 10f;
    [SerializeField] private LayerMask targetLayers = -1;
    [SerializeField] private bool showReticle = true;
    
    [Header("Visual Settings")]
    [SerializeField] private bool scaleWithDistance = true;
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 2f;
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.Linear(0, 1, 1, 1);
    
    // Components
    private Camera mainCamera;
    private AimingComponent aimingComponent;
    private Transform reticleTransform;
    private GameObject reticleInstance;
    
    // State
    private Vector3 currentTargetPosition;
    private bool isValidTarget = false;
    
    // Events
    public System.Action<Vector3, bool> OnTargetPositionChanged;
    
    private void Awake()
    {
        mainCamera = Camera.main ?? FindObjectOfType<Camera>();
        aimingComponent = GetComponent<AimingComponent>();
        
        CreateReticle();
    }
    
    private void OnEnable()
    {
        if (aimingComponent != null)
        {
            aimingComponent.OnAimDirectionChanged += HandleAimDirectionChanged;
        }
    }
    
    private void OnDisable()
    {
        if (aimingComponent != null)
        {
            aimingComponent.OnAimDirectionChanged -= HandleAimDirectionChanged;
        }
    }
    
    private void CreateReticle()
    {
        if (reticlePrefab != null)
        {
            reticleInstance = Instantiate(reticlePrefab);
            reticleTransform = reticleInstance.transform;
            reticleInstance.SetActive(showReticle);
        }
        else
        {
            // Create a simple default reticle
            CreateDefaultReticle();
        }
    }
    
    private void CreateDefaultReticle()
    {
        GameObject reticleObj = new GameObject("DefaultReticle");
        
        // Create a simple cross-hair using UI elements or primitives
        GameObject horizontal = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject vertical = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
        horizontal.transform.SetParent(reticleObj.transform);
        vertical.transform.SetParent(reticleObj.transform);
        
        // Scale to make thin lines
        horizontal.transform.localScale = new Vector3(0.2f, 0.02f, 0.02f);
        vertical.transform.localScale = new Vector3(0.02f, 0.2f, 0.02f);
        
        // Remove colliders
        DestroyImmediate(horizontal.GetComponent<Collider>());
        DestroyImmediate(vertical.GetComponent<Collider>());
        
        reticleInstance = reticleObj;
        reticleTransform = reticleObj.transform;
        reticleInstance.SetActive(showReticle);
    }
    
    private void HandleAimDirectionChanged(Vector3 aimDirection)
    {
        UpdateTargetPosition(aimDirection);
    }
    
    private void UpdateTargetPosition(Vector3 aimDirection)
    {
        // Get the current input to determine control method
        // We need to check if we're using mouse or controller for different positioning logic
        Vector3 targetPosition;
        bool validTarget;
        
        if (IsUsingMouse())
        {
            (targetPosition, validTarget) = CalculateMouseTargetPosition();
        }
        else
        {
            (targetPosition, validTarget) = CalculateControllerTargetPosition(aimDirection);
        }
        
        currentTargetPosition = targetPosition;
        isValidTarget = validTarget;
        
        UpdateReticlePosition();
        OnTargetPositionChanged?.Invoke(currentTargetPosition, isValidTarget);
    }
    
    private bool IsUsingMouse()
    {
        // Check if mouse moved recently or if gamepad input is minimal
        return Mouse.current != null && 
               (Mouse.current.delta.ReadValue().magnitude > 0.1f || 
                Gamepad.current?.leftStick.ReadValue().magnitude < 0.1f);
    }
    
    private (Vector3 position, bool isValid) CalculateMouseTargetPosition()
    {
        if (mainCamera == null) return (transform.position + transform.forward * 5f, false);
        
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        // Try to hit something in the world
        if (Physics.Raycast(ray, out RaycastHit hit, maxRange, targetLayers))
        {
            return (hit.point, true);
        }
        
        // If no hit, project onto a plane at a reasonable distance
        Plane groundPlane = new Plane(Vector3.up, transform.position);
        if (groundPlane.Raycast(ray, out float distance))
        {
            return (ray.GetPoint(distance), false);
        }
        
        // Fallback
        return (ray.origin + ray.direction * 10f, false);
    }
    
    private (Vector3 position, bool isValid) CalculateControllerTargetPosition(Vector3 aimDirection)
    {
        // For controller, cast a ray in the aim direction
        Ray ray = new Ray(transform.position, aimDirection);
        
        if (Physics.Raycast(ray, out RaycastHit hit, maxRange, targetLayers))
        {
            return (hit.point, true);
        }
        
        // No hit, place at fixed distance in aim direction
        return (transform.position + aimDirection * controllerRange, false);
    }
    
    private void UpdateReticlePosition()
    {
        if (reticleTransform == null) return;
        
        reticleTransform.position = currentTargetPosition;
        
        // Make reticle face the camera
        if (mainCamera != null)
        {
            Vector3 lookDirection = (mainCamera.transform.position - reticleTransform.position).normalized;
            reticleTransform.rotation = Quaternion.LookRotation(-lookDirection);
        }
        
        // Scale based on distance if enabled
        if (scaleWithDistance)
        {
            UpdateReticleScale();
        }
        
        // Update visual state based on target validity
        UpdateReticleVisuals();
    }
    
    private void UpdateReticleScale()
    {
        if (mainCamera == null) return;
        
        float distance = Vector3.Distance(mainCamera.transform.position, currentTargetPosition);
        float normalizedDistance = Mathf.Clamp01(distance / maxRange);
        float scaleMultiplier = Mathf.Lerp(minScale, maxScale, scaleCurve.Evaluate(normalizedDistance));
        
        reticleTransform.localScale = Vector3.one * scaleMultiplier;
    }
    
    private void UpdateReticleVisuals()
    {
        // Change color or material based on whether target is valid
        // This is a simple example - you'd customize based on your reticle design
        Renderer[] renderers = reticleInstance.GetComponentsInChildren<Renderer>();
        Color targetColor = isValidTarget ? Color.red : Color.white;
        
        foreach (Renderer renderer in renderers)
        {
            if (renderer.material.HasProperty("_Color"))
            {
                renderer.material.color = targetColor;
            }
        }
    }
    
    // Public Interface
    public Vector3 GetTargetPosition() => currentTargetPosition;
    public bool IsValidTarget() => isValidTarget;
    public void SetReticleVisibility(bool visible)
    {
        showReticle = visible;
        if (reticleInstance != null)
        {
            reticleInstance.SetActive(visible);
        }
    }
    
    public void SetMaxRange(float range)
    {
        maxRange = range;
    }
    
    public void SetControllerRange(float range)
    {
        controllerRange = range;
    }
    
    // For debugging
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = isValidTarget ? Color.red : Color.yellow;
            Gizmos.DrawWireSphere(currentTargetPosition, 0.5f);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, currentTargetPosition);
        }
    }
}