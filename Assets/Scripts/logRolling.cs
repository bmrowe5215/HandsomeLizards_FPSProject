using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logRolling : MonoBehaviour
{
    //[SerializeField] GameObject trigger;
    [SerializeField] GameObject log;
    [SerializeField] int destroyTime;
    //[SerializeField] GameObject logSpawn;
    //Vector3 spawnPos;
    // Start is called before the first frame update
    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamage>().takeDamage(99);
        }
    }

}
