using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    [SerializeField] private Slider m_loadingSlider;
    [SerializeField] private TextMeshProUGUI m_loadingInfoText;
    [SerializeField] private TextMeshProUGUI m_elipsisText;
    [SerializeField] private int m_elipsisCount = 3;

    public void Initialize(int maxSliderValue)
    {
        m_loadingSlider.maxValue = maxSliderValue;
        m_loadingSlider.value = 1;
    }

    private void Start()
    {
        StartCoroutine(nameof(LoadingElipsis));
    }

    public void UpdateValue(float newValue)
    {
        m_loadingSlider.value = newValue;
    }

    public void UpdateInfo(string newInfo, bool showElipsis = true)
    {
        m_loadingInfoText.text = newInfo;
        m_elipsisText.gameObject.SetActive(showElipsis);
    }

    private void OnDestroy()
    {
        StopCoroutine(nameof(LoadingElipsis));
    }

    private IEnumerator LoadingElipsis()
    {
        while (true) {
            if (m_elipsisText.text.Length >= m_elipsisCount)
                m_elipsisText.text = "";
            m_elipsisText.text += ".";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
