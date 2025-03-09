using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : MonoBehaviour
{
    private Transform parent;
    //private float fixedYdistance;

    private Vector3 initialLocalPosition;

    // Start is called before the first frame update
    void Start()
    {
        // Get the parent GO ref.
        parent = transform.parent;
        // Get the initial Distance between the hat and the ball
        //fixedYdistance = transform.position.y - parent.position.y;

        // Guarda la posición inicial relativa al padre
        initialLocalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Mantener la posición X y Z respecto al padre, pero bloquear Y
        //Vector3 newPosition = parent.position + new Vector3(initialLocalPosition.x, 0, initialLocalPosition.z);
        transform.position = new Vector3(parent.position.x, parent.position.y + initialLocalPosition.y, parent.position.z);


        // Only allows Y Axis rotations
        transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y,0);
    }
}
