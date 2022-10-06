using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour , IDamage
{
    [Header("----- Components -----")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] SphereCollider sightCollider;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int HP;
    [SerializeField] int facePlayerSpeed;
    [SerializeField] int sightDist;
    [Range(0, 3)][SerializeField] float damageFreeze;

    [Header("----- Gun Stats -----")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject shootPos;

    bool isShooting;
    public bool playerInRange;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.enemyNum++;
        gameManager.instance.enemyCount.text = gameManager.instance.enemyNum.ToString("F0");
        sightCollider.radius = sightDist;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.enabled && playerInRange)
        {
            agent.SetDestination(gameManager.instance.player.transform.position);

            if (!isShooting)
                StartCoroutine(shoot());
        }
    }

    public void takeDamage(int dmg) 
    {
        HP -= dmg;
        StartCoroutine(flashDamage());
        if (HP <= 0)
        {
            gameManager.instance.checkEnemyTotal();
            Destroy(gameObject);
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPos.transform.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        agent.enabled = false;
        yield return new WaitForSeconds(damageFreeze);
        model.material.color = Color.white;
        agent.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
