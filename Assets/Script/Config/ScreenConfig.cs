using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "NewScreenConfig", menuName = "Config/Screen", order = 6)]
public class ScreenConfig : ScriptableObject
{
    public ScreenInformations[] ScreensInfo;

}
