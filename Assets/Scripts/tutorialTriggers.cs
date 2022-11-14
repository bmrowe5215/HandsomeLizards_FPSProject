using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialTriggers : MonoBehaviour
{
    //Array of a bunch of gameObjects that i'm gonna use to contain the tutorial messages.
    [SerializeField] GameObject textBackground;
    GameObject currentlyOpened;
    public GameObject[] TutorialMessages;
    [SerializeField] triggerIDs triggerid;
    // Plan of action:
    // Player Walks through triggers that have numerical values assigned to each of them.
    // Switch case statement goes through, taking whatever numerical value the trigger has and displaying the corresponding UI Message.
    // After about idk 15 seconds? the message dissappears. I could probably use Instantiate a UI thing and use DestroyObject(); which would be pretty bangin.
    // or maybe do a thing where it just sets it active for a set amount of time. and I can prevent overlaps by having a
    // currentlyOpened = TutorialMessage[triggerID] 
    //which btw public enums have a dropdown apparently and thats REALLY sick.
    // But thats my plan of action when I'm not conked out of my gourd. Future me I hope this makes sense.
    enum triggerIDs
    {
        //oopsies ahaha forgot to make this 0 based oopsies ahaha oops.
        basic = 0,
        lava, 
        enemy,
        groundpound,
        jumpPad,
        Traps,

    }

    // Start is called before the first frame update
    IEnumerator OnTriggerEnter(Collider other)
    {
        // check if player
        if (other.CompareTag("Player"))
        {
            if (gameManager.instance.openTutorial == true)
            {
                yield return new WaitUntil(() => gameManager.instance.openTutorial == false);
            }
            //Problem: If you hit multiple triggers in a row, it gets really messy.
            //Solution: Enter a trigger, if theres a message already being displayed then disable it.

            //stores which tutorial message is currently on screen.
            currentlyOpened = TutorialMessages[(int)triggerid];

            gameManager.instance.openTutorial = true;

            //Turns on Background
            textBackground.SetActive(true);
            //Fetches the selected enum, and the correspoding message
            TutorialMessages[(int)triggerid].SetActive(true);
            //Because OnTriggerEnter can be a IEnumerator you can have a baked in timer, which is BALLER AS HELL.
            // making this whole thing possible. Absolutely Goated
            // it also allows for people to walk back into it if they want to re learn something. This method is goated and can be used universally in any level.
            yield return new WaitForSeconds(4f);

            textBackground.SetActive(false);
            TutorialMessages[(int)triggerid].SetActive(false);
            gameManager.instance.openTutorial = false;
        }
    }

}
