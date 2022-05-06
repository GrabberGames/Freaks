using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeInfoUI : MonoBehaviour
{
    [SerializeField] private Image waveGauge;
    [SerializeField] private Text playTimeText;

    public void WaveGaugeVisualization(float beforeSpawnTime, float nowPlayTime, float spawnIntervalTime)
    {
        waveGauge.fillAmount = 1.0f - ((nowPlayTime - beforeSpawnTime) / spawnIntervalTime);
    }

    public void PlayTimeTextVisualization(int nowPlayTime)
    {
        playTimeText.text = string.Format("{0:D1}:{1:D2}", (int)(nowPlayTime / 60), (int)(nowPlayTime % 60));
    }
}
