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

    }

    public void options()
    {
        gameManager.instance.mainMenuSubmenu.SetActive(false);
        gameManager.instance.optionsMenu.SetActive(true);
        gameManager.instance.menuUIAudio.PlayOneShot(gameManager.instance.debugBruh);

    }

    public void goBack()
    {
        gameManager.instance.optionsMenu.SetActive(false);
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

    public void respawn()
    {
        //gameManager.instance.menuUIAudio.PlayOneShot(gameManager.instance.debugBruh);
        gameManager.instance.playerScript.respawn();
        gameManager.instance.cursorUnlockUnpause();
        gameManager.instance.lava.GetComponent<lavaRising>().LavaReset();
    }
}
