using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Vars
    // Static var which contains the single instance of the Singleton
    private static GameManager gm;
    // Static property used to access to the Singleton
    public static GameManager Gm
    {
        // Trying to access to the GameManager singleton through its property Gm.
        get
        {
            // In case doesn't exist already an instance of the GameManager script
            if (gm == null)
            {
                // In case there is already a GO of type GameManager then assign it
                // to our Singleton instance
                gm = FindAnyObjectByType<GameManager>();

                // In case there is no any GO of type GameManager then we'll create it,
                // then we add it a GameManager Component and then we assign it to our
                // Singleton instance gm.
                if (gm == null)
                {
                    GameObject go = new GameObject("GameManager");
                    gm = go.AddComponent<GameManager>();
                }
            }
            return gm;
        }
    }

    public static readonly bool isWebGL = Application.platform == RuntimePlatform.WebGLPlayer;
    //public static readonly bool isWebGL = false;     // true = WebGL, false = Windows

    [Header("UI Controls")]
    private GameObject Canvas;
    private GameObject titlePanel;
    private GameObject menuPanel;
    private GameObject optionsPanel;
    private GameObject pausePanel;
    private GameObject winPanel;
    private GameObject loosePanel;
    private GameObject LevelPassedPanel;

    private MenuButton menuPanelStartButton;
    private MenuButton menuPanelOptionsButton;

    [Header("Fx Audio")]
    //[SerializeField] private AudioClip failAudioclip;    
    //[SerializeField] private AudioClip winAudioclip;
    //[SerializeField] private AudioClip restartAudioclip;    
    //[SerializeField] private AudioClip pauseAudioclip;
    //[SerializeField] private AudioClip selectFXAudioclip;

    [Header("Game Audio")]
    [SerializeField] private AudioClip gameAudioclip;
    [SerializeField] private AudioClip mainTitleAudioclip;
    [SerializeField] private AudioClip levelPassedAudioclip;    
    [SerializeField] private AudioClip winAudioclip;
    [SerializeField] private AudioClip looseAudioclip;

    private AudioSource audioSource;

    // Pause Flag
    public bool IsPaused { get; private set; } = false;
    public bool IsLevelPassedEnabled { get; private set; } = false;
    public bool IsWinPanelEnabled { get; private set; } = false;
    public bool IsLoosePanelEnabled { get; private set; } = false;

    public bool IsReachedCheckPoint { get; private set; } = false;

    #region Enums
    public enum Scenes {Menu, Level1, Level2, Level3}
    private Scenes sceneSelected = Scenes.Menu;
    #endregion

    [SerializeField] public float playerRbLevel1DefAngularDrag;   // 0.8f
    [SerializeField] public float playerRbLevel2DefAngularDrag;   // <0.8f
    [SerializeField] public float playerRbLevel3DefAngularDrag;   // <0.8f
    [SerializeField] public float playerRbPlatformAngularDrag;    // 100f

    [SerializeField] public bool debugLevel2;

    #endregion

    #region Unity_API
    void Awake()
    {
        // If doesn't exist already an instance of the GameManager script
        // then saves the current instance on the singleton var.
        if (gm == null)
        {
            gm = this;
            DontDestroyOnLoad(gameObject);              // Avoids the GO will be destroyed when
                                                        // the Scene changes

            SceneManager.sceneLoaded += OnSceneLoaded;  // Subscribe to the event.
        }
        // Otherwise, if does already exists an instance of the GameManager script then
        // we'll destroy the GO is attached to
        else
            Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();        
    }

    private void Start()
    {
        // ALL THESE LINES NEED TO BE MOVED TO OnSceneLoaded 
        // Otherwise all the refs. to GOs dependent of each Scene
        // won't be loaded as Start Method only runs once.

    }

    void Update()
    {
        // Only Enable when the current Scene is not the Menu Scene
        if (/*sceneSelected != Scenes.Menu && */
            (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)))
        {
            TooglePause();
        }
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion

    #region Private Methods 
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Canvas = GameObject.Find("Canvas");
        if (Canvas == null)
            Debug.LogError("The Canvas object is null");

        // Parsing the Scene Name to enum typedata
        if (System.Enum.TryParse(SceneManager.GetActiveScene().name, out Scenes currentScene))
        {
            switch (currentScene)
            {
                case Scenes.Menu:

                    // Set the new Scene as the current one
                    sceneSelected = Scenes.Menu;

                    // Get all the Panel GOs                
                    titlePanel = Canvas.transform.Find("TitlePanel")?.gameObject;
                    if (titlePanel == null)
                        Debug.LogError("The Title Panel object is null");

                    optionsPanel = Canvas.transform.Find("OptionsPanel")?.gameObject;
                    if (optionsPanel == null)
                        Debug.LogError("The options Panel object is null");

                    menuPanel = Canvas.transform.Find("MenuPanel")?.gameObject;
                    if (menuPanel == null)
                        Debug.LogError("The menu Panel object is null");

                    menuPanelStartButton = menuPanel.transform.Find("StartButton")?.GetComponent<MenuButton>();
                    if (menuPanelStartButton == null)
                        Debug.LogError("The Menu Panel Start Button object is null");

                    menuPanelOptionsButton = menuPanel.transform.Find("OptionsButton")?.GetComponent<MenuButton>();
                    if (menuPanelOptionsButton == null)
                        Debug.LogError("The Menu Panel Options Button object is null");

                    pausePanel = Canvas.transform.Find("PausePanel")?.gameObject;
                    if (pausePanel == null)
                        Debug.LogError("The Pause Panel object is null");

                    // Start playing Title Screen Audio
                    PlayMainTitleAudioClip();

                    // Enable the TitlePanel Screen
                    titlePanel.SetActive(true);
                    // Update the Panel Selected State
                    //panelSelected = PanelSelected.Title;

                    // Unlock the Mouse Cursor
                    ShowMouseCursor(true);

                    break;
                case Scenes.Level1: 
                case Scenes.Level2:
                case Scenes.Level3:

                    // Set the new Scene as the current one
                    if (SceneManager.GetActiveScene().name == Scenes.Level1.ToString())
                        sceneSelected = Scenes.Level1;
                    if (SceneManager.GetActiveScene().name == Scenes.Level2.ToString())
                        sceneSelected = Scenes.Level2;
                    if (SceneManager.GetActiveScene().name == Scenes.Level3.ToString())
                        sceneSelected = Scenes.Level3;

                    // Get the Pause, Win & Loose Panels GOs                
                    pausePanel = Canvas.transform.Find("PausePanel")?.gameObject;
                    if (pausePanel == null)
                        Debug.LogError("The Pause Panel object is null");
                    winPanel = Canvas.transform.Find("WinPanel")?.gameObject;
                    if (winPanel == null)
                        Debug.LogError("The Win Panel object is null");
                    loosePanel = Canvas.transform.Find("LoosePanel")?.gameObject;
                    if (loosePanel == null)
                        Debug.LogError("The Loose Panel object is null");
                    LevelPassedPanel = Canvas.transform.Find("LevelPassedPanel")?.gameObject;
                    if (LevelPassedPanel == null)
                        Debug.LogError("The Level Passed Panel object is null");

                    // Start playing Game Audio
                    PlayGameAudioClip();

                    // Disable all the Panels screens
                    pausePanel.SetActive(false);
                    SetWinPanel(false);
                    SetLoosePanel(false);
                    SetLevelPassedPanel(false);

                    //// Set the corresponding Player Pos. in func. of CheckPoint Reached or not
                    //if (IsReachedCheckPoint)                                       
                    //    GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().SetCheckPointPos();
                    //else
                    //    GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().SetStartPos();

                    // Lock the Mouse Cursor
                    ShowMouseCursor(false);
                    break;                    
            }
        }        
    }
    private void TooglePause()
    {
        IsPaused = !IsPaused;

        if (IsPaused)
        {
            Time.timeScale = 0f;                // Stops the game (stop the physics and pending updates which are time dependent)
            audioSource.Pause();
            pausePanel.SetActive(true);
            if (sceneSelected != Scenes.Menu)
                ShowMouseCursor(true);
            // Update the Panel Selected State
            //panelSelected = PanelSelected.Pause;
        }
        else
        {
            Time.timeScale = 1f;                // Resumes the game
            audioSource.Play();
            pausePanel.SetActive(false);
            if (sceneSelected != Scenes.Menu)
                ShowMouseCursor(false);
            //panelSelected = PanelSelected.Game;     // As the Pause can be launch from any Panel this could be wrong (NEEDED TO UPDATE!)
        }
    }
    #region AudioManager
    private void PlayAudioClip(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        if (!audioSource.isPlaying)
            audioSource.Play();
    }    
    #endregion
    #endregion

    #region Public Methods
    public void SetCheckPoint(bool enable)
    {
        IsReachedCheckPoint = enable;
    }
    // Restart the Game from the 1st Level again
    public void RestartFromLevel1()
    {
        // Reset the Player Starting Pos.
        SetCheckPoint(false);        
        OnStartGameClick();
    }
    // Restart the Game from the 
    public void RestartLevel()
    {        
        // Load again the corresponding Schen where we currently are
        if (SceneManager.GetActiveScene().name == Scenes.Level1.ToString())        
            LoadLevel1();
        else if (SceneManager.GetActiveScene().name == Scenes.Level2.ToString())
            LoadLevel2();
        else if (SceneManager.GetActiveScene().name == Scenes.Level3.ToString())
            LoadLevel3();
    }
    public void ReturnToMenu()
    {
        // Reset the Player Starting Pos.
        SetCheckPoint(false);
        // Return to the Menu Scene
        SceneManager.LoadScene(Scenes.Menu.ToString());
    }
    public void LoadLevel1()
    {
        // Return to the Level2 Scene
        SceneManager.LoadScene(Scenes.Level1.ToString());
    }
    public void LoadLevel2()
    {
        // Return to the Level2 Scene
        SceneManager.LoadScene(Scenes.Level2.ToString());
    }
    public void LoadLevel3()
    {
        // Return to the Level2 Scene
        SceneManager.LoadScene(Scenes.Level3.ToString());
    }
    public void SetLevelPassedPanel(bool enable)
    {
        IsLevelPassedEnabled = enable;
        LevelPassedPanel.SetActive(enable);
        ShowMouseCursor(enable);
    }
    public void SetWinPanel(bool enable)
    {
        IsWinPanelEnabled = enable;
        winPanel.SetActive(enable);
        ShowMouseCursor(enable);
    }
    public void SetLoosePanel(bool enable)
    {
        IsLoosePanelEnabled = enable;
        loosePanel.SetActive(enable);
        ShowMouseCursor(enable);
    }
    public void OnQuitGame()
    {
        if (isWebGL)
            SceneManager.LoadScene(Scenes.Menu.ToString());
        else
            QuitGame();
    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();            
        #endif
    }
    public void OnTitleScreenClick()
    {
        // Disable  the TitlePanel Screen & Enable the Menu Panel
        titlePanel.SetActive(false);
        menuPanel.SetActive(true);

        // Update the Panel Selected State
        //panelSelected = PanelSelected.Menu;
    }    
    public void OnToOptionsButtonClick()
    {
        // Disable  the Menu Panel & Enable the Options Panel
        menuPanel.SetActive(false);
        optionsPanel.SetActive(true);

        // Fade the Menu Panel buttons
        menuPanelStartButton.FadeButton();
        menuPanelOptionsButton.FadeButton();

        // Update the Panel Selected State
        //panelSelected = PanelSelected.Options;
    }
    public void OnExitOptionsScreenClick()
    {
        // Disable  the Options Panel & Enable the Menu panel
        optionsPanel.SetActive(false);
        menuPanel.SetActive(true);

        // Update the Panel Selected State
        //panelSelected = PanelSelected.Menu;
    }
    public void OnStartGameClick()
    {
        //if (debugLevel2)
        //    SceneManager.LoadScene(Scenes.Level2.ToString());         // ONLY FOR TEST!!! REMEMBER TO DELETE WHEN FINISH LEVEL2!!
        //else
        //{
            // Load the Level 1 Scene
            SceneManager.LoadScene(Scenes.Level1.ToString());         // ONLY FOR TEST!!! REMEMBER TO LEAVE ONLY THIS OPTION
                                                                      // WHEN FINISH LEVEL2!!
        //}
    }    

    #region AudioMenuMethods
    public void StopAudioSourceClip()
    {
        audioSource.Stop();
    }
    public void PlayGameAudioClip()
    {
        audioSource.volume = 0.4f;
        audioSource.loop = true;
        PlayAudioClip(gameAudioclip);
    }    
    public void PlayMainTitleAudioClip()
    {
        audioSource.volume = 0.4f;
        audioSource.loop = true;
        PlayAudioClip(mainTitleAudioclip);
    }
    public void PlayLevelPassedAudioClip()
    {
        audioSource.volume = 0.4f;
        audioSource.loop = false;
        PlayAudioClip(levelPassedAudioclip);
    }    
    public void PlayWinAudioClip()
    {
        audioSource.volume = 0.4f;
        audioSource.loop = false;
        PlayAudioClip(winAudioclip);
    }
    public void PlayLooseAudioClip()
    {
        audioSource.volume = 0.4f;
        audioSource.loop = false;
        PlayAudioClip(looseAudioclip);
    }
    #endregion

    public void ShowMouseCursor(bool enable)
    {
        if (enable)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Force the updating of the mouse cursor on the next frame
            //StartCoroutine(nameof(FixCursorVisibility));
        }
    }
    private IEnumerator FixCursorVisibility()
    {
        yield return new WaitForSeconds(3f);
        Cursor.visible = false;
    }
    #endregion
}
