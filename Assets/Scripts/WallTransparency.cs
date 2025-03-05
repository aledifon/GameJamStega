using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTransparency : MonoBehaviour
{
    private Renderer wallRenderer;
    [SerializeField] private float transparencyLevel;

    void Start()
    {
        wallRenderer = GetComponent<Renderer>();
    }
    
    public void SetWallTransparent(bool isTransparent)
    {
        if (wallRenderer != null)
        {
            Color wallColor = wallRenderer.material.color;

            // Set the wall as Transparent or opaque (in func. of isTransparent)
            wallColor.a = isTransparent ? transparencyLevel : 1f;
            // Apply the new Color to the material
            wallRenderer.material.color = wallColor;    
        }        
    }
}
