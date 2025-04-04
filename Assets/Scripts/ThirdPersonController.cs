using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public float movementSpeed = 5;
    Rigidbody rb;
    Animator animator;


    float horizontalInput;
    float verticalInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;

        rb.AddForce(moveDirection * movementSpeed, ForceMode.Force);

        // Animation
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if(flatVel.magnitude > movementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * movementSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }

        float velocity = flatVel.magnitude;

        animator.SetFloat("Velocity", velocity);
    }
}
