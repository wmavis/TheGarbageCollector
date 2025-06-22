using TMPro;
using UnityEngine;

public class ShootingSystem : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammoText;
    public float fireRate = 1f;
    private float nextTimeToFire = 0f;
    public float bullets = 5;
    public float bulletVelocity = 20f;

    public AudioClip shootSFX;
    AudioSource audioSource;
    private Camera combatCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); 
        combatCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        if(Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            FireBullet();
        }
    }

    private void FireBullet()
    {
        if(bullets > 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.transform.localScale = new Vector3(100, 100, 100);
            GameObject muzzle = Instantiate(muzzleFlash, firePoint.position, firePoint.rotation);
            Rigidbody bulletRb = bullet.transform.GetChild(0).GetComponent<Rigidbody>();

            bullets--;
            ammoText.text = bullets.ToString();

            RaycastHit hit;
            Vector3 linearVelocity;
            if (Physics.Raycast(combatCamera.transform.position, combatCamera.transform.forward, out hit, Mathf.Infinity))
            {
                /*
                LineRenderer lr = bullet.AddComponent<LineRenderer>();
                lr.positionCount = 2;
                lr.SetPosition(0, combatCamera.transform.position);
                lr.SetPosition(1, combatCamera.transform.position + combatCamera.transform.forward * hit.distance);
                */
                //Debug.DrawRay(combatCamera.transform.position, combatCamera.transform.forward * hit.distance, Color.yellow);
                Debug.Log("Did Hit: " + combatCamera.transform.position + " - " + (combatCamera.transform.forward * hit.distance) + " - " + hit.distance);
                linearVelocity = (hit.point - firePoint.position).normalized * bulletVelocity;
            }
            else
            {
                Debug.DrawRay(combatCamera.transform.position, combatCamera.transform.forward * 1000, Color.white);
                Debug.Log("Did not Hit");
                linearVelocity = transform.forward * bulletVelocity;
            }

            if (bulletRb != null)
            {
                bulletRb.linearVelocity = linearVelocity;
            }

            /*
            LineRenderer lr = bullet.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.SetPosition(0, combatCamera.transform.position);
            lr.SetPosition(1, firePoint.position + 100*bulletRb.linearVelocity);
            */

            audioSource.Stop();
            audioSource.clip = shootSFX;
            audioSource.time = 0.8f;
            audioSource.Play();

            Destroy(bullet, 10f);
            Destroy(muzzle, 2f);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ammo")
        {
            Destroy(other.gameObject);

            bullets += 5;
            ammoText.text = bullets.ToString();
        }
    }
}
