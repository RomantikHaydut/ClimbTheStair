using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class StairController : MonoBehaviour
{
    private TMP_Text moneyText;
    private Canvas stairCanvas;

    PlayerController playerController;
    PowerupController powerupController;

    private void Awake()
    {
        stairCanvas = GetComponentInChildren<Canvas>();
        stairCanvas.worldCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>(); // UI Camera

        powerupController = FindObjectOfType<PowerupController>();
        playerController = FindObjectOfType<PlayerController>();

        powerupController.MoneyIncrease(playerController.income); // Money increasing here.

        moneyText = GetComponentInChildren<TMP_Text>();
        moneyText.text = playerController.income.ToString("f1");
    }

    private void Update()
    {
        DistanceControlFromPlayer();
    }

    private void DistanceControlFromPlayer()
    {
        float distance = playerController.gameObject.transform.position.y - transform.position.y;
        float destroyDistance = 3f;
        if (distance >= destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}
