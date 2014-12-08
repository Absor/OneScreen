using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class HighScore : MonoBehaviour {

    [SerializeField]
    private Text scoreText;

    private List<float> currentTop;

    void Start()
    {
        currentTop = new List<float>();
        
        int index = 0;
        while (PlayerPrefs.HasKey("score" + index) && index < 5)
        {
            float time = PlayerPrefs.GetFloat("score" + index);
            currentTop.Add(time);
            index++;
        }

        drawScores();
    }

    private void drawScores()
    {
        string scores = "";

        for (int i = 0; i < Mathf.Min(currentTop.Count, 5); i++)
        {
            float time = currentTop[i];
            scores += String.Format("{0}.  {1:0.##} sec\n", (i + 1), time);
        }
        scoreText.text = scores;
    }

    public void AddTime(float time)
    {
        currentTop.Add(time);
        currentTop.Sort();

        for (int i = 0; i < Mathf.Min(currentTop.Count, 5); i++)
        {
            PlayerPrefs.SetFloat("score" + i, currentTop[i]);
        }
        PlayerPrefs.Save();
        drawScores();
    }
}
