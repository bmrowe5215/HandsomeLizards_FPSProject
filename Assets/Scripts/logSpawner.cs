using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logSpawner : MonoBehaviour
{
    [SerializeField] GameObject trigger;
    [SerializeField] GameObject log;
    [SerializeField] GameObject logSpawn;
    [SerializeField] GameObject logSpawn2;
    bool spawned;
    // Start is called before the first frame update
    IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && spawned == false)
        {
            spawned = true;
            Instantiate(log, logSpawn.transform.position, log.transform.rotation);
            yield return new WaitForSeconds(5);
            Instantiate(log, logSpawn2.transform.position, log.transform.rotation);
            Destroy(log, 5f);
        }
    }
}
