using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPRadius : MonoBehaviour
{
    Rigidbody[] rbs;
    

   public void enemyGroundPoundCheck(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Debug.Log("Enemy in Groundpound");
            foreach (var item in rbs)
            {
                item.AddForce(0, 10, 0, ForceMode.Impulse);
            }
        }
    }
}
