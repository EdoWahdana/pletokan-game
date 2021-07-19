using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManage : MonoBehaviour
{
    public GameObject character;

    public GameObject mp, mp1, mp2;
    private AudioSource audioSource;

    private int move;

    public void LevelMenu()
    {
        move = PlayerPrefs.GetInt("maplevel");
        if (move == 1)
        {
            character.SetActive(false);
            mp.SetActive(true);
            mp1.SetActive(false);
            mp2.SetActive(true);
            PlayerPrefs.SetInt("maplevel", 0);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        character.SetActive(true);
        LevelMenu();
    }

    public void MMenu(GameObject menu)
    {
        ButtonEffect();
        character.SetActive(false);
        if (menu.name == "MenuPause")
        {
            Time.timeScale = 0;
        }

        if(menu.name != "MenuPlay")
        {
            menu.SetActive(true);
        }
        else if(menu.name == "MenuPlay")
        {
            if (PlayerPrefs.GetInt("level") == 0)
            {
                SceneManagement("CutScene");
            }
            else
            {
                menu.SetActive(true);
            }
        }
    }

    public void CMenu(GameObject menu)
    {
        ButtonEffect();
        if (menu.name != "ContainerWarning"){
            character.SetActive(true);
        }
        if (menu.name == "MenuPause")
        {
            Time.timeScale = 1;
        }
        menu.SetActive(false);
    }

    public void SceneManagement(string scene)
    {
        ButtonEffect();
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        SceneManager.LoadScene(scene);
    }

    public void ExitGame()
    {
        ButtonEffect();
        Application.Quit();
    }


    public void ButtonEffect()
    {
        int buttonEffect = PlayerPrefs.GetInt("SoundEffect");
        if(buttonEffect == 0)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    public void NewGame(int _level)
    {
        PlayerPrefs.SetInt("checklevel", _level);
        PlayerPrefs.SetInt("level", _level);
        SceneManagement("CutScene");
    }
    
}
