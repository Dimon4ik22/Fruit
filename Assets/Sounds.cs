using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Sounds : Singleton<Sounds>
{
    
    public AudioClip pickUpSound; // Reference to the audio clip
    public AudioClip unlockSound; // Reference to the audio clip
    public AudioClip spendSound; // Reference to the audio clip

    private AudioSource audioSource;

    private void Awake()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
    }

    [Button]
    public void PlayPickup()
    {
        // Play the sound clip
        audioSource.PlayOneShot(pickUpSound);
    }

    [Button]
    public void PlayUnlockSound()
    {
        if(!audioSource.isPlaying)
            audioSource.PlayOneShot(unlockSound);
        
    }
    public void PlaySpendMoney()
    {
        // if (!audioSource.isPlaying)
        // {
        //     audioSource.loop = true;
        //     audioSource.PlayOneShot(spendSound);
        //     
        // }
        //
    }

    public void StopSound()
    {
        audioSource.loop = false;
        audioSource.Stop();
    }
}
