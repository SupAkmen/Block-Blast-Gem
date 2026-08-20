using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
     [SerializeField] private TextMeshProUGUI scoreText;
     [SerializeField] private TextMeshProUGUI highscoreText;
     [SerializeField] private TextMeshProUGUI comboText;
     [SerializeField] private GameObject gameOverPanel;

     private void Start()
     {
          if (ScoreManager.instance != null)
          {
               ScoreManager.instance.OnScoreUpdated += UpdateScoreUI;
               ScoreManager.instance.OnHighScoreUpdated += UpdateHighScoreUI;
               
               UpdateScoreUI(ScoreManager.instance.CurrentScore);
               UpdateHighScoreUI(ScoreManager.instance.HighScore);
          }

          if (ComboManager.instance != null)
          {
               ComboManager.instance.OnComboChanged += UpdateComboUI;
               UpdateComboUI(ComboManager.instance.CurrentCombo);
          }

          if (GameManager.instance != null)
          {
               GameManager.instance.OnGameOver += ShowGameOverPanel;
          }
          
          if(gameOverPanel != null)
               gameOverPanel.SetActive(false);
     }

     private void OnDestroy()
     {
          if (ScoreManager.instance != null)
          {
               ScoreManager.instance.OnScoreUpdated -= UpdateScoreUI;
               ScoreManager.instance.OnHighScoreUpdated -= UpdateHighScoreUI;
          }

          if (ComboManager.instance != null)
          {
               ComboManager.instance.OnComboChanged -= UpdateComboUI;
          }

          if (GameManager.instance != null)
               GameManager.instance.OnGameOver -= ShowGameOverPanel;
     }

     private void UpdateScoreUI(int newScore)
     {
          if (scoreText != null)
          {
               scoreText.text = newScore.ToString();
          }
     }

     private void UpdateHighScoreUI(int newHighScore)
     {
          if (highscoreText != null)
          {
               highscoreText.text = newHighScore.ToString();
          }
     }

     private void UpdateComboUI(int newCombo)
     {
          if (comboText != null)
          {
               if (newCombo > 0)
               {
                    comboText.text = $"x{newCombo}";
                    comboText.gameObject.SetActive(true);
               }
               else
               {
                    comboText.gameObject.SetActive(false);
               }
          }
     }

     private void ShowGameOverPanel(bool isWin)
     {
          Debug.Log("Lose");
          if (gameOverPanel != null)
          {
               gameOverPanel.SetActive(true);

          }
               
     }
}
