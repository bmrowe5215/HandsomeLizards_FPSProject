using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] Camera playerCamera;
    [SerializeField] AudioSource gunShot;
    [SerializeField] AudioSource playerHurt;


    [Header("----- Player Stats -----")]
    [Range(1, 15)][SerializeField] public int HP;
    [Range(1,15)] [SerializeField] float playerSpeed;
    [Range(1, 15)] [SerializeField] float playerSprintSpeed;
    [Range(5,15)] [SerializeField] float jumpHeight;
    [SerializeField] float sprintFOV;
    [SerializeField] float adsFOV;
    [SerializeField] float gravityValue;
    [SerializeField] int jumpsMax;

    [Header("----- Gun Stats -----")]
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] int shootDmg;
    [SerializeField] GameObject gunModel;
    [SerializeField] List<gunStats> gunStat = new List<gunStats>();

    [Header("----- Power Up Stats -----")]
    [SerializeField] int jumpPadPower;
    

    private Vector3 playerVelocity;
    private int timesJumped;
    bool isShooting;
    bool isSprinting;
    bool isAiming;
    int selectGun;
    int HPOrig;
    float speedOrig;
    float fovOriginal;
   
   

    private void Start()
    {
        fovOriginal = playerCamera.fieldOfView;
        speedOrig = playerSpeed;
        HPOrig = HP;
        respawn();
    }

    void Update()
    {
        movement();
        StartCoroutine(shoot());
        StartCoroutine(aimDownSights());
        gunSelect();

    }

    void movement()
    {
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            timesJumped = 0;
        }

        Vector3 move = (transform.right * Input.GetAxis("Horizontal")) +
                       (transform.forward * Input.GetAxis("Vertical"));

        if (Input.GetButtonDown("Jump") && timesJumped < jumpsMax)
        {
            timesJumped++;
            playerVelocity.y = jumpHeight;
        }

        // Sprinting, isgrounded check is to make sure you can't sprint in the air (plus it only runs when you move since its in update)
        // BASE FOV: 60
        if (Input.GetKey(KeyCode.LeftShift) && controller.isGrounded && !isAiming)
        {
            Debug.Log("Sprinting");
            playerSpeed = playerSprintSpeed;
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
                //This code says "huh if i'm not aiming, then the fov is gonna be normal (ish)
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fovOriginal, Time.deltaTime * 10);
            }
            else if(isAiming)
            {
                //this code says "IM GONNA ZOOM" and does so.
                playerCamera.fieldOfView = Mathf.Lerp(adsFOV, playerCamera.fieldOfView,  Time.deltaTime * 5);
            }
            //to make sure that i actually scripted this right
            Debug.Log("ZOOM!");
            yield return new WaitForSeconds(2);
            //IT WORKS POGGERS
        }
    }

    IEnumerator shoot()
    {
        if (gunStat.Count > 0 && Input.GetButton("Shoot") && !isShooting && !gameManager.instance.openedMenu)
        {
            isShooting = true;
            RaycastHit hit;
            gunShot.PlayOneShot(gunShot.clip, 0.2f);
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f,0.5f)),out hit, shootDist))
            {
                //Instantiate(cube, hit.point, transform.rotation);
                if (hit.collider.GetComponent<IDamage>() != null)
                    hit.collider.GetComponent<IDamage>().takeDamage(shootDmg);
            }


            Debug.Log("Shoot!");
            yield return new WaitForSeconds(shootRate);
            isShooting = false;
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

    public void takeDamage(int dmg)
    {
        
        HP -= dmg;
        updatePlayerHUD();
        playerHurt.PlayOneShot(playerHurt.clip, 0.1f);
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
        updatePlayerHUD();
    }

    public void jumpPad()
    {
        playerVelocity.y = jumpHeight * jumpPadPower;
        Debug.Log("Jumppad() Triggered");

    }
    
}