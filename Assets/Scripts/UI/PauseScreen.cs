using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI exitButtonText;

    private void OnEnable()
    {
        if (exitButtonText == null)
            Debug.LogError("Exit Button Text is null");
        else
        {
            if (GameManager.isWebGL)
                exitButtonText.text = "Restart game";
            else
                exitButtonText.text = "Exit game";
        }
    }
}
