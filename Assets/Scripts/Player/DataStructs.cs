using UnityEngine;

// === DATA STRUCTURES ===
[System.Serializable]
public struct MovementInput
{
    public Vector2 moveInput;
    public Vector2 rawAimInput;
    public bool isUsingController;
    public float timestamp;
}

[System.Serializable]
public struct MovementState
{
    public Vector3 position;
    public Vector3 velocity;
    public float timestamp;
}
