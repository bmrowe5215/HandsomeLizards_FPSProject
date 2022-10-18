using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpPad : MonoBehaviour
{
    [Header("----- Power Up Stats -----")]
    [SerializeField] float jumpPadPower;
    [SerializeField] AudioSource warp;
    [SerializeField] GameObject pad;
    
    
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.jumpPad(jumpPadPower);
            AudioSource.PlayClipAtPoint(warp.clip, pad.transform.position, 0.1f);
        }
    }
}
