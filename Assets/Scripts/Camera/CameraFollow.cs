using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform ballTransform;
    [SerializeField] private float speed;    
    private Vector3 offset;

    void Start()
    {
        // Init distance between the camera and the ball
        offset = transform.position - ballTransform.position;
    }
    
    void Update()
    {
        //Vector3 targetCamPos = ballTransform.position + offset;
        //transform.position = Vector3.Lerp(transform.position, targetCamPos, speed * Time.deltaTime);
        //transform.position = targetCamPos;

        transform.LookAt(ballTransform);
    }
}
