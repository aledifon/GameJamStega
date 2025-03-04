using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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

    float newIndicatorScale = 0f;
    float newIndicatorYPos = 0f;


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
            // END LEVEL

            // 1. Stop the Table and Ball Movement? or Timescale to 0f?
            // 2. Show Success/Win Panel for 5s
            // 3. Load Scene of Next Level

            Debug.Log("Congratulations! You reach the End of the Level!");

            if (SceneManager.GetActiveScene().name == GameManager.Scenes.Level1.ToString())
                SceneManager.LoadScene(GameManager.Scenes.Level2.ToString());   
            else if (SceneManager.GetActiveScene().name == GameManager.Scenes.Level2.ToString())
                SceneManager.LoadScene(GameManager.Scenes.Level3.ToString());
            else if (SceneManager.GetActiveScene().name == GameManager.Scenes.Level3.ToString())
            {
                // Load Win Panel + Win Music

                // If pressed "Come back to Title Screen" Button    --> Load Menu Scene

                // If pressed "Try again" Button                    --> Load Level1 Scene
            }
        }
    }

    private void Travel()
    {
        //elapsedTime += Time.deltaTime;

        //if (elapsedTime >= travelDuration)
        //{
        //    elapsedTime = 0;            

        //}

        newIndicatorYPos = Mathf.PingPong(Time.time * 2f, 2);
        newIndicatorScale = Mathf.PingPong(Time.time, 1f);

        indicatorTransform.position = new Vector3(transform.position.x,
                                                transform.position.y + newIndicatorYPos,
                                                transform.position.z);

        indicatorTransform.localScale = new Vector3(newIndicatorScale+0.2f, 
                                                    newIndicatorScale+0.2f, 
                                                    newIndicatorScale+0.2f);
    }
}
