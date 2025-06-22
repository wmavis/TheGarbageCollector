using UnityEngine;
using UnityEngine.UIElements;

public class AimCamera : MonoBehaviour
{
    public Transform player;
    public Transform gun;

    private float mouseX, mouseY;
    public float mouseSensitivity = 2.0f;

    public bool isAiming = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mouseX = player.eulerAngles.y;
        mouseY = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAiming)
        {
            mouseX += Input.GetAxis("Mouse X") * mouseSensitivity;
            mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            mouseY = Mathf.Clamp(mouseY, -35, 60);
        }

        transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);

        if (isAiming)    
        {
            Vector3 aimDirection = transform.forward;
            gun.forward = aimDirection;
        }
    }

    public void StartAiming()
    {
        isAiming = true;
        mouseX = player.eulerAngles.y;
    }

    public void StopAiming()
    {
        isAiming = false;
    }
}
