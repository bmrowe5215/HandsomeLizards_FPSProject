using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Vector3 startPos;
    [SerializeField] GameObject point;
    [SerializeField] float speed;
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
        if (other.CompareTag("Player"))
        {
            gameManager.instance.player.transform.parent = transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.player.transform.parent = null;
        }
    }

}
