using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Old Input System Setup
    //TODO: Change to New Input System, and network based controls
    [SerializeField] private int movementSpeed = 5;
    [SerializeField] private float rotationSpeed = 10f;
    private void Update()
    {
        Vector2 inputVector = new Vector2(0,0);
        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y += 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y -= 1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x += 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x -= 1;
        }
        
        inputVector = inputVector.normalized;
        
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        transform.position += moveDir * (Time.deltaTime * movementSpeed);
        
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
    }
}
