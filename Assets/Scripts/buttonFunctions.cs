using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void resume()
    {
        gameManager.instance.cursorUnlockUnpause();
        gameManager.instance.isPaused = false;
        gameManager.instance.pauseMenu.SetActive(false);
        gameManager.instance.menuUIAudio.PlayOneShot(gameManager.instance.debugBruh);

    }

    public void playGame()
    {
        SceneManager.LoadScene((int)gameManager.levelID.tutorial);
        gameManager.instance.menuUIAudio.PlayOneShot(gameManager.instance.debugBruh);
        gameManager.instance.cursorUnlockUnpause();

    }

    public void options()
    {
        gameManager.instance.mainMenuSubmenu.SetActive(false);
        gameManager.instance.optionsMenu.SetActive(true);
        gameManager.instance.menuCurrentlyOpen = gameManager.instance.optionsMenu;
        gameManager.instance.menuUIAudio.PlayOneShot(gameManager.instance.debugBruh);

    }

    public void goBack()
    {
        gameManager.instance.menuCurrentlyOpen.SetActive(false);
        gameManager.instance.mainMenuSubmenu.SetActive(true);
        gameManager.instance.menuUIAudio.PlayOneShot(gameManager.instance.debugBruh);

    }

    public void restart()
    {
        gameManager.instance.cursorUnlockUnpause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.menuUIAudio.PlayOneShot(gameManager.instance.debugBruh);
        gameManager.instance.resetTimer();
    }

    public void quit()
    {
        Application.Quit();
    }
    public void quitToMenu()
    {
        //this should just take you to the main menu scene *hopefully*
       SceneManager.LoadScene((int)gameManager.levelID.mainmenu);
    }

    public void credits()
    {
        gameManager.instance.menuCurrentlyOpen = gameManager.instance.creditsMenu;
        gameManager.instance.mainMenuSubmenu.SetActive(false);
        gameManager.instance.creditsMenu.SetActive(true);
        
    }
    public void respawn()
    {
        //gameManager.instance.menuUIAudio.PlayOneShot(gameManager.instance.debugBruh);
        gameManager.instance.playerScript.respawn();
        gameManager.instance.cursorUnlockUnpause();
        gameManager.instance.lava.GetComponent<lavaRising>().LavaReset();
    }
}
