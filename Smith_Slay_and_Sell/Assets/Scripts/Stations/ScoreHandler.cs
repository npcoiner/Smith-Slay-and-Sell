using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    public int score = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(GameObject inputItem)
    {
	Debug.Log("score update called");
	if (inputItem != null)
	{
	    score += 5;
	    Debug.Log(score);
	}
    }

}
