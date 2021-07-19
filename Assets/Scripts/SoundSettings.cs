using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSettings : MonoBehaviour
{
    private void Awake()
    {
        int bgm = PlayerPrefs.GetInt("BGM");
        bool m = (bgm == 1) ? false : true;
        BGM(m);

        //Default Playerprefs kangge damage sareng ammocapacity
        if(PlayerPrefs.GetFloat("damage") == 0f)
        {
            PlayerPrefs.SetFloat("damage", 20f);
        }
        if (PlayerPrefs.GetFloat("ammocapacity") == 0f)
        {
            PlayerPrefs.SetFloat("ammocapacity", 20f);
        }
    }

    public void BGM(bool _bgm)
    {
        AudioSource audioSource = GetComponentInChildren<AudioSource>();
        if (_bgm)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }
}
