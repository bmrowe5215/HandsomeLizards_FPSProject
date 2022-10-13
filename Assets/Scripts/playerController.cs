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
    [Range(1, 15)][SerializeField] int HP;
    [Range(1,15)] [SerializeField] float playerSpeed;
    [Range(5,15)] [SerializeField] float jumpHeight;
    [SerializeField] float gravityValue;
    [SerializeField] int jumpsMax;

    [Header("----- Gun Stats -----")]
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] int shootDmg;
    [SerializeField] GameObject gunModel;
    [SerializeField] List<gunStats> gunStat = new List<gunStats>();

    private Vector3 playerVelocity;
    private int timesJumped;
    bool isShooting;
    int selectGun;
    int HPOrig;
    float fovOriginal;

    private void Start()
    {
        fovOriginal = playerCamera.fieldOfView;
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


        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && timesJumped < jumpsMax)
        {
            timesJumped++;
            playerVelocity.y = jumpHeight;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    //Simple FOV changer to simulate aiming down the sights
    IEnumerator aimDownSights()
    {
        //i'm gonna be honest, I (bernardo) don't like toggle aim so you gotta press n hold to zoom.
        if (gunStat.Count > 0 && Input.GetButtonDown("Fire2") && !gameManager.instance.openedMenu)
        {
            if (playerCamera.fieldOfView != fovOriginal)
            {
                StartCoroutine(LerpFOV(false));
                //playerCamera.fieldOfView = fovOriginal;
            }
            else
            {
                //Transition the movement
                StartCoroutine(LerpFOV(true));
                //playerCamera.fieldOfView = 15f;
            }
            //to make sure that i actually scripted this right
            Debug.Log("ZOOM!");
            yield return new WaitForSeconds(1);
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

    
}