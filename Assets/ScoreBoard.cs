using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
public static ScoreBoard instance;

    public Text scoreText;
    public Text highscoreText;

    int score=0;
    int highscore=0;

   private void Awake()
   {
    
   }

    void Start()
    {
        scoreText.text=score.ToString()+"point";
        highscoreText.text="HIGHSCORE"+highscore.ToString();
    }

   
    public void AddPoint(int addscore)
    {
        score += addscore;
        scoreText.text=score.ToString()+"point";
    }
}
