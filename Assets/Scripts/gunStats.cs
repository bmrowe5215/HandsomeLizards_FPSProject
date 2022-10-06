using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class gunStats : ScriptableObject
{
    public float shootRate;
    public int shootDist;
    public int shootDmg;
    public int ammoCount;
    public GameObject gunModel;
    public AudioClip gunClip;
    public GameObject hitEffect;
    public GameObject bulletEffect;
    
}
