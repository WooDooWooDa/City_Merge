using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class CurrencyDisplayer: MonoBehaviour
{
    [SerializeField] private Transform m_orbit;
    [SerializeField] private CurrencyDisplayItem m_textPrefab;

    private Queue<CurrencyAmount> currenciesToDisplay = new Queue<CurrencyAmount>();
    private bool m_isDisplaying = false;

    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }

    public void SetData(CurrencyAmount currency)
    {
        currenciesToDisplay.Enqueue(currency);
        DisplayNext();
    }

    public void SetData(List<CurrencyAmount> currencies)
    {
        foreach (CurrencyAmount currency in currencies) {
            currenciesToDisplay.Enqueue(currency);
        }
        DisplayNext();
    }

    private void DisplayNext()
    {
        if (currenciesToDisplay.Count > 0 && !m_isDisplaying) {
            m_isDisplaying = true;
            CurrencyDisplayItem newText = Instantiate(m_textPrefab, m_orbit);
            newText.SetData(currenciesToDisplay.Dequeue());
            LeanTween.moveLocalY(newText.gameObject, 0.2f, 0.40f).setOnComplete(() => {
                m_isDisplaying = false;
                DisplayNext();
                Destroy(newText.gameObject);
            });
        }
    }
}
