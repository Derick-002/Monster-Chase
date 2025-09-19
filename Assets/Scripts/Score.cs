using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour
{
    public int score = 0;
    public Text scoreText;

    public Text GameOverText; 
    [HideInInspector]
    public bool isGameOver = false;

    public int pointsToAdd = 1;
    public float startInterval = 2f;
    public float minInterval = 0.2f;
    public float speedUpFactor = 0.95f;

    private float currentInterval;

    void Start()
    {
        currentInterval = startInterval;
        UpdateScoreText();
        StartCoroutine(ScoreRoutine());
        GameOverText.gameObject.SetActive(false); // hide at start
    }

    IEnumerator ScoreRoutine()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(currentInterval);

            score += pointsToAdd;
            UpdateScoreText();

            currentInterval *= speedUpFactor;
            if (currentInterval < minInterval)
                currentInterval = minInterval;
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = $"{score} Score";
    }
}
