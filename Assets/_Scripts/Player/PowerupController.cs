using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerupController : MonoBehaviour
{
    private UIManager uiManager;

    private Animator anim;

    public float totalMoney;
    #region  Powerup Levels
    [Header("Powerup Levels")]

    private int staminaLevel;
    private int incomeLevel;
    private int speedLevel;
    #endregion

    #region Prices
    private int staminaPrice;
    private int speedPrice;
    private int incomePrice;
    #endregion
    private void Awake()
    {
        anim = GetComponent<Animator>();
        uiManager = FindObjectOfType<UIManager>();
        GetPowerupLevels();
        GetTotalMoney();
        GetPrices();
    }

    private void HappyAnimation()
    {
        anim.ResetTrigger("Happy");
        anim.SetTrigger("Happy");
    }

    public void MoneyIncrease(float money)
    {
        totalMoney += money;
        DataManager.Instance.SetTotalMoney(totalMoney);
        uiManager.DisplayTotalMoney();
    }
    private void GetTotalMoney()
    {
        totalMoney = DataManager.Instance.GetTotalMoney();
    }
    private void SetTotalMoney()
    {
        DataManager.Instance.SetTotalMoney(totalMoney);
    }

    private void GetPrices()
    {
        staminaPrice = DataManager.Instance.GetStaminaMoneyPrice();
        speedPrice = DataManager.Instance.GetSpeedMoneyPrice();
        incomePrice = DataManager.Instance.GetIncomeMoneyPrice();
    }
    private void GetPowerupLevels()
    {
        staminaLevel = DataManager.Instance.GetStaminaLevel();
        speedLevel = DataManager.Instance.GetSpeedLevel();
        incomeLevel = DataManager.Instance.GetIncomeLevel();
    }

    // Powerup level increase..
    public void StaminaLevelIncrease()
    {
        GetPrices();
        if (totalMoney > staminaPrice)
        {
            staminaLevel++;
            totalMoney -= staminaPrice;

            DataManager.Instance.SetStaminaLevel(staminaLevel);
            SetTotalMoney();
            uiManager.DisplayTotalMoney();
            uiManager.DisplayPowerupLevels();

            SetNewStaminaPrice();
            uiManager.DisplayPrices();

            HappyAnimation();
        }
    }

    public void IncomeLevelIncrease()
    {
        GetPrices();
        if (totalMoney > incomePrice)
        {
            incomeLevel++;
            totalMoney -= incomePrice;

            DataManager.Instance.SetIncomeLevel(incomeLevel);
            SetTotalMoney();
            uiManager.DisplayTotalMoney();
            uiManager.DisplayPowerupLevels();

            SetNewIncomePrice();
            uiManager.DisplayPrices();

            HappyAnimation();

        }
    }

    public void SpeedLevelIncrease()
    {
        GetPrices();
        if (totalMoney > speedPrice)
        {
            speedLevel++;
            totalMoney -= speedPrice;

            DataManager.Instance.SetSpeedLevel(speedLevel);
            SetTotalMoney();
            uiManager.DisplayTotalMoney();
            uiManager.DisplayPowerupLevels();

            SetNewSpeedPrice();
            uiManager.DisplayPrices();

            HappyAnimation();
        }
    }
    // Get Levels
    public int GetStaminaLevel()
    {
        if (staminaLevel == 0)
        {
            return 1;
        }
        else
        {
            return staminaLevel;
        }
    }

    public int GetSpeedLevel()
    {
        if (speedLevel == 0)
        {
            return 1;
        }
        else
        {
            return speedLevel;
        }
    }

    public int GetIncomeLevel()
    {
        if (incomeLevel == 0)
        {
            return 1;
        }
        else
        {
            return incomeLevel;
        }
    }

    // New prices.
    private void SetNewStaminaPrice()
    {
        int price = Mathf.Clamp(((staminaPrice + 2) * staminaPrice), 1, 9999);
        if (price > staminaPrice)
        {
            DataManager.Instance.SetStaminaMoneyPrice(price);
        }
        
    }
    private void SetNewSpeedPrice()
    {
        int price = Mathf.Clamp(((speedPrice + 2) * speedPrice), 1, 9999);
        if (price > speedPrice)
        {
            DataManager.Instance.SetSpeedMoneyPrice(price);
        }

    }
    private void SetNewIncomePrice()
    {
        int price = Mathf.Clamp(((incomePrice + 2) * incomePrice),1,9999);
        if (price > incomePrice)
        {
            DataManager.Instance.SetIncomeMoneyPrice(price);
        }

    }
}
