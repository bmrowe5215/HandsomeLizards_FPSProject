using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Vector3 startPos;
    [SerializeField] GameObject point;
    [SerializeField] float speed;
    //its so scuffed.
    //[Range(0, 1)] [SerializeField] int platformVariant;
    bool toggle;
    Vector3 currentPos;
    Vector3 endpos;

  

    void Start()
    {
        currentPos = gameObject.transform.position;
        endpos = point.transform.position;
        startPos = currentPos;
        //StartCoroutine(sinMoving());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        
            currentPos = Vector3.Lerp(startPos, endpos, Mathf.PingPong(Time.time * speed, 1));
            gameObject.transform.position = currentPos;
          
    }
    //IEnumerator sinMoving()
    //{
    //    //this moves it in like a sin wave motion, but I couldn't get sin waves to work so I use pingpong instead because again im cringe;
    //    // WELP, I didn't realize that having an object as a child means that it will move WITH the object. I am dumb.

    //    //while (true)
    //    //{
    //    //    currentPos = Vector3.Lerp(startPos, endpos, Mathf.PingPong(Time.time * speed, 1));
    //    //    gameObject.transform.position = currentPos;
    //    //    yield return null;
    //    //}
    //}
    //

    private void OnTriggerEnter(Collider other)
    {
        // Sets the parent for movement to the platform for the player
        if (other.CompareTag("Player"))
        {
            gameManager.instance.player.transform.parent = transform;
        }

        //BAD. NO. SHOO. GO AWAY. BAD CODE. 
        // upon coming in contact with lava, it'll parent itself to the lava, that way as it moves it'll follow.
        // Useful for lava Rise Fall levels.
        // I don't know if this will parent the rotation, but if it DOES its gonna be so baller.
        //if (other.CompareTag("Lava") && toggle)
        //{
        //    transform.SetParent(other.transform.parent);
        //    Debug.Log($"Parent = {transform.parent}");
        //    //
        //}
    }

    //SCUFFED. do NOT parent something that has jank scale. good lord I made a mistake.
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Lava")
    //    {
    //        transform.SetParent(collision.transform.parent);
    //        Debug.Log($"Parent = {transform.parent}");
    //    }
    //}
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.player.transform.parent = null;
        }
    }

}
