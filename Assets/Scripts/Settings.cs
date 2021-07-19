using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Toggle toggleSE;
    public Toggle toggleBGM;
    public Camera mainCamera;
    private SoundSettings bgmSet;
    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();

        //Sound Effect Toggle
        int se = PlayerPrefs.GetInt("SoundEffect");
        bool e = (se == 1) ? false : true;
        toggleSE.isOn = e;

        //BGM Toggle
        int bgm = PlayerPrefs.GetInt("BGM");
        bool m = (bgm == 1) ? false : true;
        toggleBGM.isOn = m;
    }

    //Sound Effect
    public void SoundEffect(bool se)
    {
        ToggleEffect();
        int e = (se) ? 0 : 1;
        PlayerPrefs.SetInt("SoundEffect", e);
    }

    //BGM
    public void BGM(bool bgm)
    {
        ToggleEffect();
        int m = (bgm) ? 0 : 1;
        PlayerPrefs.SetInt("BGM", m);

        bgmSet = mainCamera.GetComponent<Camera>().GetComponentInChildren<SoundSettings>();
        bgmSet.BGM(bgm);
    }

    public void ToggleEffect()
    {
        audioSource.PlayOneShot(audioSource.clip);
    }
}
