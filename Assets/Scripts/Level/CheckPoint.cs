using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    [Header ("Indicator")]
    [SerializeField] Transform indicatorTransform;
    //[SerializeField] Vector3 initIndicatorPos;
    //[SerializeField] float travelDuration;

    //float elapsedTime;
    //Vector3 targetIndicatorPos;

    float newIndicatorScale = 0f;
    float newIndicatorYPos = 0f;

    AudioSource audioSource;
    [SerializeField] AudioClip checkPointClip;

    void Awake()
    {
        if (GameManager.Gm.IsReachedCheckPoint)
            gameObject.SetActive(false);
        else
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                Debug.LogError("Audio Source Componente is null");
        }        
    }
    
    void Update()
    {
        Travel();   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {                       
            Debug.Log("You reached the CheckPoint Level!");

            // Disables the Box Collider to avoid enter again
            BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
            if (boxCollider != null)
                boxCollider.enabled = false;

            // Play CheckPoint Audio clip
            PlayCheckPointAudioClip();

            // Set the CheckPoint as reached
            GameManager.Gm.SetCheckPoint(true);            

            // Destroy the GO once the audio clip has been reproduced
            StartCoroutine(nameof(DestroyAfterDelay));
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
    private void PlayAudioClip(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        if (!audioSource.isPlaying)
            audioSource.Play();
    }
    public void PlayCheckPointAudioClip()
    {
        audioSource.volume = 1f;
        audioSource.loop = false;
        PlayAudioClip(checkPointClip);
    }
    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitWhile(()=>audioSource.isPlaying);

        Destroy(gameObject);
    }
}
