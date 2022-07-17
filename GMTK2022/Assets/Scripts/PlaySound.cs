using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioClip[] clips;
    private AudioSource source;
    public int hurtIndex;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        //if (walkSound != null)
        //{
        //    source.clip = walkSound;
        //    source.Play();
        //}
    }

    public void PlayClip(int index)
    {
        source.PlayOneShot(clips[index]);
    }

    public void PlayHurtSound()
    {
        PlayClip(hurtIndex);
    }
} 
