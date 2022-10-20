using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lavaRising : MonoBehaviour
{
    Vector3 lavaPos;
    Vector3 startPos;
    [SerializeField] float yPos;
    [SerializeField] float yPosMax;
    [SerializeField] float resetValue;
    [SerializeField] float riseRate;

    void Start()
    {
        lavaPos = gameObject.transform.position;
        startPos = lavaPos;
        StartCoroutine(lavaRise());
    }

    // Update is called once per frame
    void Update()
    {
        yPos = gameObject.transform.position.y;
    }

    public void OnTriggerEnter(Collider other)
    {
        // Kills both enemies and players.
        if (other.gameObject.tag == "Player" || other.CompareTag("Enemy"))
        {
            if (other.GetComponent<IDamage>() != null)
            {
                other.GetComponent<IDamage>().takeDamage(9999);
            }
        }
    }

    // lerpDuration  -  riseRate
    // startValue    -  yPos
    // endValue      -  yPosMax
    // valueToLerp   -  lavaPos.y

    IEnumerator lavaRise()
    {
        //float lerpDur;
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

    public void LavaReset()
    {
        //sets the position of the lava below the checkpoint.
        lavaPos.y = gameManager.instance.spawnPos.transform.position.y - resetValue;
    }
}
