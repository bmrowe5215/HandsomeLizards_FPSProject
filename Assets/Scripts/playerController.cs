using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;

public class playerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] Camera playerCamera;
    [SerializeField] Collider groundPoundRadius;

    [Header("----- Player Stats -----")]
    [Range(1, 15)][SerializeField] public int HP;
    [Range(1,15)] [SerializeField] float playerSpeed;
    [Range(1, 15)] [SerializeField] float playerSprintSpeed;
    [Range(5,15)] [SerializeField] public float jumpHeight;
    [Range(5, 25)] [SerializeField] float slamSpeed;
    [Range(5, 25)] [SerializeField] float slamHeight;
    [SerializeField] float sprintFOV;
    [SerializeField] float adsFOV;
    [SerializeField] float gravityValue;
    [SerializeField] int jumpsMax;

    [Header("----- Gun Stats -----")]
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] int shootDmg;
    [SerializeField] GameObject gunModel;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] List<gunStats> gunStat = new List<gunStats>();

    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] playerHurtAud;
    [Range(0, 1)][SerializeField] float playerHurtAudVol;
    [SerializeField] AudioClip[] playerStepsAud;
    [Range(0, 1)][SerializeField] float playerStepsAudVol;
    [SerializeField] AudioClip[] playerJumpAud;
    [Range(0, 1)][SerializeField] float playerJumpAudVol;

    Vector3 move;
    bool playingSteps;
    bool playingJump;
    Rigidbody[] rbs;
    private Vector3 playerVelocity;
    public int timesJumped;
    bool isShooting;
    bool isSprinting;
    bool isAiming;
    bool onGround;
    int selectGun;
    int HPOrig;
    float speedOrig;
    float gravOrig;
    float fovOriginal;
   
   

    private void Start()
    {
        fovOriginal = playerCamera.fieldOfView;
        gravOrig = gravityValue;
        speedOrig = playerSpeed;
        HPOrig = HP;
        respawn();
    }

    void Update()
    {
        movement();
        StartCoroutine(playSteps());
        StartCoroutine(shoot());
        StartCoroutine(aimDownSights());
        StartCoroutine(groundPound());
        gunSelect();

    }

    void movement()
    {
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            onGround = true;
            playerVelocity.y = 0f;
            timesJumped = 0;
        }
        else
        {
            onGround = false;
        }

        move = (transform.right * Input.GetAxis("Horizontal")) +
                       (transform.forward * Input.GetAxis("Vertical"));

        if (Input.GetButtonDown("Jump") && timesJumped < jumpsMax)
        {
            StartCoroutine(playJump());
            timesJumped++;
            playerVelocity.y = jumpHeight;
        }
        updateJumpHUD();
        // Sprinting, isgrounded check is to make sure you can't sprint in the air (plus it only runs when you move since its in update)
        // BASE FOV: 60
        if (Input.GetKey(KeyCode.LeftShift) && controller.isGrounded && !isAiming)
        {
            Debug.Log("Sprinting");
            playerSpeed = playerSprintSpeed;
            // Slower technically, but who cares
            // Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, sprintFOV, Time.deltaTime * 10);

            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, sprintFOV, Time.deltaTime * 10);
            isSprinting = true;
        }
        else 
        {
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                isSprinting = false;
            }
            if (!isSprinting && !isAiming)
            {
                playerSpeed = speedOrig;
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fovOriginal, Time.deltaTime * 10);
            }
        }
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    //Simple FOV changer to simulate aiming down the sights
    IEnumerator aimDownSights()
    {

        // turns out it is toggle aim regardless lmao
        if (gunStat.Count > 0 && Input.GetButtonDown("Fire2") && !gameManager.instance.openedMenu && !isSprinting)
        {
            //So when you rightclick, you aim down the sights and set isAiming to true;
            isAiming = !isAiming;

            if (!isAiming)
            {

                StartCoroutine(LerpFOV(false));
                //playerCamera.fieldOfView = fovOriginal;

            }
            else if (isAiming)
            {

                //Transition the movement
                StartCoroutine(LerpFOV(true));
                //playerCamera.fieldOfView = 15f;

            }
            //to make sure that i actually scripted this right
            Debug.Log("ZOOM!");
            yield return new WaitForSeconds(2);
            //IT WORKS POGGERS
        }
    }

    float lerpDuration = 0.2f;
    //float startValue = 0;
    float endValue = 15;
    float valueToLerp;

    IEnumerator LerpFOV(bool isAiming)
    {
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            if (isAiming)
            {
                playerCamera.fieldOfView = Mathf.Lerp(fovOriginal, endValue, timeElapsed / lerpDuration);
            }
            else
            {
                playerCamera.fieldOfView = Mathf.Lerp(endValue, fovOriginal, timeElapsed / lerpDuration);

            }
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        if (isAiming)
        {
            playerCamera.fieldOfView = endValue;
        }
        else
        {
            playerCamera.fieldOfView = fovOriginal;
        }
    }

    IEnumerator playSteps()
    {
        if (move.magnitude > 0.2f)
        {
            if (!playingSteps && controller.isGrounded)
            {
                playingSteps = true;
                aud.PlayOneShot(playerStepsAud[Random.Range(0, playerStepsAud.Length - 1)], playerStepsAudVol);
                if (isSprinting)
                    yield return new WaitForSeconds(0.2f);
                else
                    yield return new WaitForSeconds(0.3f);
                playingSteps = false;
            }
        }
    }
    IEnumerator playJump()
    {
        if (!playingJump)
        {
            playingJump = true;
            aud.PlayOneShot(playerJumpAud[Random.Range(0, playerJumpAud.Length - 1)], playerJumpAudVol);
            yield return new WaitForSeconds(0.2f);
            playingJump = false;
        }
    }
    IEnumerator shoot()
    {
        if (gunStat.Count > 0 && Input.GetButton("Shoot") && !isShooting && !gameManager.instance.openedMenu)
        {
            isShooting = true;
            RaycastHit hit;
            muzzleFlash.Play();
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f,0.5f)),out hit, shootDist))
            {
                //Instantiate(cube, hit.point, transform.rotation);
                if (hit.collider.GetComponent<IDamage>() != null && !hit.collider.CompareTag("weakPoint"))
                {
                    //raycast hits a collider, checks if it has IDamage, and if it ISNT a weakpoint, and if it is then do normal damage.
                    hit.collider.GetComponent<IDamage>().takeDamage(shootDmg);
                }
                else if (hit.collider.GetComponent<IDamage>() != null && hit.collider.CompareTag("weakPoint"))
                {
                    //ok so this adjustment to the shoot code should allow us to implement weakpoints on ANY enemy, and allow us to check if we hit a collider
                    //listed as a weakPoint, and do double damage to it, and it scales off of weapon damage.
                    //raycast hits a collider, checks for idamage, then do critical damage.
                    //I want to add feedback to headshots, like a critical headshot kill makes their head explode or something.
                    hit.collider.GetComponent<IDamage>().takeDamage(shootDmg);
                }
            }


            Debug.Log("Shoot!");
            yield return new WaitForSeconds(shootRate);
            muzzleFlash.Stop();
            isShooting = false;
        }
    }

    //IEnumerator groundPound()
    //{
    //    if (!onGround && Input.GetButtonDown("Crouch"))
    //    {
    //        //this sends the player flying downward.
    //        playerVelocity.y = -jumpHeight * slamSpeed;
    //        //This only fires if crouch is pressed while in the air. or it should? idk its weird.
    //        Debug.Log("GroundPound;");
    //        //
    //        //creates an array of colliders, we then filter through the colliders that contain the enemy tag, then apply physics to them.
    //        Collider[] enemyCol = Physics.OverlapSphere(groundPoundRadius.transform.position, 7.5f, LayerMask.GetMask("Enemy"), QueryTriggerInteraction.Ignore);
    //        foreach (Collider item in enemyCol)
    //        {
    //            Debug.Log(item.ToString());
    //            if (item.CompareTag("Enemy"))
    //            {
    //                // slight bug, shooting an enemy while they ARENT in the air sends them fucking flying.
    //                // OH, ITS CONSTANTLY BUILDING SPEED DOWNWARD OHHHHH.
    //                float currentPos = item.transform.position.y;
    //                Debug.Log("and SLAM!");
    //                Rigidbody rb = item.GetComponent<Rigidbody>();
    //                NavMeshAgent nv = item.GetComponent<NavMeshAgent>();
    //                enemyAnim = item.GetComponent<Animator>();
    //                //patchwork solution maybe???? so the rigid body is constantly gaining a -y velocity due to gravity, so
    //                // if I disable gravity on the rigid body before launching it, it should fix the instant transmission jutsu bug.
    //                if (rb != null)
    //                {
    //                    //rb.freezeRotation = true;
    //                    rb.useGravity = true;
    //                    nv.enabled = false;
    //                    rb.velocity = new Vector3(0, slamHeight, 0);
    //                    enemyAnim.SetBool("KnockUp",true);
    //                    yield return new WaitForSeconds(2);
    //                    //Setting Velocity to 0 when they land will prevent any weird shit from happening, like the instant transmission jutsu bug.
    //                    rb.velocity = new Vector3(0, 0, 0);
    //                    //rb.freezeRotation = false;
    //                    rb.useGravity = false;
    //                    nv.enabled = true;
    //                    enemyAnim.SetBool("KnockUp", false);
    //                }
    //            }
    //        }
    //    }
    //}


    //Holy shit this is way more optimized than whatever crazy shit i was doing earlier. LOOK AT HOW CLEAN THAT IS
    IEnumerator groundPound()
    {
        if (!onGround && Input.GetButtonDown("Crouch"))
        {
            //this sends the player flying downward.
            playerVelocity.y = -jumpHeight * slamSpeed;
            //This only fires if crouch is pressed while in the air. or it should? idk its weird.
            Debug.Log("GroundPound;");
            //
            //creates an array of colliders, we then filter through the colliders that contain the enemy tag, then apply physics to them.
            Collider[] enemyCol = Physics.OverlapSphere(groundPoundRadius.transform.position, 7.5f, LayerMask.GetMask("Enemy"), QueryTriggerInteraction.Ignore);
           
            foreach (Collider item in enemyCol)
            {
                if (item.CompareTag("Enemy"))
                {
                    item.GetComponent<Animator>().SetBool("KnockUp", true);
                    Debug.Log("Slam");
                    item.GetComponent<Rigidbody>().useGravity = true;
                    item.GetComponent<Rigidbody>().freezeRotation = true;
                    item.GetComponent<NavMeshAgent>().enabled = false;
                    item.GetComponent<Rigidbody>().AddForce(new Vector3(0, 10, 0),ForceMode.Impulse);
                    //enemyAnim.SetBool("KnockUp", true);
                }
            }
            yield return new WaitForSeconds(2.1f);
            foreach (var item in enemyCol)
            {
                if (item.CompareTag("Enemy"))
                {
                    item.GetComponent<Animator>().SetBool("KnockUp", false);
                    item.GetComponent<Rigidbody>().freezeRotation = false;
                    item.GetComponent<Rigidbody>().useGravity = false;
                    item.GetComponent<NavMeshAgent>().enabled = true;
                    item.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    
                }
            }
        }
    }
    
    

    public void gunPickup(gunStats stats)
    {
        shootRate = stats.shootRate;
        shootDist = stats.shootDist;
        shootDmg = stats.shootDmg;
        gunModel.GetComponent<MeshFilter>().sharedMesh = stats.gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = stats.gunModel.GetComponent<MeshRenderer>().sharedMaterial;
        gunStat.Add(stats);
    }

    void gunSelect()
    {
        if(gunStat.Count > 1)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectGun < gunStat.Count-1) 
            {
                selectGun++;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectGun > 0)
            {
                selectGun--;
            }

            shootRate = gunStat[selectGun].shootRate;
            shootDist = gunStat[selectGun].shootDist;
            shootDmg = gunStat[selectGun].shootDmg;
            gunModel.GetComponent<MeshFilter>().sharedMesh = gunStat[selectGun].gunModel.GetComponent<MeshFilter>().sharedMesh;
            gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunStat[selectGun].gunModel.GetComponent<MeshRenderer>().sharedMaterial;
        }
    }
  
    public void updatePlayerHUD()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / (float)HPOrig;
    }

    public void updateJumpHUD()
    {
        if (onGround)
            foreach (var item in gameManager.instance.jumpBars)
                item.enabled = true;
        else
            for (int i = timesJumped - 1; i >= 0; i--)
                gameManager.instance.jumpBars[i].enabled = false;
    }

    public void takeDamage(int dmg)
    {
        
        HP -= dmg;
        updatePlayerHUD();
        aud.PlayOneShot(playerHurtAud[Random.Range(0, playerHurtAud.Length - 1)], playerHurtAudVol);
        StartCoroutine(gameManager.instance.playerDamage());
        if (HP <= 0)
        {
            gameManager.instance.playerDeadMenu.SetActive(true);
            gameManager.instance.cursorLockPause();
        }
    }

    public void respawn()
    {
        controller.enabled = false;
        HP = HPOrig;
        updatePlayerHUD();
        transform.position = gameManager.instance.spawnPos.transform.position;
        controller.enabled = true;
        gameManager.instance.playerDeadMenu.SetActive(false);
    }

    public void heal()
    {
        HP = HPOrig;
        //updatePlayerHUD();
        StartCoroutine(gameManager.instance.playerHeal());
    }

    public void jumpPad(float jumpPadPower)
    {
        playerVelocity.y = jumpHeight * jumpPadPower;
        Debug.Log("Jumppad() Triggered");
        timesJumped = 0;
        onGround = true;
        updateJumpHUD();
        onGround = false;
    }
}