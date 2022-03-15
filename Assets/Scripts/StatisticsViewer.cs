using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatisticsViewer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _stats;

    private void Start()
    {
        UpdateTotalGameplayTime();
    }

    private void UpdateTotalGameplayTime()
    {
        int totalSeconds = (int)GameData.TotalGameplayTime;
        int h = totalSeconds / 3600;
        int m = totalSeconds / 60 - h * 3600;
        _stats.text = $"¬рем€ игры: {h}ч. {m}мин.";
    }
}
