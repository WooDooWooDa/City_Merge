using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionData
{
    public bool Lock = false;

    public string Title;
    public Sprite Icon;
    public string Description;
}

public class CollectionItem : MonoBehaviour
{
    [SerializeField] private Sprite m_lockIcon;
    [SerializeField] private TextMeshProUGUI m_title;
    [SerializeField] private Image m_icon;

    public void SetData(CollectionData data)
    {
        if (data.Lock) {
            m_title.text = "????";
            m_icon.sprite = m_lockIcon;
        } else {
            m_title.text = data.Title;
            m_icon.sprite = null;
        }
    }
}
