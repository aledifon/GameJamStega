using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;

    [SerializeField] private float travelCycleTime;
    private float travelSpeed;
    private float pinpongLength;
    
    Player player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Calculate travelSpeed in func. of the CycleTime and the length
        float distance = Vector3.Distance(startPos.position, endPos.position);  // Distance between Start and End points
        travelSpeed = distance / travelCycleTime;                               // Speed = Distance / Time

        // travelCycleTime = (2*pinpongLength)/travelSpeed, 
        pinpongLength = (travelCycleTime * travelSpeed) / 2;        
    }
    void FixedUpdate()
    {
        // Normalized PingPong Length time value
        float t = Mathf.PingPong(Time.time * travelSpeed, pinpongLength)/pinpongLength;

        // Continuous update of the next Local Position (in func. of time)
        Vector3 nextLocalPos = Vector3.Lerp(startPos.localPosition, endPos.localPosition, t);

        // Apply the Movement to the Rb's platform
        rb.MovePosition(transform.parent.TransformPoint(nextLocalPos));

        // Sync the platform's rotation whith its parent(Level) rotation
        //transform.rotation = transform.parent.rotation;
        rb.MoveRotation(transform.parent.rotation);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {            
            player = collision.transform.GetComponent<Player>();
            player.SetRbPlatformAngularDrag();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.ResetRbAngularDrag();
        }
    }    
}
