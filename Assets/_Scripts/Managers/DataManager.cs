using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Best Score
    public void SetBestScore(float score)
    {
        PlayerPrefs.SetFloat(SceneManager.GetActiveScene().buildIndex + "score", score);
    }

    public float GetBestScore()
    {
        return PlayerPrefs.GetFloat(SceneManager.GetActiveScene().buildIndex + "score", 0);
    }

    // Pole count for best score pole.
    public void SetPoleCount(int amount)
    {
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().buildIndex + "poleCount", amount);
    }

    public int GetPoleCount()
    {
        return PlayerPrefs.GetInt(SceneManager.GetActiveScene().buildIndex + "poleCount", 0);
    }

    // Total Money

    public void SetTotalMoney(float totalMoney)
    {
        PlayerPrefs.SetFloat("TotalMoney", totalMoney);
    }

    public float GetTotalMoney()
    {
        return PlayerPrefs.GetFloat("TotalMoney", 0);
    }

    // Powerup levels.

    public void SetStaminaLevel(int level)
    {
        PlayerPrefs.SetInt("StaminaLevel", level);
    }
    public void SetIncomeLevel(int level)
    {
        PlayerPrefs.SetInt("IncomeLevel", level);
    }
    public void SetSpeedLevel(int level)
    {
        PlayerPrefs.SetInt("SpeedLevel", level);
    }
    public int GetStaminaLevel()
    {
        return PlayerPrefs.GetInt("StaminaLevel", 0);
    }
    public int GetIncomeLevel()
    {
        return PlayerPrefs.GetInt("IncomeLevel", 0);
    }
    public int GetSpeedLevel()
    {
        return PlayerPrefs.GetInt("SpeedLevel", 0);
    }

    // Powerup money prices.
    public void SetStaminaMoneyPrice(int money)
    {
        PlayerPrefs.SetInt("StaminaPrice", money);
    }
    public void SetIncomeMoneyPrice(int money)
    {
        PlayerPrefs.SetInt("IncomePrice", money);
    }
    public void SetSpeedMoneyPrice(int money)
    {
        PlayerPrefs.SetInt("SpeedPrice", money);
    }
    public int GetStaminaMoneyPrice()
    {
        return PlayerPrefs.GetInt("StaminaPrice", 1);
    }
    public int GetIncomeMoneyPrice()
    {
        return PlayerPrefs.GetInt("IncomePrice", 1);
    }
    public int GetSpeedMoneyPrice()
    {
        return PlayerPrefs.GetInt("SpeedPrice", 1);
    }

    // Last level
    public void SetLastLevel(int level)
    {
        PlayerPrefs.SetInt("level", level);
    }

    private int GetLastLevel()
    {
        int level = PlayerPrefs.GetInt("level", 1);
        return level;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(GetLastLevel());
    }
    public void ResetEveryThing()
    {
        SetSpeedMoneyPrice(1); SetIncomeMoneyPrice(1); SetStaminaMoneyPrice(1); SetSpeedLevel(0); SetIncomeLevel(0); SetStaminaLevel(0); SetTotalMoney(0); SetPoleCount(0); SetBestScore(0); SetLastLevel(1);
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) // Reset all best scores in all scenes.
        {
            PlayerPrefs.SetInt(i + "poleCount", 0);
            PlayerPrefs.SetFloat(i + "score", 0);
        }
    }

}
