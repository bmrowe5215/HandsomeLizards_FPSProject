using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        gameManager.instance.masterSliderNav.Select();
        gameManager.instance.mainMenuSubmenu.SetActive(false);
        gameManager.instance.optionsMenu.SetActive(true);
        gameManager.instance.menuCurrentlyOpen = gameManager.instance.optionsMenu;
        gameManager.instance.menuUIAudio.PlayOneShot(gameManager.instance.debugBruh);

    }

    public void goBack()
    {
        gameManager.instance.menuButtonNav.Select();
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

    public void continueButton()
    {
        if (SceneManager.GetActiveScene().buildIndex != (int)gameManager.levelID.debug)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            gameManager.instance.cursorUnlockUnpause();

        }
        else
        {
            SceneManager.LoadScene((int)gameManager.levelID.mainmenu);
            gameManager.instance.cursorUnlockUnpause();
        }
    }
    
    public void levelSelect()
    {
        gameManager.instance.levelSelectButtonNav.Select();
        gameManager.instance.mainMenuSubmenu.SetActive(false);
        gameManager.instance.levelselectMenu.SetActive(true);
        gameManager.instance.menuCurrentlyOpen = gameManager.instance.levelselectMenu;
        gameManager.instance.menuUIAudio.PlayOneShot(gameManager.instance.debugBruh);
    }

    public void tutorialLevel()
    {
        SceneManager.LoadScene((int)gameManager.levelID.tutorial);
        gameManager.instance.cursorUnlockUnpause();
    }

    public void stillLevel()
    {
        SceneManager.LoadScene((int)gameManager.levelID.still);
        gameManager.instance.cursorUnlockUnpause();
    }

    public void riseLevel()
    {
        SceneManager.LoadScene((int)gameManager.levelID.rising);
        gameManager.instance.cursorUnlockUnpause();
    }

    public void risefallLevel()
    {
        SceneManager.LoadScene((int)gameManager.levelID.risefall);
        gameManager.instance.cursorUnlockUnpause();
    }

    public void debugLevel()
    {
        SceneManager.LoadScene((int)gameManager.levelID.debug);
        gameManager.instance.cursorUnlockUnpause();
    }

    public void credits()
    {
        gameManager.instance.creditButtonNav.Select();
        gameManager.instance.menuCurrentlyOpen = gameManager.instance.creditsMenu;
        gameManager.instance.mainMenuSubmenu.SetActive(false);
        gameManager.instance.creditsMenu.SetActive(true);
        gameManager.instance.menuUIAudio.PlayOneShot(gameManager.instance.debugBruh);

    }
    public void respawn()
    {
        //gameManager.instance.menuUIAudio.PlayOneShot(gameManager.instance.debugBruh);
        gameManager.instance.playerScript.respawn();
        gameManager.instance.cursorUnlockUnpause();
        gameManager.instance.lava.GetComponent<lavaRising>().LavaReset();
    }
}
