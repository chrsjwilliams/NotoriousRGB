using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Clips { CLICK, WIN, LOSE, RED, GREEN, BLUE }

public class AudioManager : MonoBehaviour
{
   
    private Dictionary<Clips, AudioClip> audioLibrary;
    private AudioSource audioSource;
    private AudioClip audioClip;


	// Use this for initialization
	void Start ()
    {
        audioLibrary = new Dictionary<Clips, AudioClip>();
        audioSource = GetComponent<AudioSource>();
        LoadLibrary();
	}

    private void LoadLibrary()
    {
        audioLibrary.Add(Clips.CLICK, Resources.Load<AudioClip>("Audio/Click"));
        audioLibrary.Add(Clips.WIN, Resources.Load<AudioClip>("Audio/Win"));
        audioLibrary.Add(Clips.LOSE, Resources.Load<AudioClip>("Audio/Lose"));
        audioLibrary.Add(Clips.RED, Resources.Load<AudioClip>("Audio/Red"));
        audioLibrary.Add(Clips.GREEN, Resources.Load<AudioClip>("Audio/Green"));
        audioLibrary.Add(Clips.BLUE, Resources.Load<AudioClip>("Audio/Blue"));
    }

    public void PlayClip(Clips clip)
    {
        PlayClip(clip, 1.0f);
    }

    public void PlayClip(Clips clip, float volume)
    {
        audioClip = audioLibrary[clip];
        audioSource.PlayOneShot(audioClip, volume);
    }
}
