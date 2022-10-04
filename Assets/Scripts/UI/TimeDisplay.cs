using System.Globalization;
using TMPro;
using UnityEngine;
using ZnZUtil;

public class TimeDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText, modifierText, waveTitle, waveText;

    public void UpdateTime(int time) => timeText.text = $"{time}s";
    public void UpdateModifier(float mod) => modifierText.text = $"x{mod.ToString(CultureInfo.InvariantCulture)}";

    public void UpdateWaveCount(int count) => waveTitle.text = $"Wave {count} in:";
    public void UpdateWaveCountdown(float countdown) => waveText.text = $"{countdown.Round(0)}s";
}