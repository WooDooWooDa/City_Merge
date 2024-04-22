using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewPopupInfo", menuName = "Config/Screen/PopupInfo")]
public class PopupInformations : ScriptableObject
{
    [Header("Informations")]
    public string Title;
    public Sprite Icon;
    [TextArea(2, 5)]
    public string Content;
    public bool IconCenter = false;
    public ButtonInfo[] buttons;
}
