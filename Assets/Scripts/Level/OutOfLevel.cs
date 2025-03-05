using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfLevel : MonoBehaviour
{
    [SerializeField] Rigidbody levelRb;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {                        
            // Stop the table (Level) Movement            
            FreezeLevelMovement();
            // Destroy the Ball GO
            Destroy(other.gameObject);
            // Load Loose Panel
            GameManager.Gm.SetLoosePanel(true);
            // Load Win Music?
            GameManager.Gm.PlayLooseAudioClip();

            Debug.Log("You failed! Maybe try again?");
        }
    }
    private void FreezeLevelMovement()
    {        
        levelRb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
