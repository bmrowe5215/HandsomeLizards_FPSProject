using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lavaFunctions : MonoBehaviour
{
    Vector3 lavaPos;
    [SerializeField] float yPos;
    [SerializeField] float yPosMax;

    [SerializeField] float riseRate;

    // Start is called before the first frame update
    void Start()
    {
        lavaPos = gameObject.transform.position;
        StartCoroutine(lavaRising());
    }

    // Update is called once per frame
    void Update()
    {
        //LavaRising();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameManager.instance.playerScript.takeDamage(100);
        }
    }

    // lerpDuration  -  riseRate
    // startValue    -  yPos
    // endValue      -  yPosMax
    // valueToLerp   -  lavaPos.y

    IEnumerator lavaRising()
    {
        float lerpDur ;
        //float timeElapsed = 0;
        while (lavaPos.y < yPosMax)
        {
            lavaPos.y = Mathf.Lerp(lavaPos.y, yPosMax, riseRate * Time.deltaTime);

            gameObject.transform.position = lavaPos;

            //timeElapsed += Time.deltaTime;

            yield return null;
        }

        gameObject.transform.position = lavaPos;
    }
}
