using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService; 
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    [Header ("Indicator")]
    [SerializeField] Transform indicatorTransform;
    //[SerializeField] Vector3 initIndicatorPos;
    //[SerializeField] float travelDuration;

    //float elapsedTime;
    //Vector3 targetIndicatorPos;

    //float newIndicatorScale = 0f;
    float newIndicatorYPos = 0f;

    [SerializeField] Rigidbody ballRb;
    [SerializeField] Rigidbody levelRb;

    void Start()
    {
        
    }
    
    void Update()
    {
        Travel();   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {           
            // Stop the table (Level) and the ball Movement
            FreezeBallAndLevelMovement();

            Debug.Log("Congratulations! You reach the End of the Level!");

            if (SceneManager.GetActiveScene().name == GameManager.Scenes.Level1.ToString())
            {
                // Load Level Passed Panel
                GameManager.Gm.SetLevelPassedPanel(true);
                // Load LevelPassed Music?
                GameManager.Gm.PlayLevelPassedAudioClip();
                // Load Level 2 after elapsed a certain time
                StartCoroutine(LoadLevel2AfterDelay(2f));
            }                
            //else if (SceneManager.GetActiveScene().name == GameManager.Scenes.Level2.ToString())
            //{
            //    // Load Level Passed Panel
            //    GameManager.Gm.SetLevelPassedPanel(true);
            //    // Load LevelPassed Music?
            //    GameManager.Gm.PlayLevelPassedAudioClip();
            //    // Load Level 3 after elapsed a certain time
            //    StartCoroutine(LoadLevel3AfterDelay(2f));
            //}
            //else if (SceneManager.GetActiveScene().name == GameManager.Scenes.Level3.ToString())
            else if (SceneManager.GetActiveScene().name == GameManager.Scenes.Level2.ToString())
            {
                // Load Win Panel
                GameManager.Gm.SetWinPanel(true);
                // Load Win Music?
                GameManager.Gm.PlayWinAudioClip();
            }
        }
    }

    private void FreezeBallAndLevelMovement()
    {
        ballRb.constraints = RigidbodyConstraints.FreezeAll;
        levelRb.constraints = RigidbodyConstraints.FreezeAll;
    }
    private IEnumerator LoadLevel2AfterDelay(float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);

        // Reset the Player Starting Pos.
        GameManager.Gm.SetCheckPoint(false);
        //SceneManager.LoadScene(GameManager.Scenes.Level2.ToString());
        GameManager.Gm.LoadLevel2();
    }
    private IEnumerator LoadLevel3AfterDelay(float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);

        // Reset the Player Starting Pos.
        GameManager.Gm.SetCheckPoint(false);
        //SceneManager.LoadScene(GameManager.Scenes.Level3.ToString());
        GameManager.Gm.LoadLevel3();
    }
    private void Travel()
    {
        //elapsedTime += Time.deltaTime;

        //if (elapsedTime >= travelDuration)
        //{
        //    elapsedTime = 0;            

        //}

        newIndicatorYPos = Mathf.PingPong(Time.time * 2f, 2);
        //newIndicatorScale = Mathf.PingPong(Time.time, 2f);

        indicatorTransform.position = new Vector3(transform.position.x,
                                                transform.position.y + newIndicatorYPos+0.6f,
                                                transform.position.z);

        //indicatorTransform.localScale = new Vector3(indicatorTransform.localScale.x, 
        //                                            newIndicatorScale+1f, 
        //                                            newIndicatorScale+1f);

        indicatorTransform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);
    }
}
