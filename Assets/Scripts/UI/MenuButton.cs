using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private AudioSource audioSource;  // Arrastra el AudioSource desde el Inspector
    [SerializeField] private AudioClip hoverSound;     // Arrastra el sonido desde el Inspector

    private Image buttonImage;                          // Button Image Component
    private Color imageColor;                           // Button Image Color
    private Button button;                              // Button Objecvt

    void Start()
    {
        buttonImage = GetComponent<Image>();        // Gets the button image component
        imageColor = buttonImage.color;             // Gets the original button color
        button = GetComponent<Button>();            // Gets the Button

        if (button != null)
        {
            // Check which Button is this one
            if (gameObject.name == "StartButton")
            {
                // Limpiar eventos previos para evitar duplicados
                button.onClick.RemoveAllListeners();
                // Asignar el método de GameManager al botón
                button.onClick.AddListener(GameManager.Gm.OnStartGameClick);
            }
            else if (gameObject.name == "OptionsButton")
            {
                // Limpiar eventos previos para evitar duplicados
                button.onClick.RemoveAllListeners();
                // Asignar el método de GameManager al botón
                button.onClick.AddListener(GameManager.Gm.OnToOptionsButtonClick);
            }
            else if (gameObject.name == "ExitButton")
            {
                // Limpiar eventos previos para evitar duplicados
                button.onClick.RemoveAllListeners();
                // Asignar el método de GameManager al botón
                button.onClick.AddListener(GameManager.Gm.OnQuitGame);
            }
            else if (gameObject.name == "ReplayButton")
            {
                // Limpiar eventos previos para evitar duplicados
                button.onClick.RemoveAllListeners();
                // Asignar el método de GameManager al botón
                button.onClick.AddListener(GameManager.Gm.ReplayGame);
            }
            else if (gameObject.name == "ReturnToMenuButton")
            {
                // Limpiar eventos previos para evitar duplicados
                button.onClick.RemoveAllListeners();
                // Asignar el método de GameManager al botón
                button.onClick.AddListener(GameManager.Gm.ReturnToMenu);
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Play the sound Fx
        if (audioSource != null && hoverSound != null)        
            audioSource.PlayOneShot(hoverSound);

        HighlightButton();       
    }
    public void OnPointerExit(PointerEventData eventData)
    {        
        FadeButton();
    }

    public void HighlightButton()
    {
        // Set the button image color as visible
        if (buttonImage != null)
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1f);
    }
    public void FadeButton()
    {
        // Set the button image color as transparent
        if (buttonImage != null)
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 0f);
    }
}