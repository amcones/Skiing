using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip hitAudio;
    public AudioClip loseAudio;
    public AudioClip enterAudio;
    public AudioClip chooseAudio;

    public AudioSource audioPlayer;

    public void PlayHit()
    {
        Debug.Log("Hit");
        PlayAudio(hitAudio);
    }

    public void PlayLose()
    {
        Debug.Log("Lose");
        audioPlayer.Stop();
        PlayAudio(loseAudio);
    }

    public void PlayEnter()
    {
        PlayAudio(enterAudio);
    }

    public void PlayChoose()
    {
        PlayAudio(chooseAudio);
    }

    private void PlayAudio(AudioClip clip)
    {
        audioPlayer.clip = clip;
        audioPlayer.Play();
    }
}
