using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    private PlayerController playerController;

    private PowerupController powerupController;

    public TMP_Text levelText;

    public TMP_Text scoreText;

    public TMP_Text startText;

    public TMP_Text moneyText;

    public GameObject waterOption;

    public Image waterTimerBar;

    public Image progressBar;

    private int goalScore;

    private float activeScore;

    [SerializeField] Animator closeImageAnim;

    #region Powerup Variables
    // Stamina 
    public TMP_Text staminaLevel;
    public TMP_Text staminaMoneyPrice;
    // Income
    public TMP_Text incomeLevel;
    public TMP_Text incomeMoneyPrice;
    // Speed
    public TMP_Text speedLevel;
    public TMP_Text speedMoneyPrice;
    #endregion



    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        powerupController = GameObject.FindGameObjectWithTag("Player").GetComponent<PowerupController>();

        // Start displays.
        DisplayPrices();
        DisplayPowerupLevels();
        DisplayTotalMoney();
        DisplayScore();
        DisplayLevel();

        StartCoroutine(DisplayStartText());
    }

    private void Start()
    {
        goalScore = GameManager.Instance.GoalScore();

    }

    void Update()
    {
        if (GameManager.Instance.isGameStarted)
        {
            CloseStartText();
            DisplayScore();
            ProgressBarControl();
        }
    }

    public IEnumerator DisplayStartText()
    {
        if (startText != null)
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                startText.gameObject.SetActive(true);
                startText.text = "Press Space For Start";
                yield return new WaitForSecondsRealtime(4f);
                startText.text = "Left Mouse Button For Climb";
                yield return new WaitForSecondsRealtime(4f);
                startText.gameObject.SetActive(false);
            }
            else
            {
                startText.gameObject.SetActive(false);
            }
        }
        yield break;
    }

    public void CloseStartText()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startText.gameObject.SetActive(false);
            }
        }
    }

    private void DisplayLevel()
    {
        levelText.text = "Level " + SceneManager.GetActiveScene().buildIndex.ToString();
    }

    public void DisplayTotalMoney()
    {
        moneyText.text = DataManager.Instance.GetTotalMoney().ToString("f0");
    }

    public void DisplayScore()
    {
        scoreText.text = playerController.GetScore().ToString("f1") + "m";
    }

    public void DisplayBestScore(GameObject textObject)
    {
        textObject.GetComponentInChildren<TMP_Text>().text = DataManager.Instance.GetBestScore().ToString("f1") + "m";
    }


    public void DisplayPowerupLevels()
    {
        staminaLevel.text = "lvl " + DataManager.Instance.GetStaminaLevel().ToString();
        incomeLevel.text = "lvl " + DataManager.Instance.GetIncomeLevel().ToString();
        speedLevel.text = "lvl " + DataManager.Instance.GetSpeedLevel().ToString();
    }
    public void DisplayPrices()
    {
        staminaMoneyPrice.text = DataManager.Instance.GetStaminaMoneyPrice().ToString();
        incomeMoneyPrice.text = DataManager.Instance.GetIncomeMoneyPrice().ToString();
        speedMoneyPrice.text = DataManager.Instance.GetSpeedMoneyPrice().ToString();
    }

    private void ProgressBarControl()
    {
        activeScore = playerController.GetScore();
        float _fillAmount = Mathf.Clamp((activeScore / goalScore), 0, 1);
        progressBar.fillAmount = _fillAmount;
    }

    public void OpenWaterOption() // For powers on road.
    {
        waterTimerBar.fillAmount = 1f;
        StartCoroutine(WaterTimer());
        waterOption.SetActive(true);
    }

    public void CloseWaterOption()
    {
        waterOption.SetActive(false);
    }

    private IEnumerator WaterTimer()
    {
        float timeForClose = 3.5f;
        float maxTimeForClose = timeForClose;
        while (true)
        {
            yield return null;
            timeForClose -= Time.deltaTime;
            waterTimerBar.fillAmount = timeForClose / maxTimeForClose;
            if (waterTimerBar.fillAmount <= 0.03f)
            {
                playerController.DestroyWater();
                CloseWaterOption();
                yield break;
            }

        }
    }

    public void LevelEndClose()
    {
        closeImageAnim.SetTrigger("End");
    }
}
