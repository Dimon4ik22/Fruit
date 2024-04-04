using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public Sound sound;
    public AudioSource audioSource;
    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }
}
