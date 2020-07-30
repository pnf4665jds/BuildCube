using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : Singleton<GameAudioManager>
{
    public void PlaySound(AudioClip clip)
    {
        GameObject obj = new GameObject("AuidoPlayer");
        AudioSource source = obj.AddComponent<AudioSource>();
        source.PlayOneShot(clip);
        Destroy(source, clip.length);
    }
}
