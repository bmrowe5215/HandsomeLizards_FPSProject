using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lavaRising : MonoBehaviour
{
    Vector3 lavaPos;
    Vector3 startPos;
    Quaternion rotation;
    float xRot;
    float zRot;
    [SerializeField] float yPos;
    [SerializeField] float yPosMax;
    [SerializeField] float resetValue;
    [SerializeField] float riseRate;
    [SerializeField] Quaternion lavaTilt;
    [Range(0, 2)][SerializeField] int lavaVariant;
    [Header("Toggles between the rise and fall and rise only modes.")]
    //[SerializeField] bool lavaToggle;

    //Lava RiseFall Settings
    [SerializeField] float lrfStartPos;
    [SerializeField] float lrfMaxPos;



    void Start()
    {
        //xRot = gameObject.transform.rotation.x;
        //zRot = gameObject.transform.rotation.z;
        rotation = gameObject.transform.rotation;
        lavaPos = gameObject.transform.position;
        startPos = lavaPos;
        //true = lava rising 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // 0: Still Lava.
        // 1: Rising Lava.
        // 2: Rise Fall Lava.
        //This is such a cool method for level design i'm patting myself on the back for how fuckin sick this kinda is.

        yPos = gameObject.transform.position.y;
        switch (lavaVariant)
        {
            case 0:
                gameObject.transform.position = lavaPos;
                break;
            case 1:
                lavaPos.y = Mathf.Lerp(lavaPos.y, yPosMax, riseRate * Time.deltaTime);
                rotation = Quaternion.Lerp(rotation, lavaTilt, Time.deltaTime * riseRate);

                gameObject.transform.rotation = rotation;
                gameObject.transform.position = lavaPos;
                break;
            case 2:
                lavaPos.y = Mathf.Lerp(lrfStartPos, lrfMaxPos, Mathf.PingPong(Time.time * riseRate, 1));
                rotation = Quaternion.Lerp(rotation, lavaTilt, Time.deltaTime * riseRate);

                gameObject.transform.rotation = rotation;
                gameObject.transform.position = lavaPos;
                break;
        }
        gameObject.transform.position = lavaPos;

        
        //if (lavaToggle)
        //{
        //    lavaPos.y = Mathf.Lerp(lavaPos.y, yPosMax, riseRate * Time.deltaTime);
        //    gameObject.transform.position = lavaPos;
        //}
        //else
        //{
        //    lavaPos.y = Mathf.Lerp(lrfStartPos, lrfMaxPos, Mathf.PingPong(Time.time * riseRate, 1));
        //    gameObject.transform.position = lavaPos;
        //}
    }

    IEnumerator OnTriggerEnter(Collider other)
    {
        // Kills both enemies and players.
        if (!other.isTrigger)
        {
            if (other.gameObject.tag == "Player")
            {
                yield return new WaitForSeconds(0.2f);
                if (other.GetComponent<IDamage>() != null)
                {
                    other.GetComponent<IDamage>().takeDamage(9999);
                    gameManager.instance.playerLavaFlash.SetActive(true);
                }
            }
            else if (other.CompareTag("Enemy"))
            {
                other.GetComponent<IDamage>().takeDamage(9999);
            }
        }
    }

    // lerpDuration  -  riseRate
    // startValue    -  yPos
    // endValue      -  yPosMax
    // valueToLerp   -  lavaPos.y

    //IEnumerator lavaRise()
    //{
    //    //float lerpDur;
    //    //float timeElapsed = 0;
    //    while (lavaPos.y < yPosMax)
    //    {
    //        lavaPos.y = Mathf.Lerp(lavaPos.y, yPosMax, riseRate * Time.deltaTime);

    //        gameObject.transform.position = lavaPos;

    //        //timeElapsed += Time.deltaTime;

    //        yield return null;
    //    }
    //    gameObject.transform.position = lavaPos;
    //}

    // lerpDuration  -  riseRate
    // startValue    -  yPos
    // 
    // endValue      -  yPosMax
    // valueToLerp   -  lavaPos.y
    //IEnumerator lavaRiseLower()
    //{
    //    while (true)
    //    {
    //        lavaPos.y = Mathf.Lerp(lrfStartPos, lrfMaxPos, Mathf.PingPong(Time.time*riseRate,1));
    //        gameObject.transform.position = lavaPos;
    //            //Vector3.Lerp(new Vector3(0, lrfStartPos, 0), new Vector3(0,lrfMaxPos,0), (Mathf.Sin(riseRate*Time.deltaTime)+1.0f)/2.0f);
    //        yield return null;
    //    }

    //}

    public void LavaReset()
    {
        //sets the position of the lava below the checkpoint.
        Debug.Log("Lava Reset");
        if (lavaVariant != 0)
        {
            lavaPos.y = gameManager.instance.spawnPos.transform.position.y - resetValue;
        }
        Debug.Log("Lava position is now: " + lavaPos);
    }
}
