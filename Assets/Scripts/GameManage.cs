using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManage : MonoBehaviour
{
    public GameObject padiGroup1;
    public GameObject padiGroup2;
    public GameObject padiGroup3;
    public GameObject winTitle;
    public GameObject loseTitle;
    public GameObject winButton;
    public GameObject loseButton;
    public GameObject display;
    public AudioSource winSound;
    public AudioSource loseSound;
    public Text score;
    public Text playTimeText;
    public Text playScoreText;
    public Text coinText;
    public int playerScore = 0;

    [HideInInspector]
    public int enemies;
    [HideInInspector]
    public int scorePerEnemy;

    MenuManage menuManage;

    private float m_coin;

    private float m_playTime = 0f;

    public IEnumerator WinDisplay(bool isWin)
    {

        display.SetActive(true);
        m_coin = PlayerPrefs.GetFloat("coin");
        float plusCoin = playerScore * 15 / m_playTime;
        PlayerPrefs.SetFloat("coin", m_coin + plusCoin);
        playTimeText.text = ((int)m_playTime).ToString() +" Second";
        playScoreText.text = playerScore.ToString();
        coinText.text = "$" +((int)plusCoin).ToString();

        if (isWin) {
            winTitle.SetActive(true);
            //winButton.SetActive(true);
            winSound.Play();
        }
        else { 
            loseTitle.SetActive(true);
            //loseButton.SetActive(true);
            loseSound.Play();
        } 

        yield return new WaitForSeconds(5f);

        menuManage = GameObject.FindGameObjectWithTag("MenuManage").GetComponent<MenuManage>();
        menuManage.SceneManagement("MainMenu");
        PlayerPrefs.SetInt("maplevel", 1);
    }

    public void AddScore(int enemyScore)
    {
        playerScore += enemyScore;
        score.text = playerScore.ToString();
    }

    public void Win()
    {
        // Parameter merupakan boolean isWin
        StartCoroutine(WinDisplay(true));        
    }

    public void Lose()
    {
        StartCoroutine(WinDisplay(false));
    }

    void Update()
    {
        m_playTime += Time.deltaTime;
    }
}