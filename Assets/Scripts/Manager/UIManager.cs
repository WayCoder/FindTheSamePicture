using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    [SerializeField] private Image darkPanel;

    [SerializeField] private TextMeshProUGUI titleText;

    [SerializeField] private TextMeshProUGUI buttonText;

    [SerializeField] private Image timerImage;

    [SerializeField] private TextMeshProUGUI timeText;

    [SerializeField] private GameObject scoreTextParent;

    [SerializeField] private TextMeshProUGUI[] resultText;

    [SerializeField] private Image[] healthBar;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);

            return;
        }

        instance = this;
    }

    public void SetDarkPanelUI(bool value)
    {
        darkPanel.gameObject.SetActive(value);
    }

    public void SetTitlePanelUI(bool value, string titleText = null, string buttonText = null)
    {
        SetDarkPanelUI(value);

        this.titleText.gameObject.SetActive(value);

        if (value)
        {
            this.titleText.text = titleText;

            this.buttonText.text = buttonText;
        }
    }
   
    public void SetTimerUI(bool value, float amount = 0f, int time = 0)
    {
        timerImage.gameObject.SetActive(value);

        if (value)
        {
            RuntimeDataSO data = GameManager.instance.data;

            timerImage.fillAmount = amount;
       
            timerImage.color = time <= data.gamestateData.gameplayTime / 2 ? data.uiData.timerRedColor : data.uiData.timerBaseColor;

            int seconds = time % 60;

            int minutes = (time / 60) % 60;

            timeText.text = $"{minutes:00}:{seconds:00}";

        }
    }

    public void SetResultUI(bool value, GameManager.Result result = GameManager.Result.Draw)
    {
        if (!value)
        {
            foreach(TextMeshProUGUI text in resultText)
            {
                text.gameObject.SetActive(false);
            }

            return;
        }

        UIData data = GameManager.instance.data.uiData;

        switch(result)
        {
            case GameManager.Result.Planet_A:
                resultText[(int)result].gameObject.SetActive(true);         
                resultText[(int)result].text = data.winnerText;
                resultText[(int)result].color = data.winnerTextColor;
                resultText[(int)GameManager.Result.Planet_B].gameObject.SetActive(true);
                resultText[(int)GameManager.Result.Planet_B].text = data.loserText;
                resultText[(int)GameManager.Result.Planet_B].color = data.loserTextColor;
                break;


            case GameManager.Result.Planet_B:
                resultText[(int)result].gameObject.SetActive(true);
                resultText[(int)result].text = data.winnerText;
                resultText[(int)result].color = data.winnerTextColor;
                resultText[(int)GameManager.Result.Planet_A].gameObject.SetActive(true);
                resultText[(int)GameManager.Result.Planet_A].text = data.loserText;
                resultText[(int)GameManager.Result.Planet_A].color = data.loserTextColor;
                break;


            case GameManager.Result.Draw:
                resultText[(int)result].gameObject.SetActive(true);
                break;
        }
    }

    public void SetHealthBarUI(bool value, int index = -1, float health = 0f, float maxHealth = 0f)
    {
        foreach (Image bar in healthBar)
        {
            bar.gameObject.SetActive(value);
        }

        if (index == -1)
        {
            return;
        }

        UIData data = GameManager.instance.data.uiData;

        TextMeshProUGUI textMesh = healthBar[index].GetComponentInChildren<TextMeshProUGUI>();

        Image image = healthBar[index].transform.GetChild(0).GetComponentInChildren<Image>();

        if (value && textMesh && image)
        {
            textMesh.text = health.ToString() + "/" + maxHealth.ToString();

            image.fillAmount = health / maxHealth;

            if (image.fillAmount <= data.planetWarningAmount)
            {
                image.color = image.fillAmount <= data.planetDangerAmount ? data.planetDangerColor : data.planetWarningColor;
            }
            else
            {
                image.color = data.planetSafeColor;
            }
        }

    }


    public void SetScoreTextUI(bool value, int index = -1, int score = 0)
    {
        scoreTextParent.gameObject.SetActive(value);

        if (index == -1)
        {
            return;
        }

        TextMeshProUGUI textMesh = scoreTextParent.transform.GetChild(index).GetComponent<TextMeshProUGUI>();

        if (value && textMesh)
        {
            textMesh.text = score.ToString();
        }
    }

}