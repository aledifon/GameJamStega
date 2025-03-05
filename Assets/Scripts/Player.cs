using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private AudioSource audioSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (GameManager.Gm.IsPaused || 
            GameManager.Gm.IsWinPanelEnabled ||
            GameManager.Gm.IsLoosePanelEnabled ||
            GameManager.Gm.IsLevelPassedEnabled)
        {
            StopRollFxClip();
        }
        else
        {
            if (rb.velocity.magnitude > 1f)
                PlayRollFxClip();
            else
                StopRollFxClip();
        }            
    }

    private void PlayRollFxClip()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
    }
    private void StopRollFxClip()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }
}
