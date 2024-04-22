using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockOpenInfo : OpenInfo
{
    public string Title;
    public string ItemName;
    public Sprite Icon;
}

public class UnlockScreen : UIScreenBase
{
    [Header("Specific")]
    [SerializeField] private string m_msgPlaceholder;
    [SerializeField] private TextMeshProUGUI m_title;
    [SerializeField] private TextMeshProUGUI m_content;

    [SerializeField] private Image m_icon;

    protected override void Awake()
    {
        base.Awake();
    }

    public override UIScreenBase Open(OpenInfo openInfo)
    {
        base.Open(openInfo);

        var data = openInfo as UnlockOpenInfo;
        if (data is UnlockOpenInfo) {
            m_title.text = data.Title;
            m_content.text = GenerateMsg(data.ItemName);
            m_icon.sprite = data.Icon;
        }

        return this;
    }

    private string GenerateMsg(string name)
    {
        return m_msgPlaceholder.Replace("[Celebration]", RandomCelebration()).Replace("[Item]", name);
    }

    private string RandomCelebration()
    {
        //TODO add more celebration msg when unlocking
        return "Hourrays!";
    }
}
