using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    //[SerializeField] private Rigidbody levelRb;
    private AudioSource audioSource;
    [SerializeField] private AudioClip rollingAudioClip;
    [SerializeField] private float rollingFxStoppingDelay;
    [SerializeField] private float rollingFxVelocityThreshold;
    private float rollingFxStoppingTime;
    [SerializeField] private AudioClip brakingAudioClip;

    //private Vector3 lastPlayerDirection;
    //private float lastPlayerMagnitude;
    float lastAngVelocityZ = 0f;
    //[SerializeField] private float playerBrakeThreshold;
    //[SerializeField] private bool isbraking;
    bool directionChangedZ;
    bool significantChangeZ;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        //lastPlayerDirection = rb.velocity.normalized;
    }

    void Update()
    {
        //if (!isbraking)
        //    BrakingDetection();

        if (GameManager.Gm.IsPaused || GameManager.Gm.IsWinPanelEnabled ||
            GameManager.Gm.IsLoosePanelEnabled || GameManager.Gm.IsLevelPassedEnabled)
        {
            StopAudioSourceClip();
        }        
        //else if (isbraking)
        //{
        //    //StopAudioSourceClip();
        //    PlayBrakeFxClip();
        //    StartCoroutine(nameof(ReleaseBraking));
        //}
        else
        {
            if (rb.velocity.magnitude > rollingFxVelocityThreshold)
            {
                rollingFxStoppingTime = 0f;
                PlayRollFxClip();                
            }
            else
            {
                StopRollFxTimer();
            }                
        }
        //Debug.Log(rb.velocity.magnitude);
    }

    //private void BrakingDetection()
    //{
    //    // Get the current Player Dir.
    //    Vector3 currentPlayerDirection = rb.velocity.normalized;
    //    float currentPlayerMagnitude = rb.velocity.magnitude;

    //    // Calculate the Escalar Product Vectors
    //    float dot = Vector3.Dot(currentPlayerDirection, lastPlayerDirection);

    //    // Detect a braking when the Sphere direction changes & has reached a certain velocity
    //    //if (dot < 0 && lastPlayerMagnitude > playerBrakeThreshold)
    //    //{
    //    //    isbraking = true;
    //    //}
    //    if (dot < 0)
    //    {
    //        //Debug.Log("Detectado cambio direccion");
    //    }

    //    //if (lastPlayerMagnitude - currentPlayerMagnitude > playerBrakeThreshold)
    //    //{
    //    //    isbraking = true;
    //    //}

    //    // Update the last Player Direction & the Magnitude
    //    lastPlayerDirection = rb.velocity.normalized;
    //    lastPlayerMagnitude = rb.velocity.magnitude;
    //}
    //private void BrakingDetection()
    //{
    //    //Debug.Log(" Horiz = " + xMouse + ", Z-AngularVel = " + rb.angularVelocity.z);
    //    //Debug.Log(" Vert = " + yMouse + "  X-AngularVel = " + rb.angularVelocity.x);

    //    float currentAngVelocityZ = rb.angularVelocity.z;

    //    //Debug.Log("Z-AngularVel = " + currentAngVelocityZ + 
    //    //            ", DiffAngVel = " + (Mathf.Abs(lastAngVelocityZ) - Mathf.Abs(currentAngVelocityZ)));        

    //    //directionChangedZ = Mathf.Sign(lastAngVelocityZ) != Mathf.Sign(currentAngVelocityZ);
    //    significantChangeZ = Mathf.Abs(lastAngVelocityZ) - Mathf.Abs(currentAngVelocityZ) > playerBrakeThreshold;

    //    if (significantChangeZ)
    //    {
    //        Debug.Log("Sense changed on Wz" + " LastAngVelZ = " + lastAngVelocityZ + ", CurrentAngVelZ = " + currentAngVelocityZ);
    //        isbraking = true;
    //        StopAudioSourceClip();
    //    }

    //    // Update the last Ang Vel
    //    lastAngVelocityZ = currentAngVelocityZ;
    //}
    //IEnumerator ReleaseBraking() 
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    StopAudioSourceClip();
    //    isbraking = false;
    //}
    private void StopRollFxTimer()
    {
        rollingFxStoppingTime += Time.deltaTime;
        if (rollingFxStoppingTime >= rollingFxStoppingDelay)
        {
            StopAudioSourceClip();
            rollingFxStoppingTime = 0f;
        }
    }

    #region AudioManager
    private void PlayAudioClip(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        if (!audioSource.isPlaying)
            audioSource.Play();
    }    
    private void PlayRollFxClip()
    {
        audioSource.volume = 0.4f;
        audioSource.loop = true;
        PlayAudioClip(rollingAudioClip);
    }
    private void PlayBrakeFxClip()
    {
        audioSource.volume = 0.3f;
        audioSource.loop = false;
        PlayAudioClip(brakingAudioClip);
    }
    private void StopAudioSourceClip()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }
    #endregion
}
