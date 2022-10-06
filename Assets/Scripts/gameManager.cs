using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class gameManager : MonoBehaviour
{
    //create the first instance of the game manager object.
    //so when we need to access anything, "Karen" asks for the manager to handle certain tasks.
    //For example, lets say our karen was found dead, she in spirit would ask for a manager to try and resurrect her.
    //the manager pulls from the playerController script, disables it, moves the player (karen) object to the spawn position, then re-enables control.
    //thus effectively "ressurecting" our "Karen".
    public static gameManager instance;
    public GameObject player;

    //To be added when we merge player scripts, but this'll just be here commented as placeholder.
    //public playerController playerScript;


    //UI HANDLER STUFF
    [Header("===== UI SCREENS =====")]
    //karen wants a time out
    public GameObject pauseScreen;
    //karen is dead
    public GameObject deathScreen;
    //karen succeeds in her penultimate task of being cringe.
    public GameObject victoryScreen;
    //karen asks for a manager to check if a menu is open before opening up another dialogue box.
    public GameObject menuOpen;

    [Header("===== Player HUD =====")]
    //karen asks for how much health a player has, but needs it represented as a png.
    public Image playerHPBar;
    //karen asks for how many enemies/cashiers to harass to achieve her goal.
    public TextMeshProUGUI enemyCounter;
    //karen asks for how much health she has numerically, because pngs can be unreliable.
    public TextMeshProUGUI hpNumber;

    //karen timed out.
    public bool paused;


    // We use Awake instead, as the game manager wakes up before every other script starts up, and handles/deals all the future information needed/necessary.
    //also, its a singleton and does a damn good job at doing its job.
    void Awake()
    {
        //initialize the instance
        instance = this;
        //karen wants to be acknowledge as a player, (this is for the actual like player OBJECT not the SCRIPT) 
        player = GameObject.FindGameObjectWithTag("Player");
        //Karen wants to get to access to everything attached to the player controller, including functions n whatnot.
        //playerScript = player.getComponent<playerController>();
        //now we're gaming, and can make calls n stuff.
        //This is where the player stuff would go, but i'm making the game manager *before* we even have a player object.
    }

    // Update is called once per frame
    void Update()
    {
        //insert the pause menu stuff here.
        // will do capitan
        if (Input.GetButtonDown("Cancel"))
        {
            paused = !paused;
            pauseScreen.SetActive(paused);
            if (paused)
            {
                pause();
            }
            else
            {
                unpause();
            }
        }
    }
    //will bring up the menu,
    public void pause()
    {
        //this is SICK yo, you can make it .5 and it'll be slow mo.
        Time.timeScale = 0;
        Cursor.visible = paused;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void unpause()
    {
        Time.timeScale = 1;
        Cursor.visible = paused;
        Cursor.lockState = CursorLockMode.Confined;
    }

}