using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinAnimEvent : MonoBehaviour
{
    // Start is called before the first frame update
    
    public void WinAnim()
    {
        gameManager.instance.cursorLockPause();
    }
}
