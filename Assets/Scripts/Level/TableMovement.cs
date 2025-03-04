using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableMovement : MonoBehaviour
{
    private Rigidbody rb;

    // Movement vectors
    private float horizontal;
    private float vertical;
    private float xMouse;
    private float yMouse;
    [SerializeField] private float accelerationSpeed;

    private RigidbodyConstraints rbConstraints = RigidbodyConstraints.FreezePosition | 
                                                RigidbodyConstraints.FreezeRotationY;

    [Header("Mouse Settings")]
    [SerializeField] float mouseSensitivity;
    [SerializeField] float bottomAngle;
    [SerializeField] float topAngle;
    [SerializeField] float leftAngle;
    [SerializeField] float rightAngle;
    [SerializeField] float xzRotationSpeed;

    private float desiredXRotation;
    private float currentXRotation;    
    private float rotationXVelocity;    
    private float desiredZRotation;
    private float currentZRotation;
    private float rotationZVelocity;    

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {        
        InputPlayer();
        //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);                

        //Debug.Log("Mouse Y = " + yMouse);
        //Debug.Log("Mouse X = " + xMouse);

        ExitCursorLockedMode();
    }
    private void FixedUpdate()
    {
        //Movement();
        ApplyRotation();
    }
    private void InputPlayer()
    {
        //horizontal = Input.GetAxis("Horizontal");   //AD
        //vertical = Input.GetAxis("Vertical");       //WS
        xMouse = Input.GetAxis("Mouse X");
        yMouse = Input.GetAxis("Mouse Y");        

        // Desired turn for the table around the X-axis and Z-Axis.
        desiredXRotation = desiredXRotation + (yMouse * mouseSensitivity);
        desiredZRotation = desiredZRotation - (xMouse * mouseSensitivity);

        // Limit both rotation angles
        desiredXRotation = Mathf.Clamp(desiredXRotation, bottomAngle, topAngle);
        desiredZRotation = Mathf.Clamp(desiredZRotation, -leftAngle, -rightAngle);
    }
    private void Movement()
    {
        Vector3 torqueApplied = new Vector3(vertical, 0, -horizontal) * accelerationSpeed;
        rb.AddTorque(torqueApplied, ForceMode.Impulse);

        //Vector3 torqueApplied = new Vector3(yMouse, 0, -xMouse) * accelerationSpeed;
        //rb.AddTorque(torqueApplied, ForceMode.Impulse);

        rb.constraints = rbConstraints;
        Debug.Log(torqueApplied);
    }
    private void ApplyRotation()
    {
        // Calculate the table rotation to apply around the X-Axis
        currentXRotation = Mathf.SmoothDamp(currentXRotation,
                                            desiredXRotation,
                                            ref rotationXVelocity,
                                            xzRotationSpeed);

        // Calculate the table rotation to apply around the Z-Axis
        currentZRotation = Mathf.SmoothDamp(currentZRotation,
                                            desiredZRotation,
                                            ref rotationZVelocity,
                                            xzRotationSpeed);

        // Apply the rotations around the GO
        //transform.rotation = Quaternion.Euler(currentXRotation,0,currentZRotation);

        //rb.MoveRotation(rb.rotation* Quaternion.Euler(currentXRotation, 0, currentZRotation));                
        rb.MoveRotation(Quaternion.Euler(currentXRotation, 0, currentZRotation));

        //Debug.Log("X Rotation = " + currentXRotation);
        //Debug.Log("Z Rotation = " + currentZRotation);
    }
    private void ExitCursorLockedMode()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
