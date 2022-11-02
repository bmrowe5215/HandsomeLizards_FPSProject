using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recoil : MonoBehaviour
{
    public Vector3 upRecoil;
    public float recoilTime;
    Vector3 originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        originalRotation = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Recoil()
    {
        addRecoil();
        yield return new WaitForSeconds(recoilTime);
        stopRecoil();
    }

    private void addRecoil()
    {
        transform.localEulerAngles += upRecoil;
    }

    private void stopRecoil()
    {
        transform.localEulerAngles = originalRotation;
    }
}
