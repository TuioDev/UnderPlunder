using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI UI_LevelCrystals;
    [SerializeField] private GameObject UI_GameOver;
    [SerializeField] private GameObject UI_Win;
    
    public int AmountOfCrystals { get; private set; }

    private void Start()
    {
        AmountOfCrystals = FindObjectsOfType<Crystal>().Length;
        UI_LevelCrystals.text = AmountOfCrystals.ToString();
    }
    private void OnEnable()
    {
        UnpauseGame();
        Player.OnGameOver += EndGameLosing;
        Player.OnVictory += EndGameWinning;
    }
    private void OnDisable()
    {
        Player.OnGameOver -= EndGameLosing;
        Player.OnVictory -= EndGameWinning;
    }
    public void PauseGame()
    {
        Time.timeScale = 0.0f;
    }   
    public void UnpauseGame()
    {
        Time.timeScale = 1.0f;
    }
    private void EndGameLosing(bool isOver)
    {
        if (isOver)
        {
            PauseGame();
            UI_GameOver.SetActive(true);
        }
        else
        {
            UI_GameOver.SetActive(false);
            UnpauseGame();
        }
    }
    private void EndGameWinning(bool isOver)
    {
        if (isOver)
        {
            PauseGame();
            
            UI_Win.SetActive(true);
        }
        else
        {
            UnpauseGame();
            UI_Win.SetActive(false);
        }
    }
}