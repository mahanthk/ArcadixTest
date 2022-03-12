using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerAnimationController controller;
    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.canMove)
        {
            transform.Translate(new Vector3(0, 0, Input.GetAxis("Vertical")) * (Input.GetKey(KeyCode.LeftShift) ? speed * 2 : speed) * Time.deltaTime);
        }
    }
}
