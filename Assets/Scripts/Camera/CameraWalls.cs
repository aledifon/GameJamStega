using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWalls : MonoBehaviour
{    
    [SerializeField] private Transform playerTransform;

    // Raycast Vars
    Ray ray;
    RaycastHit hit;
    private LayerMask playerLayer;
    private LayerMask wallsLayer;
    private LayerMask raycastLayer;

    // Last Wall detected
    private WallTransparency lastDetectedWall = null;

    // Start is called before the first frame update
    void Awake()
    {
        playerLayer = 1 << LayerMask.NameToLayer("Player");
        wallsLayer = 1 << LayerMask.NameToLayer("Walls");
        raycastLayer = playerLayer | wallsLayer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (playerTransform != null)
            DetectWalls();
        else
            Debug.Log("The ball was already destroyed");
    }
    void DetectWalls()
    {
        ray.origin = transform.position;
        ray.direction = playerTransform.position - transform.position;

        // Launch a raycast from the Camera to the player
        if(Physics.Raycast(ray.origin,ray.direction, out hit, Mathf.Infinity, raycastLayer))
        {
            // Get the layer hit number
            int layerHit = hit.collider.gameObject.layer;

            //if ((playerLayer & (1 << layerHit)) != 0)
            //{
            //    Debug.Log("I'm hitting the Walls");
            //}

            // When Walls Layer are detected
            if ((wallsLayer & (1 << layerHit)) != 0)
            {                
                WallTransparency wall = hit.collider.GetComponent<WallTransparency>();

                Debug.Log("I'm hitting the Wall " + wall.gameObject);

                // If a new wall is detected
                if (wall != null && wall != lastDetectedWall)
                {
                    //If there was a previous wall detected make it opaque
                    if (lastDetectedWall != null)
                    {
                        lastDetectedWall.SetWallTransparent(false);
                    }                    

                    // Set as transparent the new wall
                    wall.SetWallTransparent(true);

                    // Update the last Detected Wall with the new wall ref.
                    lastDetectedWall = wall;
                }                                
            }
            // No Walls Layers are detected
            else
            {
                // If there was any wall already detected then this one is set
                // as opaque and its reference is cleaned
                if(lastDetectedWall != null)
                {
                    lastDetectedWall.SetWallTransparent(false);
                    lastDetectedWall = null;
                }
            }
        }
        Debug.DrawRay(ray.origin,ray.direction*1000f,Color.red);
    }    
}
