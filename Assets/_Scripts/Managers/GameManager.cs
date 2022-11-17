using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private LevelGenerator levelGenerator;

    private UIManager uiManager;

    public bool isGameStarted;

    public bool isGameOver;

    public bool isLevelFinished;

    public int level;

    #region Levels Win Score Requirements
    [Header("Levels Win Score Requirements")]
    public List<int> ScoreForWinLevelList = new List<int>();

    private int scoreForActiveLevel;
    #endregion


    #region Panels
    public GameObject optionsPanel;
    public GameObject gameOverPanel;
    public GameObject gameWinPanel;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        levelGenerator = FindObjectOfType<LevelGenerator>();
        uiManager = FindObjectOfType<UIManager>();

        isLevelFinished = false;
        isGameOver = false;
        isGameStarted = false;

        gameOverPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    private int GetLevel()
    {
        level = SceneManager.GetActiveScene().buildIndex;
        return level;
    }
    public int GoalScore()
    {
        level = SceneManager.GetActiveScene().buildIndex;
        for (int i = 1; i <= ScoreForWinLevelList.Count; i++)
        {
            if (i == level)
            {
                scoreForActiveLevel = ScoreForWinLevelList[i - 1];
            }
        }

        if (scoreForActiveLevel < 100)
        {
            scoreForActiveLevel = 500;
        }

        return scoreForActiveLevel;
    }
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void GameWin()
    {
        DataManager.Instance.SetLastLevel(GetLevel() + 1);
        gameWinPanel.SetActive(true);
        isLevelFinished = true;
        isGameOver = true;
    }

    public void GameStarted()
    {
        isGameStarted = true;
        optionsPanel.SetActive(false);
    }

    public void RestartLevel()
    {
        StartCoroutine(LoadSameLevel());
    }

    public void NextLevel()
    {
        StartCoroutine(LoadNextLevel());
    }

    public void ContinueLevel()
    {
        StartCoroutine(FindObjectOfType<PlayerController>().DissolveReverse());
        GameManager.Instance.isGameOver = false;
        gameOverPanel.SetActive(false);
    }

    public void StairEvents()// Break all stairs.
    {
        levelGenerator.GameOver();
    }

    private IEnumerator LoadSameLevel() // Restart
    {
        StairEvents();
        uiManager.LevelEndClose();
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(GetLevel());
    }
    private IEnumerator LoadNextLevel() // Loads first level after last level.
    {
        StairEvents();
        uiManager.LevelEndClose();
        yield return new WaitForSecondsRealtime(1f);
        if (GetLevel() < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(GetLevel() + 1);
        }
        else
        {
            SceneManager.LoadScene(1); 
        }
        
    }
}
