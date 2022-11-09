using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logRolling : MonoBehaviour, IDamage
{
    //[SerializeField] GameObject trigger;
    [SerializeField] GameObject log;
    [SerializeField] float HP;
    [SerializeField] int destroyTime;
    [SerializeField] bool enemyKillToggle;
    //[SerializeField] GameObject logSpawn;
    //Vector3 spawnPos;
    // Start is called before the first frame update
    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") )
        {
            other.GetComponent<IDamage>().takeDamage(99);
        }

        if (enemyKillToggle && other.CompareTag("Enemy"))
        {
            other.GetComponent<IDamage>().takeDamage(99);
        }
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;
        if (HP <= 0)
        { 
            //Debug.Log("BOOM");
            Destroy(gameObject, 1.5f);
        }
    }
}
