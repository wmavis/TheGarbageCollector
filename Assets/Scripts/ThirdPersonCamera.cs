using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public GameObject player;
    public float rotationSpeed = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()   
    {
        Vector3 viewDirection = player.transform.position - new Vector3(transform.position.x, transform.position.y, transform.position.z);
        player.transform.forward = new Vector3(viewDirection.normalized.x, player.transform.forward.y, viewDirection.normalized.z);
    }
}
