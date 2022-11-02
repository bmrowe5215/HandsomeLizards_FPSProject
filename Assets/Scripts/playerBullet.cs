using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBullet : MonoBehaviour
{

    [SerializeField] Rigidbody rb;
    [SerializeField] bool barrelDamageToggle;
    [SerializeField] int speed;
    [SerializeField] int damage;
    [SerializeField] int destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            if (other.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<enemyAI>().takeDamage(damage);
                Destroy(gameObject);

            }
            if (other.CompareTag("Prop") && barrelDamageToggle == true)
            {
                other.gameObject.GetComponent<IDamage>().takeDamage(damage);
            }
        }
    }
}
