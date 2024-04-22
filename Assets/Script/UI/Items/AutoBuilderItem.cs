using System;
using UnityEngine;
using UnityEngine.UI;

public class AutoBuilderItem : MonoBehaviour
{
    [SerializeField] private Sprite m_pauseImg;
    [SerializeField] private Sprite m_stopImg;
    [Space]
    [SerializeField] private Button m_toggleAutoOff;
    [SerializeField] private Button m_toggleAutoOn;
    [SerializeField] private Slider m_levelSlider;
    [SerializeField] private Image m_pauseStopImage;

    private AutoBuilderUpgrade m_upgrade;

    public void Initialize(AutoBuilderUpgrade upgrade)
    {
        m_upgrade = upgrade;
        m_toggleAutoOn.onClick.AddListener(m_upgrade.ToggleActive);
        m_toggleAutoOff.onClick.AddListener(m_upgrade.ToggleActive);
    }

    public void OnToggle(bool isActive)
    {
        m_toggleAutoOn.gameObject.SetActive(!isActive);
        m_levelSlider.gameObject.SetActive(isActive);
        if (isActive) {
            Pause(false);
        }
    }

    public void OnChangeTime(float newTime)
    {
        m_levelSlider.value = newTime;
    }

    public void Pause(bool paused)
    {
        //TODO change this pause and stop btn 
        m_pauseStopImage.sprite = paused ? m_pauseImg : m_stopImg ;
    }
}
