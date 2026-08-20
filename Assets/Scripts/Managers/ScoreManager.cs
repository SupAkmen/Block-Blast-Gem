using System;
using UnityEngine;

public class ScoreManager : SingletonBehaviour<ScoreManager>
{
     public Action<int> OnScoreUpdated;
     public Action<int> OnHighScoreUpdated;

     [SerializeField] private int baseLineScore = 10;

     private int currentScore = 0;
     private int highScore = 0;

     public override void Awake()
     {
          base.Awake();
          highScore = PlayerPrefs.GetInt("HighScore",0);
     }
     
     public int CurrentScore => currentScore;
     public int HighScore => highScore;

     public void AddScore(int linesCleared,int currentCombo)
     {
          if (linesCleared <= 0) return;

          float comboMultiplier = 1f + (currentCombo * 0.5f);
          int points = Mathf.RoundToInt(linesCleared * baseLineScore * comboMultiplier);
          
          currentScore += points;
          OnScoreUpdated?.Invoke(currentScore);

          if (currentScore > highScore)
          {
               highScore = currentScore;
               PlayerPrefs.SetInt("HighScore",highScore);
               OnHighScoreUpdated?.Invoke(highScore);
          }
               
     }

     public void ResetScore()
     {
          currentScore = 0;
          OnScoreUpdated?.Invoke(currentScore);
     }
}
