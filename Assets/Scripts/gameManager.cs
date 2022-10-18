using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    public int enemyNum;

    [Header("----- Player Stuff -----")]
    public GameObject player;
    public GameObject spawnPos;
    public playerController playerScript;

    [Header("----- Prop Handler -----")]
    public ParticleSystem explosion;

    [Header("----- UI -----")]
    public GameObject pauseMenu;
    public GameObject playerDeadMenu;
    public GameObject winMenu;
    public GameObject menuCurrentlyOpen;
    public GameObject playerDamageFlash;
    public GameObject playerHealthFlash;
    public GameObject victoryBanner;
    public Image playerHPBar;
    public TextMeshProUGUI enemyCount;
    public TextMeshProUGUI timerText;
    public bool killCheckToggle;
    public bool pauseTimer;
    public float timer;

    public bool isPaused;
    public bool openedMenu;


    void Awake()
    {
        timer = 0;
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        spawnPos = GameObject.FindGameObjectWithTag("Spawn Position");
        victoryBanner = GameObject.FindGameObjectWithTag("Win");

        if (victoryBanner == null)
        {
            killCheckToggle = true;
            gameManager.instance.enemyCount.enabled = killCheckToggle;
        }
        else
        {
            killCheckToggle = false;
            gameManager.instance.enemyCount.enabled = killCheckToggle;
        }

    }

    void Update()
    {
        if (!pauseTimer)
        {
            timer += Time.deltaTime;
            speedruntimer(timer);
        }
        if (Input.GetButtonDown("Cancel") && !playerDeadMenu.activeSelf && !winMenu.activeSelf)
        {
            isPaused = !isPaused;
            openedMenu = true;
            Debug.Log($"Opened menu = {openedMenu}");
            pauseMenu.SetActive(isPaused);

            if (isPaused)
                cursorLockPause();
            else
                cursorUnlockUnpause();
        }
    }

    public void cursorLockPause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        openedMenu = true;
        pauseTimer = true;
    }

    public void cursorUnlockUnpause()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        openedMenu = false;
        pauseTimer = false;
    }

    public IEnumerator playerDamage()
    {
        playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerDamageFlash.SetActive(false);
    }

    public IEnumerator playerHeal()
    {
        gameManager.instance.playerScript.updatePlayerHUD();
        playerHealthFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerHealthFlash.SetActive(false);
    }

    public void checkEnemyTotal()
    {            
        if (killCheckToggle == true)
        {
            enemyNum--;
            enemyCount.text = gameManager.instance.enemyNum.ToString("F0");
            if (enemyNum <= 0)
            {
                winMenu.SetActive(true);
                cursorLockPause();
            }
        }
    }

    public void speedruntimer(float time)
    {
        time += 1;
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        timerText.text = "Time: " + string.Format("{0:00}:{1:00}",minutes,seconds);
        
    }
    public void resetTimer()
    {
        timer = 0;
    }
}
