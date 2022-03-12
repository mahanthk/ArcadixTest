using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Quaternion cameraRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cameraRotation.y += Input.GetAxis("Mouse Y");
        cameraRotation.y = Mathf.Clamp(cameraRotation.y, -50, 20);
        cameraRotation.x += Input.GetAxis("Mouse X");
        transform.rotation = Quaternion.Euler(-cameraRotation.y, cameraRotation.x, cameraRotation.z);
    }
}
