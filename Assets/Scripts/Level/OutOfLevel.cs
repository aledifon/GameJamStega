using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfLevel : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Out OF LEVEL
            
            // Destroy the Player GO
            Destroy(other.gameObject);

            // 2. Show Fail Panel for 5s
            // 3. Load again the Menu Scene (Title) or Try Again Panel?.

            Debug.Log("You failed! Maybe try again?");
        }
    }
}
