using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Transform cameraPos;
    [SerializeField] private Transform cameraParent;
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask pickableObjectLayer;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private Transform pickedObjectParent;
    [SerializeField] private Text pickUpText;


    #region private fields non-serializefields
    private float vertical;
    private float smoothTime = 0.1f;
    private float turnSmoothVelocity;
    private float targetAngle;
    private float dampAngle;
    private GameObject currentPickedObject;
    private GameObject currentPickableObject;
    #endregion

    #region public fields
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isRunning;
    [HideInInspector] public bool isPickable;
    [HideInInspector] public Vector3 direction;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        cameraParent.position = cameraPos.position;
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        cameraParent.position = cameraPos.position;
        //transform.rotation = Quaternion.Euler(0, cameraParent.eulerAngles.y, 0);
        vertical = Input.GetAxis("Vertical");
        isRunning = Input.GetKey(KeyCode.LeftShift);
        isGrounded = Isgrounded();
        direction = new Vector3(0, 0, vertical).normalized;
        targetAngle = cameraParent.eulerAngles.y;
        dampAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0, dampAngle, 0);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerRb.AddForce(Vector3.up * 90, ForceMode.Impulse);
        }

        currentPickableObject = PickableThing();
        PickUpThing();
        

    }

    private void LateUpdate()
    {
        
    }

    bool Isgrounded()
    {
        return Physics.Raycast(groundCheckPos.position, Vector3.down, 0.01f, groundLayer);
    }

    public void PickUpThing()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(currentPickedObject != null)
            {
                currentPickedObject.transform.parent = null;
                currentPickedObject.GetComponent<Rigidbody>().isKinematic = false;
                currentPickedObject.GetComponent<Rigidbody>().detectCollisions = true;
            }
            if(currentPickableObject != null)
            {
                currentPickedObject = currentPickableObject;
                currentPickedObject.transform.position = pickedObjectParent.position;
                currentPickedObject.GetComponent<Rigidbody>().isKinematic = true;
                currentPickedObject.GetComponent<Rigidbody>().detectCollisions = false;
                currentPickedObject.transform.parent = pickedObjectParent;
                currentPickableObject = null;
            }
        }
    }

    private GameObject PickableThing()
    {
        Vector3 p1 = transform.position + capsuleCollider.center + Vector3.up * -capsuleCollider.height * 0.5f;
        Vector3 p2 = p1 + Vector3.up * capsuleCollider.height;
        RaycastHit hit;
        if(Physics.CapsuleCast(p1, p2, capsuleCollider.radius, transform.forward, out hit, 0.5f, pickableObjectLayer))
        {
            Debug.Log(hit.collider.tag);
            isPickable = true;
            pickUpText.text = $"Press E to pick up the {hit.collider.tag}.";
            return hit.collider.gameObject;
        }
        else
        {
            isPickable = false;
            pickUpText.text = "";
        }
        return null;
    }
}
