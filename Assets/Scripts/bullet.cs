using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
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
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.takeDamage(damage);
            Destroy(gameObject);

        }
        if (other.CompareTag("Prop") && barrelDamageToggle == true)
        {
            other.gameObject.GetComponent<IDamage>().takeDamage(damage);
        }
    }
}
