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
    [Header("Toggles between the rise and fall and rise only modes.")]
    [SerializeField] bool lavaToggle;

    //Lava RiseFall Settings
    [SerializeField] float lrfStartPos;
    [SerializeField] float lrfMaxPos;



    void Start()
    {
        lavaPos = gameObject.transform.position;
        startPos = lavaPos;
        //true = lava rising 
        if (lavaToggle)
        {
            StartCoroutine(lavaRise());
        }
        else
        {
            StartCoroutine(lavaRiseLower());
        }
    }

    // Update is called once per frame
    void Update()
    {
        yPos = gameObject.transform.position.y;
    }

    IEnumerator OnTriggerEnter(Collider other)
    {
        // Kills both enemies and players.
        if (other.gameObject.tag == "Player" || other.CompareTag("Enemy"))
        {
            yield return new WaitForSeconds(0.5f);
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

    // lerpDuration  -  riseRate
    // startValue    -  yPos
    // 
    // endValue      -  yPosMax
    // valueToLerp   -  lavaPos.y
    IEnumerator lavaRiseLower()
    {
        while (true)
        {
            lavaPos.y = Mathf.Lerp(lrfStartPos, lrfMaxPos, Mathf.PingPong(Time.time*riseRate,1));
            gameObject.transform.position = lavaPos;
                //Vector3.Lerp(new Vector3(0, lrfStartPos, 0), new Vector3(0,lrfMaxPos,0), (Mathf.Sin(riseRate*Time.deltaTime)+1.0f)/2.0f);
            yield return null;
        }

    }

    public void LavaReset()
    {
        //sets the position of the lava below the checkpoint.
        lavaPos.y = gameManager.instance.spawnPos.transform.position.y - resetValue;
    }
}
