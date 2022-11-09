using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IDamage
{
    [Header("----- Barrel Stats -----")]
    [SerializeField] int HP;
    [SerializeField] int explosionDamage;
    [SerializeField] float explosionRadius;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] AudioSource barrelSource;
    [SerializeField] AudioClip[] barrelExplosion;
    [SerializeField] Collider barrelPos;
    Collider[] nearEntitys;

   


    public void takeDamage(int dmg)
    {
        HP -= dmg;
        if (HP <= 0)
        {
            barrelSource.PlayOneShot(barrelExplosion[Random.Range(0, barrelExplosion.Length - 1)], 1);
            //gameObject.GetComponent<MeshFilter>().
            gameObject.GetComponent<MeshCollider>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            blowUp();
           //Debug.Log("BOOM");
            Destroy(gameObject,1.5f);
        }
    }

    void blowUp()
    {
        explosion.Play();
        nearEntitys = Physics.OverlapSphere(barrelPos.transform.position, explosionRadius, LayerMask.GetMask("Enemy", "Player"), QueryTriggerInteraction.Ignore);
        foreach (var item in nearEntitys)
        {
            item.GetComponent<IDamage>().takeDamage(explosionDamage);
        }
    }

    // Start is called before the first frame update
}
