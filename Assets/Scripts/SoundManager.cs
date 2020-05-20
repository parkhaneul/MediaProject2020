using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<AudioClip> audios;

    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            return instance;
        }
    }

    public void PlayPickAxeSound(GameObject obj)
    {
        AudioSource audio = obj.GetComponent<AudioSource>();
        if(audio == null)
        {
            audio = obj.AddComponent<AudioSource>();
        }
        if(audio.isPlaying && audio.clip == audios[1])
        {
            return;
        }
        audio.clip = audios[1];
        audio.loop = false;
        audio.Play();
    }

    public void StopPickAxeSound(GameObject obj)
    {
        AudioSource audio = obj.GetComponent<AudioSource>();
        if(audio == null)
        {
            return;
        }

        if (audio.clip == audios[1])
        {
            audio.Stop();
        }
    }

    public void PlayPickItemSound(GameObject obj)
    {
        AudioSource audio = obj.GetComponent<AudioSource>();
        if(audio == null)
        {
            audio = obj.AddComponent<AudioSource>();
        }
        if(audio.isPlaying && audio.clip == audios[2])
        {
            return;
        }
        audio.clip = audios[2];
        audio.loop = false;
        audio.Play();
    }

    public void StopPickItemSound(GameObject obj)
    {
        AudioSource audio = obj.GetComponent<AudioSource>();
        if(audio == null)
        {
            return;
        }

        if (audio.clip == audios[2])
        {
            audio.Stop();
        }
    }

    public void PlayDraggingSound(GameObject obj)
    {
        AudioSource audio = obj.GetComponent<AudioSource>();
        if(audio == null)
        {
            audio = obj.AddComponent<AudioSource>();
        }
        if(audio.isPlaying && audio.clip == audios[3])
        {
            return;
        }
        audio.clip = audios[3];
        audio.loop = true;
        audio.Play();
    }

    public void StopDraggingSound(GameObject obj)
    {
        AudioSource audio = obj.GetComponent<AudioSource>();
        if(audio == null)
        {
            return;
        }

        if (audio.clip == audios[3])
        {
            audio.Stop();
        }
    }
    public void PlayWalkSound(GameObject obj)
    {
        AudioSource audio = obj.GetComponent<AudioSource>();
        if(audio == null)
        {
            audio = obj.AddComponent<AudioSource>();
        }
        if(audio.isPlaying && audio.clip == audios[0])
        {
            return;
        }
        audio.clip = audios[0];
        audio.loop = true;
        audio.Play();
    }

    public void StopWalkSound(GameObject obj)
    {
        AudioSource audio = obj.GetComponent<AudioSource>();
        if(audio == null)
        {
            return;
        }

        if (audio.clip == audios[0])
        {
            audio.Stop();
        }
    }

    void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }    
        else 
        {
            DestroyImmediate(this);
        }
    }


}
