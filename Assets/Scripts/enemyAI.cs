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
    //[SerializeField] GameObject headHitbox;
    // ignore this this was cringe.
    [SerializeField] AudioSource enemyHurt;
    [SerializeField] Animator anim;
    [SerializeField] Collider col;
    [SerializeField] Collider head;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int HP;
    [SerializeField] float speedChase;
    [SerializeField] int facePlayerSpeed;
    [SerializeField] int animLerpSpeed;
    [SerializeField] int sightDist;
    [SerializeField] int roamDist;
    [Range(0, 180)][SerializeField] int viewAngle;
    [SerializeField] GameObject headPos;
    [Range(0, 3)][SerializeField] float damageFreeze;

    [Header("----- Gun Stats -----")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject shootPos;

    bool isShooting;
    bool isDamaged;
    public bool playerInRange;
    Vector3 playerDir;
    Vector3 startingPos;
    float stoppingDistOrig;
    float angle;
    float speedPatrol;

    // Start is called before the first frame update
    void Start()
    {
        //gameManager.instance.enemyNum++;
        //gameManager.instance.enemyCount.text = gameManager.instance.enemyNum.ToString("F0");
        stoppingDistOrig = agent.stoppingDistance;
        startingPos = transform.position;
        speedPatrol = agent.speed;
        roam();
    }

    // Update is called once per frame
    void Update()
    {
        if (HP > 0)
        {
            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * animLerpSpeed));

            //Debug.Log(angle);
            if (agent.enabled)
            {
                if (playerInRange)
                {
                    playerDir = gameManager.instance.player.transform.position - headPos.transform.position;
                    angle = Vector3.Angle(playerDir, transform.forward);
                    canSeePlayer();
                }
                if (agent.remainingDistance < 0.1f && agent.destination != gameManager.instance.player.transform.position)
                    roam();
            }
        }
    }

    void roam()
    {
        agent.stoppingDistance = 0;
        agent.speed = speedPatrol;

        Vector3 randomDir = Random.insideUnitSphere * roamDist;
        randomDir += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDir, out hit, 1, 1);
        NavMeshPath path = new NavMeshPath();

        agent.CalculatePath(hit.position, path);
        agent.SetPath(path);
    }

    void canSeePlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(headPos.transform.position, playerDir, out hit, sightDist))
        {
            Debug.DrawRay(headPos.transform.position, playerDir);

            if (hit.collider.CompareTag("Player") && angle <= viewAngle)
            {
                agent.speed = speedChase;
                agent.stoppingDistance = stoppingDistOrig;
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (agent.remainingDistance < agent.stoppingDistance)
                    facePlayer();

                if (!isShooting)
                    StartCoroutine(shoot());
            }
        }
        else if (agent.remainingDistance < 0.1f && agent.destination != gameManager.instance.player.transform.position)
            roam();
    }

    void facePlayer()
    {
        playerDir.y = 0;
        Quaternion rotation = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * facePlayerSpeed);
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;

        if (HP <= 0)
        {
            col.enabled = false;
            agent.enabled = false;
            head.enabled = false;
            gameManager.instance.checkEnemyTotal();
            anim.SetBool("Dead", true);
        }
        else
            if (!isDamaged)
                StartCoroutine(flashDamage());
    }

    IEnumerator shoot()
    {
        isShooting = true;
        anim.SetTrigger("Shoot");
        Instantiate(bullet, shootPos.transform.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator flashDamage()
    {
        if (agent.isActiveAndEnabled)
        {
           isDamaged = true;
           anim.SetTrigger("Damage");
           model.material.color = Color.red;
           agent.enabled = false;
           yield return new WaitForSeconds(damageFreeze);
           model.material.color = Color.white;
           agent.enabled = true;
           agent.SetDestination(gameManager.instance.player.transform.position);
           isDamaged = false;
        }
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
            agent.stoppingDistance = 0;
        }
    }
}
