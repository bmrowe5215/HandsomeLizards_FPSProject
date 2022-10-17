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
            Debug.Log($"{gameManager.instance.killCheckToggle}");
           gameManager.instance.winMenu.SetActive(true);
           gameManager.instance.cursorLockPause();
        }
    }

}
