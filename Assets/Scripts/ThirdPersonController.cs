using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public float movementSpeed = 5;
    Rigidbody rb;

    float horizontalInput;
    float verticalInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;

        rb.AddForce(moveDirection * movementSpeed, ForceMode.Force);
    }
}
