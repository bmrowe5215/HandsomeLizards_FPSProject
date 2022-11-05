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
    public int ammoTracker;
    public int slotNum;
    public float reloadTime;
    public float gunFOV;
    public GameObject bullet;
    public GameObject gunModel;
    public AudioClip gunClip;
    public AudioClip reloadClip;
    public AudioClip emptyClip;
    public GameObject hitEffect;
    public ParticleSystem bulletEffect;
    public Animation recoilAnimation;
    public Animation reloadAnimation;

}
