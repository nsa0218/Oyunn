using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    private void Awake()
    {
        instance = this;
    }




    [SerializeField]
    public Text earnedValueText;
    public double earnedExp;







    public TMP_Text overheatedMessage;
    public Slider weaponTempSlider;

    public GameObject deathScreen;
    public TMP_Text deathText;

    public Slider healthSlider;

    public TMP_Text killsText, deathsText;

    public GameObject leaderboard;
    public LeaderboardPlayer leaderboardPlayerDisplay;

    public GameObject endScreen;

    public TMP_Text timerText;

    public GameObject optionsScreen;
    public GameObject EndPanel;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.Escape))
        // {
        //     ShowHideOptions();
        // }

        // if (optionsScreen.activeInHierarchy && Cursor.lockState != CursorLockMode.None)
        // {
        //     Cursor.lockState = CursorLockMode.None;
        //     Cursor.visible = true;
        // }
    }

    public void ShowHideOptions()
    {
        if (!optionsScreen.activeInHierarchy)
        {
            optionsScreen.SetActive(true);
        }
        else
        {
            optionsScreen.SetActive(false);
        }
    }

    public void ReturnToMainMenu()
    {
       // byte sort = 0;
        // sort = PhotonNetwork.CurrentRoom.PlayerCount;

    
        //  ReadScore(sort);
        // DatabaseManager.instance.logtext.text ="sort değeri " + sort;




        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.LeaveRoom();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void ReadScore(byte sort)
    {


        if (sort == 10)
        {
            earnedExp = 5;
        }
        else if (sort == 9)
        {
            earnedExp = 10;
        }
        else if (sort == 8)
        {
            earnedExp = 20;
        }
        else if (sort == 7)
        {
            earnedExp = 30;
        }
        else if (sort == 6)
        {
            earnedExp = 40;
        }
        else if (sort == 5)
        {
            earnedExp = 50;
        }
        else if (sort == 4)
        {
            earnedExp = 60;
        }
        else if (sort == 3)
        {
            earnedExp = 70;
        }
        else if (sort == 2)
        {
            earnedExp = 80;
        }
        else if (sort == 1)
        {
            earnedExp = 90;
        }
        else if (sort == 0)
        {
            earnedExp = 100;
        }
        else
        {
            earnedExp = 0;
        }



        earnedValueText.text = earnedExp.ToString();


        DatabaseManager.instance.SendExp(earnedExp);

    }

    public void LoadMainScene()
    {

        SceneManager.LoadScene(0);
    }
}
