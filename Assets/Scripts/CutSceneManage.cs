using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutSceneManage : MonoBehaviour
{
    public GameObject[] buttonSkip;
    public GameObject gotimeLine1;
    public GameObject gotimeLine2;
    public PlayableDirector timeLine1;
    public PlayableDirector timeLine2;

    private int timelineIndex;

    // Start is called before the first frame update
    void Start()
    {
        timelineIndex = PlayerPrefs.GetInt("indextimeline");
        timeLine1 = timeLine1.GetComponentInChildren<PlayableDirector>();
        timeLine2 = timeLine2.GetComponentInChildren<PlayableDirector>();

        if (timelineIndex == 0){
            gotimeLine1.SetActive(true);
            timeLine1.Play();
        } else if (timelineIndex == 1){
            gotimeLine2.SetActive(true);
            timeLine2.Play();
            buttonSkip[0].SetActive(false);
            buttonSkip[1].SetActive(true);
            PlayerPrefs.SetInt("indextimeline", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timeLine1.time >= timeLine1.duration-1)
        {
            TimeLine1();
        } else if (timeLine2.time >= timeLine2.duration-1)
        {
            TimeLine2();
        } 
    }

    public void TimeLine1()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void TimeLine2()
    {
        SceneManager.LoadScene("GamePlay");
    }
}
