using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonType
{
    Confirm,
    Cancel
}

[CreateAssetMenu(fileName = "NewButtonInfo", menuName = "Config/Screen/ButtonInfo")]
public class ButtonInfo : ScriptableObject
{
    public string Text;
    public Button Button;
    public ButtonType Type;

    public Action OnClickAction;
}