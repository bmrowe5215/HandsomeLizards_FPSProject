using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpPad : MonoBehaviour
{
    [SerializeField] AudioSource warp;
    [SerializeField] GameObject pad;
    
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.jumpPad();
            AudioSource.PlayClipAtPoint(warp.clip, pad.transform.position, 0.1f);
        }
    }
}
