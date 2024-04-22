using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyDisplayItem : MonoBehaviour
{
    [SerializeField] private Image m_icon;
    [SerializeField] private TextMeshProUGUI m_amount;

    private Currency last = null;

    public void SetData(CurrencyAmount curr)
    {
        if (last == null || last.Type != curr.CurrencyType)
            last = Main.Instance.GetManager<ProfileManager>().GetCurrencyData(curr.CurrencyType);

        m_amount.text = $"+{curr.Amount}{last.Extension}";
        m_amount.color = last.Color;
    }
}