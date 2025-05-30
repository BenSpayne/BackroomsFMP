using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseSensitivityManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider sensitivitySlider;      
    public TMP_Text sensitivityText;     

    [Header("Player Look Reference")]
    public PlayerLook playerLook;         

    private const string SensitivityPref = "MouseSensitivity";
    private const float minSensitivity = 0.5f;
    private const float maxSensitivity = 5f;

    void Start()
    {
        float savedSliderValue = PlayerPrefs.GetFloat(SensitivityPref, 50f);
        sensitivitySlider.value = savedSliderValue;
        ApplySensitivity(savedSliderValue);

        sensitivitySlider.onValueChanged.AddListener(ApplySensitivity);
    }

    private void ApplySensitivity(float sliderValue)
    {
        float mappedSensitivity = Mathf.Lerp(minSensitivity, maxSensitivity, sliderValue / 100f);

        if (playerLook != null)
        {
            playerLook.mouseSensitivity = mappedSensitivity;
        }

        sensitivityText.text = sliderValue.ToString("F1");

        PlayerPrefs.SetFloat(SensitivityPref, sliderValue);
        PlayerPrefs.Save();
    }
}