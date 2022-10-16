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
    public playerController playerScript;
    public GameObject spawnPos;

    [Header("----- UI -----")]
    public GameObject pauseMenu;
    public GameObject playerDeadMenu;
    public GameObject winMenu;
    public GameObject menuCurrentlyOpen;
    public GameObject playerDamageFlash;
    public GameObject playerHealthFlash;
    public Image playerHPBar;
    public TextMeshProUGUI enemyCount;

    public bool isPaused;
    public bool openedMenu;
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        spawnPos = GameObject.FindGameObjectWithTag("Spawn Position");
    }

    void Update()
    {
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
        
    }

    public void cursorUnlockUnpause()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        openedMenu = false;
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
        enemyNum--;
        gameManager.instance.enemyCount.text = gameManager.instance.enemyNum.ToString("F0");
        if (enemyNum <= 0)
        {
            winMenu.SetActive(true);
            cursorLockPause();
        }
    }
}
