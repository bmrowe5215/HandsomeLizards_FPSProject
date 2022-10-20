using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logSpawner : MonoBehaviour
{
    [SerializeField] GameObject trigger;
    [SerializeField] GameObject log;
    [SerializeField] GameObject logSpawn;
    bool spawned;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && spawned == false)
        {
            Instantiate(log, logSpawn.transform.position, log.transform.rotation);
            Destroy(log, 5f);
            spawned = true;
        }
    }
}
