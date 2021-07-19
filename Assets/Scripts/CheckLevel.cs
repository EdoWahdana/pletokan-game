using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckLevel : MonoBehaviour
{
    public Button[] buttonLevel;
    public Button[] tabLevel;
    public GameObject[] containerLevel;
    public Text title;
    private int level;

    MenuManage menuManage;
    // Start is called before the first frame update
    void Start()
    {
        menuManage = GameObject.FindGameObjectWithTag("MenuManage").GetComponent<MenuManage>();
        level = PlayerPrefs.GetInt("level");
        //level = 8;
        for (int i=0; i<=level; i++)
        {
            buttonLevel[i].interactable = true;
        }

        if (level >= 0 && level <= 2)
        {
            tabLevel[0].interactable = false;
            tabLevel[1].interactable = true;
            tabLevel[2].interactable = true;
            containerLevel[0].SetActive(true);
            title.text = "SEEDING";
        }
        else if (level >= 3 && level <= 5)
        {
            tabLevel[0].interactable = true;
            tabLevel[1].interactable = false;
            tabLevel[2].interactable = true;
            containerLevel[1].SetActive(true);
            title.text = "PRE HARVEST";
        }
        else if (level >= 6 && level <= 8)
        {
            tabLevel[0].interactable = true;
            tabLevel[1].interactable = true;
            tabLevel[2].interactable = false;
            containerLevel[2].SetActive(true);
            title.text = "PASCA HARVEST";
        }
    }

    public void ContinueButton()
    {
        Start();
    }

    public void PlayGame(int _level)
    {
        PlayerPrefs.SetInt("checklevel", _level);
        menuManage.SceneManagement("GamePlay");
    }
}
