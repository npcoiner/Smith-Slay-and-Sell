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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //score_txt = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update() { }

    public void UpdateScore(GameObject inputItem)
    {
        Debug.Log("score update called");
        if (inputItem != null)
        {
            score += 5;
            score_txt.text = "Score: " + score;
            Debug.Log(score);
        }
    }
}
