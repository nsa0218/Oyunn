using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
    [SerializeField]
   public Text earnedValueText;
    public double earnedExp;
    public static EndScene instance;
    public byte sort;

    private void Awake()
    {
        instance = this;
        
    }
    public void ReadScore()
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
       
        
      
       earnedValueText.text =earnedExp.ToString();


        DatabaseManager.instance.SendExp(earnedExp);

    }

    public void LoadMainScene()
    {

        SceneManager.LoadScene(0);
    }

}
