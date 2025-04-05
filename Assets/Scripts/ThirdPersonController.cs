using System.Collections;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    // length = 1.167 = rollDistance/rollSpeed
    public float movementSpeed = 20f;
    public float rollDistance = 20f;
    public float rollSpeed = 17f;
    public bool isRolling;
    public Camera mainCamera;
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
        Movement();
        Roll();
    }

    void Movement()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = (cameraForward * verticalInput + cameraRight * horizontalInput).normalized;

        rb.AddForce(moveDirection * movementSpeed, ForceMode.Force);

        // Animation
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > movementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * movementSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }

        float velocity = flatVel.magnitude;

        animator.SetFloat("Velocity", velocity);
    }

    void Roll()
    {
        if(Input.GetKeyDown(KeyCode.F) && !isRolling)
        {
            animator.SetTrigger("Roll");

            Vector3 rollDirection;

            if(horizontalInput == 0)
            {
                Vector3 cameraForward = mainCamera.transform.forward;

                cameraForward.y = 0;
                cameraForward.Normalize();

                rollDirection = cameraForward;

                transform.forward = rollDirection;
            }
            else
            {
                rollDirection = transform.forward;
            }

            StartCoroutine(PerformRoll(rollDirection));
        }
    }

    private IEnumerator PerformRoll(Vector3 rollDirection)
    {
        isRolling = true;

        float startTime = Time.time;

        while(Time.time < startTime + rollDistance / rollSpeed)
        {
            rb.MovePosition(rb.position + rollDirection * rollSpeed * Time.deltaTime);
            yield return null;
        }

        isRolling = false;
    }
}
