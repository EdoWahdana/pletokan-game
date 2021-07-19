using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManage : MonoBehaviour
{
    public GameObject padiGroup1;
    public GameObject padiGroup2;
    public GameObject padiGroup3;
    public Text score;
    public int playerScore = 0;

    MenuManage menuManage;

    [HideInInspector]
    public int enemies;
    [HideInInspector]
    public int scorePerEnemy;


    public void AddScore(int enemyScore)
    {
        playerScore += Random.Range(5, scorePerEnemy);
        score.text = playerScore.ToString();
    }

    public void Win()
    {
        menuManage = GameObject.FindGameObjectWithTag("MenuManage").GetComponent<MenuManage>();
        menuManage.SceneManagement("MainMenu");
        PlayerPrefs.SetInt("maplevel", 1);
    }

}