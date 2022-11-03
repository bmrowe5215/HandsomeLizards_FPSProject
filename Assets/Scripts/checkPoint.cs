using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPoint : MonoBehaviour
{
    public GameObject spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = gameManager.instance.spawnPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spawnPoint.transform.position = gameObject.transform.position;
            gameManager.instance.updateSpawn();
            gameManager.instance.popUpAnim.SetTrigger("trigger");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.popUpAnim.ResetTrigger("trigger");
        }
    }
}
