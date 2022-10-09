using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class cameraControls : MonoBehaviour
{
    
    [SerializeField] int sensHort;
    [SerializeField] int sensVert;

    [SerializeField] int lockVertMin;
    [SerializeField] int lockVertMax;

    [SerializeField] bool invert;

    float xRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //get input
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensHort;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensVert;

        if (invert)
            xRotation += mouseY;
        else
            xRotation -= mouseY;

        //clamp camera rotation
        xRotation = Mathf.Clamp(xRotation, lockVertMin, lockVertMax);
        
        //rotate the camera on the X-Axis
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        //rotate the player
        transform.parent.Rotate(Vector3.up * mouseX);
    
    }
}
