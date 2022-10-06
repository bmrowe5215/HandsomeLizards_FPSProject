using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    //this is gonna call back to the game manager, say "hey, game's paused. let these buttons do things to unpause and or quit the game"
    public void resume()
    {
        gameManager.instance.unpause();
    }

    public void restart()
    {
        gameManager.instance.unpause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void quit()
    {
        Application.Quit();
    }

    public void respawn()
    {
        //gameManager.instance.playerScript.whatever because its not done yet.
        //gameManager.instance.unpause(); karen asks the manager to unpause her game.
    }
}
