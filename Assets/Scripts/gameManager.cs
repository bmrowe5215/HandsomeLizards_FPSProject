using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    //create the first instance of the game manager object.
    public static gameManager instance;

    //UI HANDLER STUFF
    public GameObject pauseScreen;
    public bool paused;


    // We use Awake instead, as the game manager wakes up before every other script starts up, and handles/deals all the future information needed/necessary.
    //also, its a singleton and does a damn good job at doing its job.
    void Awake()
    {
        //initialize the instance
        instance = this;
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
