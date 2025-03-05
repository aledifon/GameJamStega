using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleAndOptionsScreen : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    private Color firstColor;
    [SerializeField] private Color lastColor;
    [SerializeField] private float fadeDuration;
    //private TextMeshProUGUI buildNumberText;

    // Boolean Flags
    private bool isFading = false;     

    private float elapsedTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (textMeshProUGUI != null)
            firstColor = textMeshProUGUI.color;
        else
            Debug.LogWarning("The TextMeshPro object is null");

        if (gameObject.name == "TitlePanel")
        {
            TextMeshProUGUI buildNumberText = gameObject.transform.Find("BuildVersionText").
                                                gameObject.
                                                GetComponent<TextMeshProUGUI>();

            if (buildNumberText != null)
            {
                buildNumberText.text = "Build " + Application.version;
            }
            else
                Debug.LogWarning("The BuildVersionText object is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFading)
        {            
            if(textMeshProUGUI.color == firstColor)
            {
                //Debug.Log("Starting going from red to yellow color");
                StartCoroutine(FadeInOut(firstColor, lastColor));
            }                
            else if (textMeshProUGUI.color == lastColor)
            {
                //Debug.Log("Starting going from yellow to red color");
                StartCoroutine(FadeInOut(lastColor, firstColor));
            }
                
        }
    }

    // Screen Click Handler
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject clickedObject = eventData.pointerEnter;

        if (clickedObject != null)
        {            
            if (clickedObject.name == "TitlePanel")
                GameManager.Gm.OnTitleScreenClick();
            else if (clickedObject.name == "OptionsPanel")
                GameManager.Gm.OnExitOptionsScreenClick();
        }
    }

    // Text Fading Handler
    private IEnumerator FadeInOut(Color startColor, Color targetColor)
    {
        // Set to true the Fading Flag to avoid enter here again
        isFading = true;
        // Reset the timer
        elapsedTime = 0f;
        
        while (elapsedTime<fadeDuration)
        {            
            textMeshProUGUI.color = Color.Lerp(startColor, targetColor, elapsedTime/fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Assure the target Color
        textMeshProUGUI.color = targetColor;
        // Reset the Fading Flag
        isFading = false;
    }
}
