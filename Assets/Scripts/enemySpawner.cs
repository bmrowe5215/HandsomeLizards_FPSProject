using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject enemy;
    [SerializeField] int enemyCount;
    [SerializeField] float minRadius;
    [SerializeField] float maxRadius;
     float xPos;
     float zPos;
    private bool spawned = false;

    void Start()
    {
        gameManager.instance.enemyCount.text = gameManager.instance.enemyNum.ToString("F0");
        gameManager.instance.enemyNum += enemyCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && spawned == false)
        {
            for (int i = 0; i < enemyCount; i++)
            {
                StartCoroutine(spawnEnemy());
                gameManager.instance.enemyCount.text = gameManager.instance.enemyNum.ToString("F0");
            }
            spawned = true;
        }
    }

    IEnumerator spawnEnemy()
    {
        xPos = Random.Range(minRadius, maxRadius);
        zPos = Random.Range(minRadius, maxRadius);
        // TL;DR spawns enemy within a range that can be set around the trigger's game object location.
        Instantiate(enemy, new Vector3(gameObject.transform.position.x+xPos, gameObject.transform.position.y,gameObject.transform.position.z+zPos), gameObject.transform.rotation);
        yield return new WaitForSeconds(2f);
    }
}
