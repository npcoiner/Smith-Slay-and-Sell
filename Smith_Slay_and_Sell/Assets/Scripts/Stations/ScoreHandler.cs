using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour
{
    public int score = 0;

    [SerializeField]
    private TMP_Text score_txt;

    public static ScoreHandler Instance { get; private set; }

    public void Awake()
    {
        //Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateScore()
    {
        Debug.Log("score update called");
        score += 5;
        score_txt.text = "Score: " + score;
        //Debug.Log(score);
    }

    public int GetScore()
    {
        return score;
    }
}
