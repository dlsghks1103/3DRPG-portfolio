using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInGameUI : MonoBehaviour
{
    public StatsObject statsObject;

    public Text levelText;
    public Text healthText;
    public Text manaText;
    public Text expText;
    public Text goldText;
    public Slider expSlider;
    public Slider healthSlider;
    public Slider manaSlider;

    void Start()
    {
        levelText.text = statsObject.Level.ToString("n0");

        healthSlider.minValue = 0.0f;
        manaSlider.minValue = 0.0f;
        expSlider.minValue = 0.0f;

        healthSlider.maxValue = 1;
        manaSlider.maxValue = 1;
        expSlider.maxValue = 1;

        healthText.text = statsObject.HealthState;
        manaText.text = statsObject.ManaState;
        levelText.text = statsObject.Level.ToString("n0");
        expText.text = statsObject.ExpState;
        goldText.text = statsObject.Gold.ToString();
    }

    private void OnEnable()
    {
        statsObject.OnChangedStats += OnChangedStats;
        statsObject.OnChangedExp += OnChangedExp;
    }

    private void OnDisable()
    {
        statsObject.OnChangedStats -= OnChangedStats;
        statsObject.OnChangedExp -= OnChangedExp;
    }

    private void OnChangedStats(StatsObject statsObject)
    {
        healthSlider.value = statsObject.HealthPercentage;
        manaSlider.value = statsObject.ManaPercentage;
        healthText.text = statsObject.HealthState;
        manaText.text = statsObject.ManaState;
        goldText.text = statsObject.Gold.ToString();
    }

    private void OnChangedExp(StatsObject statsObject)
    {
        expSlider.value = statsObject.ExpPercentage;
        levelText.text = statsObject.Level.ToString("n0");
        expText.text = statsObject.ExpState;
    }
}
