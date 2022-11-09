using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    public int enemyNum;

    //Enum system of loading things, you can expand as you add more scenes to the UI menu and load scenes using the game manager.
    public enum levelID
    {
        //unity uses an index based/string based scene system. its easier to do index and enums.
        //main menu is 0 since we want that to be the first thing the player sees.
        mainmenu = 0,
        tutorial,
        still,
        rising,
        risefall,
        debug,

    }
    
    [Header("----- Player Stuff -----")]
    public GameObject player;
    public GameObject spawnPos;
    public playerController playerScript;

    [Header("----- Prop Handler -----")]
    public ParticleSystem explosion;

    [Header("----- Level Music -----")]
    public AudioClip[] levelMusic;
    public AudioClip victorySound;

    [Header("----- UI -----")]
    //the sound we play on ui button presses.
    public AudioSource menuUIAudio;
    public AudioSource menuSFXAudio;
    public AudioSource menuMusicAudio;
    public AudioClip debugBruh;
    public AudioClip debugGun;
   //
    public GameObject pauseMenu;
    public GameObject playerDeadMenu;
    public GameObject winMenu;
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject mainMenuSubmenu;
    public GameObject levelselectMenu;
    public GameObject creditsMenu;
    public GameObject menuCurrentlyOpen;
    public GameObject playerDamageFlash;
    public GameObject playerHealthFlash;
    public GameObject playerLavaFlash;
    public GameObject victoryBanner;
    public GameObject lava;
    public Image popUpWindow;
    public Animator popUpAnim;
    public Image playerHPBar;
    public Image[] jumpBars;
    public TextMeshProUGUI enemyCount;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI ammoText;
    public bool killCheckToggle;
    public bool pauseTimer;
    public float timer;
    public float ammoCounter;
    public int currentLevelID;
    public bool isPaused;
    public bool openedMenu;


    void Awake()
    {
        timer = 0;
        instance = this;
        currentLevelID = SceneManager.GetActiveScene().buildIndex;
      
        player = GameObject.FindGameObjectWithTag("Player");
        //playerScript = player.GetComponent<playerController>();
        spawnPos = GameObject.FindGameObjectWithTag("Spawn Position");
        victoryBanner = GameObject.FindGameObjectWithTag("Win");
        lava = GameObject.FindGameObjectWithTag("Lava");
        //

        if (player == null)
        {
            playerScript = null;
        }
        else
        {
            playerScript = player.GetComponent<playerController>();
        }

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
        //Check to see if there even is any music in the array.
        //Then find the song corresponding to the array's index in the level index.
        if (levelMusic[currentLevelID] != null)
        {
            menuMusicAudio.PlayOneShot(levelMusic[currentLevelID]);
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
            //Debug.Log($"Opened menu = {openedMenu}");
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

    public void updateSpawn()
    {
        spawnPos = GameObject.FindGameObjectWithTag("Spawn Position");
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
                popUpAnim.SetBool("Win", true);
                menuSFXAudio.PlayOneShot(victorySound);
                //cursorLockPause();
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
