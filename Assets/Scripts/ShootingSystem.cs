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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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
            GameObject muzzle = Instantiate(muzzleFlash, firePoint.position, firePoint.rotation);
            Rigidbody bulletRb = bullet.transform.GetChild(0).GetComponent<Rigidbody>();

            bullets--;
            ammoText.text = bullets.ToString();

            if(bulletRb != null)
            {
                bulletRb.linearVelocity = transform.forward * bulletVelocity;
            }

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
