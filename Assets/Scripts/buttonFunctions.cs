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
}
