using UnityEngine;
using UnityEngine.EventSystems;

public class ThirdPersonCamera : MonoBehaviour
{
    public GameObject player;
    public float rotationSpeed = 20;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()   
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if(horizontalInput != 0f || verticalInput != 0f)
        {
            Vector3 movementDirection = player.GetComponent<ThirdPersonController>().moveDirection;
            if(movementDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, toRotation, Time.deltaTime * rotationSpeed);
            }
        }
        /*
        Vector3 viewDirection = player.transform.position - new Vector3(transform.position.x, transform.position.y, transform.position.z);
        viewDirection.Normalize();
        player.transform.forward = new Vector3(viewDirection.x, player.transform.forward.y, viewDirection.z);
        */
    }
}
