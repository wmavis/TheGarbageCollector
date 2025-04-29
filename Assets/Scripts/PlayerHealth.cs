using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public float health = 100;


    public void TakeDamage(float damageAmount)
    {
        if(health > damageAmount)
        {
            health -= damageAmount;
            healthText.text = health.ToString();

            if (health <= 0)
            {
                // dead
            }
        }
        else
        {
            // dead
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "HealthPack")
        {
            health += 20;
            healthText.text = health.ToString();
        }
    }
}
