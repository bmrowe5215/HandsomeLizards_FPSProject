//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headShot : MonoBehaviour, IDamage
{
    [SerializeField] GameObject headBone;
    [SerializeField] ParticleSystem critEffect;
    [SerializeField] AudioSource critSource;
    [SerializeField] AudioClip critSound;
    GameObject Parent;
    private void Start()
    {
        Parent = gameObject.transform.parent.gameObject;
        gameObject.transform.position = headBone.transform.position;

    }
    public void takeDamage(int dmg)
    {
        critEffect.Play();
        critSource.PlayOneShot(critSound,0.25f);
        Parent.GetComponent<enemyAI>().takeDamage(dmg * 2);
    }

    
}

