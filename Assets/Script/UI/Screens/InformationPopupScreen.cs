using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InformationPopupOpenInfo : OpenInfo
{
    public PopupInformations PopupInformations;
    public Action PositiveAction;
}

public class InformationPopupScreen : UIScreenBase
{
    [Header("Specific")]
    [SerializeField] private RectTransform m_panel;
    [SerializeField] private TextMeshProUGUI m_title;
    [SerializeField] private TextMeshProUGUI m_content;
    [SerializeField] private Image m_icon;
    [SerializeField] private Transform m_buttonsParent;

    protected override void Awake()
    {
        base.Awake();
    }

    public override UIScreenBase Open(OpenInfo openInfo)
    {
        base.Open(openInfo);

        var data = openInfo as InformationPopupOpenInfo;
        if (data is InformationPopupOpenInfo) {
            m_title.text = data.PopupInformations.Title;
            m_content.text = GenerateMsg(data.PopupInformations.Content);
            
            m_icon.gameObject.SetActive(data.PopupInformations.Icon != null);
            m_icon.sprite = data.PopupInformations.Icon;
            AddButtons(data.PopupInformations.buttons, data.PositiveAction);
        }
        m_needsRefresh = true;

        return this;
    }

    public override void OnClose()
    {
        base.OnClose();

        Button[] buttons = m_buttonsParent.GetComponentsInChildren<Button>();
        foreach (Button button in buttons) {
            button.onClick.RemoveAllListeners();
        }
    }

    private void AddButtons(ButtonInfo[] buttons, Action positiveAction)
    {
        foreach (ButtonInfo button in buttons) {
            Button newBtn = Instantiate(button.Button, m_buttonsParent);
            newBtn.GetComponentInChildren<TextMeshProUGUI>().text = button.Text;

            if (button.Type == ButtonType.Confirm)
                newBtn.onClick.AddListener(new UnityAction(positiveAction));

            newBtn.onClick.AddListener(Close);
        }
    }

    private string GenerateMsg(string name)
    {
        return name;
    }
}
