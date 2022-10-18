using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryBannerScript : MonoBehaviour
{
    [SerializeField] public bool flagCheckToggle = true; 
    
    private void OnTriggerEnter(Collider other)
    {
        //gameManager.instance.killCheckToggle = !flagCheckToggle;
        if (flagCheckToggle)
        {
            //so if the flag check toggle is TRUE, kill check toggle is FALSE. and vice versa.
            // actually it isn't that deep and I don't need to do that, just check if this flag is in the level already
            //and toggle between kill goal and "reach the flag" goal
            Debug.Log($"{gameManager.instance.killCheckToggle}");
           gameManager.instance.winMenu.SetActive(true);
           gameManager.instance.cursorLockPause();
        }
    }

}
